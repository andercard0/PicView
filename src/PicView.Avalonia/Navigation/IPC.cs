﻿using System.Diagnostics;
using System.IO.Pipes;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using PicView.Avalonia.ViewModels;

namespace PicView.Avalonia.Navigation;

/// <summary>
/// Provides Inter-Process Communication (IPC) functionality using named pipes.
/// Allows multiple instances of the application to communicate, 
/// enabling the transfer of commands or data between them.
/// </summary>
internal static class IPC
{
    /// <summary>
    /// The default name for the named pipe used by the application.
    /// This pipe is used to facilitate communication between instances of the application.
    /// </summary>
    internal const string PipeName = "PicViewPipe";

    /// <summary>
    /// Sends an argument to a running instance of the application via the specified named pipe.
    /// If a running instance is detected, the argument (e.g., a file path) is passed to it for processing.
    /// </summary>
    /// <param name="arg">The argument to send to the running instance, such as a file path.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <c>true</c> if the argument is sent successfully.</returns>
    /// <remarks>
    /// This method tries to connect to an existing instance of the application using a named pipe. If successful, 
    /// it sends the argument. In case of a timeout or other exceptions, these errors are caught and logged in debug mode.
    /// This can be used to pass new command-line arguments to a running instance instead of starting a new instance.
    /// </remarks>
    internal static async Task<bool> SendArgumentToRunningInstance(string arg)
    {
        await using var pipeClient = new NamedPipeClientStream(PipeName);
        try
        {
            // Try to connect to the running instance
            await pipeClient.ConnectAsync(2750).ConfigureAwait(false);

            // Send the argument
            await using var writer = new StreamWriter(pipeClient);
            await writer.WriteLineAsync(arg).ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            // Log the timeout if in debug mode
#if DEBUG
            Trace.WriteLine($"{nameof(SendArgumentToRunningInstance)} timeout");
#endif
            return false;
        }
        catch (Exception ex)
        {
            // Log the exception if in debug mode
#if DEBUG
            Trace.WriteLine($"{nameof(SendArgumentToRunningInstance)} exception: \n{ex}");
#endif
            return false;
        }
        return true;
    }

    /// <summary>
    /// Starts a named pipe server to listen for incoming arguments from other instances of the application.
    /// Processes incoming arguments (e.g., file paths) by instructing the main view model to open the specified picture.
    /// </summary>
    /// <param name="vm">The main view model that processes the received argument, typically loading a picture.</param>
    /// <returns>A task that represents the asynchronous operation. The method runs indefinitely to handle multiple connections.</returns>
    /// <remarks>
    /// This method runs continuously, waiting for incoming connections on the specified named pipe. When a connection is made, 
    /// it reads the incoming arguments and processes them. The arguments can include file paths or commands, 
    /// and they are passed to the main view model to update the UI accordingly.
    /// </remarks>
    internal static async Task StartListeningForArguments(MainViewModel vm)
    {
        while (true) // Continuously listen for incoming connections
        {
            await using var pipeServer = new NamedPipeServerStream(PipeName);

            try
            {
                // Wait for a connection from another instance
                await pipeServer.WaitForConnectionAsync();

                using var reader = new StreamReader(pipeServer);

                // Read and process incoming arguments
                while (await reader.ReadLineAsync() is { } line)
                {
                    // Log the received argument if in debug mode
#if DEBUG
                    Trace.WriteLine("Received argument: " + line);
#endif
                    // Need to stop taskbar progress if it's running
                    // Otherwise the new taskbar progress will not be updated
                    vm.PlatformService.StopTaskbarProgress();
                    await Dispatcher.UIThread.InvokeAsync(() => 
                    {
                        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                        {
                            return;
                        }
                        // Activating the window works fine in debug mode, but not in AOT release mode 
                        desktop.MainWindow.Activate();
                    });
                    await NavigationHelper.LoadPicFromStringAsync(line, vm).ConfigureAwait(false);

                }
            }
            catch (Exception ex)
            {
                // Log any exceptions encountered while processing arguments
#if DEBUG
                Trace.WriteLine($"{nameof(StartListeningForArguments)} exception: \n{ex}");
#endif
            }
        }
        // ReSharper disable once FunctionNeverReturns
    }
}
