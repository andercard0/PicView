﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using ImageMagick;
using PicView.Avalonia.Helpers;
using PicView.Avalonia.Models;
using PicView.Avalonia.Navigation;
using PicView.Avalonia.Services;
using PicView.Avalonia.Views.UC;
using PicView.Core.Calculations;
using PicView.Core.Config;
using PicView.Core.FileHandling;
using PicView.Core.ImageDecoding;
using PicView.Core.Localization;
using PicView.Core.Navigation;
using PicView.Core.ProcessHandling;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Runtime.InteropServices;
using System.Windows.Input;
using PicView.Core.ImageTransformations;

namespace PicView.Avalonia.ViewModels
{
    public class MainViewModel : ViewModelBase, IActivatableViewModel
    {
        public ViewModelActivator? Activator { get; }

        public event EventHandler? ImageChanged;

        private void UpdateLanguage()
        {
            SelectFile = TranslationHelper.GetTranslation("OpenFileDialog");
            OpenLastFile = TranslationHelper.GetTranslation("OpenLastFile");
            FilePaste = TranslationHelper.GetTranslation("FilePaste");
            Copy = TranslationHelper.GetTranslation("Copy");
            Reload = TranslationHelper.GetTranslation("Reload");
            Print = TranslationHelper.GetTranslation("Print");
            DeleteFile = TranslationHelper.GetTranslation("DeleteFile");
            Save = TranslationHelper.GetTranslation("Save");
            CopyFile = TranslationHelper.GetTranslation("CopyFile");
            NewWindow = TranslationHelper.GetTranslation("NewWindow");
            Close = TranslationHelper.GetTranslation("Close");
            Open = TranslationHelper.GetTranslation("Open");
            OpenFileDialog = TranslationHelper.GetTranslation("OpenFileDialog");
            ShowInFolder = TranslationHelper.GetTranslation("ShowInFolder");
            OpenWith = TranslationHelper.GetTranslation("OpenWith");
            RenameFile = TranslationHelper.GetTranslation("RenameFile");
            DuplicateFile = TranslationHelper.GetTranslation("DuplicateFile");
            RotateLeft = TranslationHelper.GetTranslation("RotateLeft");
            RotateRight = TranslationHelper.GetTranslation("RotateRight");
            Flip = TranslationHelper.GetTranslation("Flip");
            UnFlip = TranslationHelper.GetTranslation("Unflip");
            ShowBottomGallery = TranslationHelper.GetTranslation("ShowBottomGallery");
            HideBottomGallery = TranslationHelper.GetTranslation("HideBottomGallery");
            AutoFitWindow = TranslationHelper.GetTranslation("AutoFitWindow");
            Stretch = TranslationHelper.GetTranslation("Stretch");
            Crop = TranslationHelper.GetTranslation("Crop");
            ResizeImage = TranslationHelper.GetTranslation("ResizeImage");
            GoToImageAtSpecifiedIndex = TranslationHelper.GetTranslation("GoToImageAtSpecifiedIndex");
            ToggleScroll = TranslationHelper.GetTranslation("ToggleScroll");
            ScrollEnabled = TranslationHelper.GetTranslation("ScrollEnabled");
            ScrollDisabled = TranslationHelper.GetTranslation("ScrollDisabled");
            Slideshow = TranslationHelper.GetTranslation("Slideshow");
            Settings = TranslationHelper.GetTranslation("Settings");
            InfoWinow = TranslationHelper.GetTranslation("InfoWindow");
            ImageInfo = TranslationHelper.GetTranslation("ImageInfo");
            About = TranslationHelper.GetTranslation("About");
            ShowAllSettingsWindow = TranslationHelper.GetTranslation("ShowAllSettingsWindow");
            StayTopMost = TranslationHelper.GetTranslation("StayTopMost");
            SearchSubdirectory = TranslationHelper.GetTranslation("SearchSubdirectory");
            ToggleLooping = TranslationHelper.GetTranslation("ToggleLooping");
            HideShowInterface = TranslationHelper.GetTranslation("HideShowInterface");
            ApplicationShortcuts = TranslationHelper.GetTranslation("ApplicationShortcuts");
            BatchResize = TranslationHelper.GetTranslation("BatchResize");
            Effects = TranslationHelper.GetTranslation("Effects");
            EffectsTooltip = TranslationHelper.GetTranslation("EffectsTooltip");
            FileProperties = TranslationHelper.GetTranslation("FileProperties");
            OptimizeImage = TranslationHelper.GetTranslation("OptimizeImage");
            ImageInfo = TranslationHelper.GetTranslation("ImageInfo");
            FileName = TranslationHelper.GetTranslation("FileName");
            FileSize = TranslationHelper.GetTranslation("FileSize");
            Folder = TranslationHelper.GetTranslation("Folder");
            FullPath = TranslationHelper.GetTranslation("FullPath");
            Created = TranslationHelper.GetTranslation("Created");
            Modified = TranslationHelper.GetTranslation("Modified");
            LastAccessTime = TranslationHelper.GetTranslation("LastAccessTime");
            ConvertTo = TranslationHelper.GetTranslation("ConvertTo");
            NoConversion = TranslationHelper.GetTranslation("NoConversion");
            Resize = TranslationHelper.GetTranslation("Resize");
            NoResize = TranslationHelper.GetTranslation("NoResize");
            Apply = TranslationHelper.GetTranslation("Apply");
            Cancel = TranslationHelper.GetTranslation("Cancel");
            BitDepth = TranslationHelper.GetTranslation("BitDepth");
            AspectRatio = TranslationHelper.GetTranslation("AspectRatio");
        }

        #region Commands

        public ICommand? ExitCommand { get; }
        public ICommand? MinimizeCommand { get; }
        public ICommand? MaximizeCommand { get; }
        public ICommand? NextCommand { get; private set; }
        public ICommand? PreviousCommand { get; private set; }
        public ICommand? FirstCommand { get; private set; }
        public ICommand? LastCommand { get; private set; }
        public ICommand? OpenFileCommand { get; private set; }
        public ICommand? SaveFileCommand { get; private set; }
        public ICommand? OpenLastFileCommand { get; }
        public ICommand? PasteCommand { get; private set; }
        public ICommand? CopyFileCommand { get; private set; }
        public ICommand? CopyFilePathCommand { get; private set; }
        public ICommand? ReloadCommand { get; private set; }
        public ICommand? PrintCommand { get; private set; }
        public ICommand? DeleteFileCommand { get; private set; }
        public ICommand? RecycleFileCommand { get; private set; }
        public ICommand? CloseMenuCommand { get; }
        public ICommand? ToggleFileMenuCommand { get; }
        public ICommand? ToggleImageMenuCommand { get; }
        public ICommand? ToggleSettingsMenuCommand { get; }
        public ICommand? ToggleToolsMenuCommand { get; }

        private ICommand? _showInFolderCommand;

        public ICommand? ShowInFolderCommand
        {
            get => _showInFolderCommand;
            set => this.RaiseAndSetIfChanged(ref _showInFolderCommand, value);
        }

        public ICommand? OpenWithCommand { get; }
        public ICommand? RenameCommand { get; }
        public ICommand? NewWindowCommand { get; }
        public ICommand? DuplicateFileCommand { get; }

        public ICommand? RotateLeftCommand { get; }
        public ICommand? RotateRightCommand { get; }
        public ICommand? FlipCommand { get; }
        public ICommand? ChangeAutoFitCommand { get; }

        public ICommand? ChangeStretchCommand { get; }

        public ICommand? ChangeTopMostCommand { get; }

        public ICommand? ChangeIncludeSubdirectoriesCommand { get; }

        public ICommand? ToggleUICommand { get; }
        public ICommand? ToggleIsRotationTransformOpenCommand { get; }

        private ICommand? _showExifWindowCommand;

        public ICommand? ShowExifWindowCommand
        {
            get => _showExifWindowCommand;
            set => this.RaiseAndSetIfChanged(ref _showExifWindowCommand, value);
        }

        #endregion Commands

        #region Fields

        private string? _getFlipped;

        public string? GetFlipped
        {
            get => _getFlipped;
            set => this.RaiseAndSetIfChanged(ref _getFlipped, value);
        }

        public string? GetBottomGallery => IsBottomGalleryShown ? HideBottomGallery : ShowBottomGallery;

        private int _scaleX = 1;

        public int ScaleX
        {
            get => _scaleX;
            set => this.RaiseAndSetIfChanged(ref _scaleX, value);
        }

        private UserControl? _currentView;

        public UserControl? CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        private IImage? _image;

        public IImage? Image
        {
            get => _image;
            set => this.RaiseAndSetIfChanged(ref _image, value);
        }

        private int _getIndex;

        public int GetIndex
        {
            get => _getIndex;
            set => this.RaiseAndSetIfChanged(ref _getIndex, value);
        }

        #region Window Properties

        private string? _title = "Loading...";

        public string? Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private string? _titleTooltip = "Loading...";

        public string? TitleTooltip
        {
            get => _titleTooltip;
            set => this.RaiseAndSetIfChanged(ref _titleTooltip, value);
        }

        private string? _windowTitle = "PicView";

        public string? WindowTitle
        {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }

        private SizeToContent _sizeToContent;

        public SizeToContent SizeToContent
        {
            get => _sizeToContent;
            set => this.RaiseAndSetIfChanged(ref _sizeToContent, value);
        }

        private bool _canResize;

        public bool CanResize
        {
            get => _canResize;
            set => this.RaiseAndSetIfChanged(ref _canResize, value);
        }

        private bool _isRotationAnimationEnabled;

        public bool IsRotationAnimationEnabled
        {
            get => _isRotationAnimationEnabled;
            set => this.RaiseAndSetIfChanged(ref _isRotationAnimationEnabled, value);
        }

        #endregion Window Properties

        #region Size

        private double _width;

        public double Width
        {
            get => _width;
            set => this.RaiseAndSetIfChanged(ref _width, value);
        }

        private double _height;

        public double Height
        {
            get => _height;
            set => this.RaiseAndSetIfChanged(ref _height, value);
        }

        private double _titleMaxWidth;

        public double TitleMaxWidth
        {
            get => _titleMaxWidth;
            set => this.RaiseAndSetIfChanged(ref _titleMaxWidth, value);
        }

        #endregion Size

        #region Zoom

        private double _rotationAngle;

        public double RotationAngle
        {
            get => _rotationAngle;
            set => this.RaiseAndSetIfChanged(ref _rotationAngle, value);
        }

        private double _zoomValue;

        public double ZoomValue
        {
            get => _zoomValue;
            set => this.RaiseAndSetIfChanged(ref _zoomValue, value);
        }

        private ScrollBarVisibility _toggleScrollBarVisibility;

        public ScrollBarVisibility ToggleScrollBarVisibility
        {
            get => _toggleScrollBarVisibility;
            set => this.RaiseAndSetIfChanged(ref _toggleScrollBarVisibility, value);
        }

        #endregion Zoom

        #region Settings

        private bool _isBottomGalleryShown = SettingsHelper.Settings.Gallery.IsBottomGalleryShown;

        public bool IsBottomGalleryShown
        {
            get => _isBottomGalleryShown;
            set => this.RaiseAndSetIfChanged(ref _isBottomGalleryShown, value);
        }

        private bool _showBottomGalleryInHiddenUI = SettingsHelper.Settings.Gallery.ShowBottomGalleryInHiddenUI;

        public bool ShowBottomGalleryInHiddenUI
        {
            get => _showBottomGalleryInHiddenUI;
            set => this.RaiseAndSetIfChanged(ref _showBottomGalleryInHiddenUI, value);
        }

        private bool _isTopMost = SettingsHelper.Settings.WindowProperties.TopMost;

        public bool IsTopMost
        {
            get => _isTopMost;
            set => this.RaiseAndSetIfChanged(ref _isTopMost, value);
        }

        private bool _includeSubdirectories = SettingsHelper.Settings.Sorting.IncludeSubDirectories;

        public bool IncludeSubdirectories
        {
            get => _includeSubdirectories;
            set
            {
                this.RaiseAndSetIfChanged(ref _includeSubdirectories, value);
                SettingsHelper.Settings.Sorting.IncludeSubDirectories = value;
                _ = SettingsHelper.SaveSettingsAsync();
            }
        }

        private bool _isScrollingEnabled = SettingsHelper.Settings.Zoom.ScrollEnabled;

        public bool IsScrollingEnabled
        {
            get => _isScrollingEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _isScrollingEnabled, value);
                ToggleScrollBarVisibility = value ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;
                SettingsHelper.Settings.Zoom.ScrollEnabled = value;
                SetSize();
                _ = SettingsHelper.SaveSettingsAsync();
            }
        }

        private bool _isStretched = SettingsHelper.Settings.ImageScaling.StretchImage;

        public bool IsStretched
        {
            get => _isStretched;
            set
            {
                this.RaiseAndSetIfChanged(ref _isStretched, value);
                SettingsHelper.Settings.ImageScaling.StretchImage = value;
                SetSize();
                _ = SettingsHelper.SaveSettingsAsync();
            }
        }

        private bool _isLooping = SettingsHelper.Settings.UIProperties.Looping;

        public bool IsLooping
        {
            get => _isLooping;
            set
            {
                this.RaiseAndSetIfChanged(ref _isLooping, value);
                SettingsHelper.Settings.UIProperties.Looping = value;
                _ = SettingsHelper.SaveSettingsAsync();
            }
        }

        private bool _isAutoFit = SettingsHelper.Settings.WindowProperties.AutoFit;

        public bool IsAutoFit
        {
            get => _isAutoFit;
            set => this.RaiseAndSetIfChanged(ref _isAutoFit, value);
        }

        private bool _isInterfaceShown = SettingsHelper.Settings.UIProperties.ShowInterface;

        public bool IsInterfaceShown
        {
            get => _isInterfaceShown;
            set => this.RaiseAndSetIfChanged(ref _isInterfaceShown, value);
        }

        #endregion Settings

        #region Menus

        private bool _isFileMenuVisible;

        public bool IsFileMenuVisible
        {
            get => _isFileMenuVisible;
            set => this.RaiseAndSetIfChanged(ref _isFileMenuVisible, value);
        }

        private bool _isImageMenuVisible;

        public bool IsImageMenuVisible
        {
            get => _isImageMenuVisible;
            set => this.RaiseAndSetIfChanged(ref _isImageMenuVisible, value);
        }

        private bool _isSettingsMenuVisible;

        public bool IsSettingsMenuVisible
        {
            get => _isSettingsMenuVisible;
            set => this.RaiseAndSetIfChanged(ref _isSettingsMenuVisible, value);
        }

        private bool _isToolsMenuVisible;

        public bool IsToolsMenuVisible
        {
            get => _isToolsMenuVisible;
            set => this.RaiseAndSetIfChanged(ref _isToolsMenuVisible, value);
        }

        #endregion Menus

        #endregion Fields

        #region Services

        public FileService? FileService;

        public ImageIterator? ImageIterator;

        public ImageService? ImageService;

        #endregion Services

        #region Methods

        public void SetSize()
        {
            if (Image is null)
            {
                return;
            }
            var preloadValue = ImageIterator?.PreLoader.Get(ImageIterator.Index, ImageIterator.Pics);
            SetSize(preloadValue?.ImageModel?.PixelWidth ?? (int)Width, preloadValue?.ImageModel?.PixelHeight ?? (int)Height, RotationAngle);
        }

        public void SetSize(double width, double height, double rotation)
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                return;
            }
            var monitor = ScreenHelper.GetScreen(desktop.MainWindow);
            double desktopMinWidth = 0, desktopMinHeight = 0, containerWidth = 0, containerHeight = 0;
            var uiTopSize = SettingsHelper.Settings.UIProperties.ShowInterface ? 32 : 0; // Height of the titlebar, TODO get actual size
            var uiBottomSize =
                SettingsHelper.Settings.UIProperties.ShowInterface || SettingsHelper.Settings.UIProperties.ShowBottomNavBar
                    ? 26 : 0;
            var galleryHeight = IsBottomGalleryShown ? 100 : 0;
            if (Dispatcher.UIThread.CheckAccess())
            {
                desktopMinWidth = desktop.MainWindow.MinWidth;
                desktopMinHeight = desktop.MainWindow.MinHeight;
                containerWidth = desktop.MainWindow.Width;
                containerHeight = desktop.MainWindow.Height - (uiTopSize + uiBottomSize);
            }
            else
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    desktopMinWidth = desktop.MainWindow.MinWidth;
                    desktopMinHeight = desktop.MainWindow.MinHeight;
                    containerWidth = desktop.MainWindow.Width;
                    containerHeight = desktop.MainWindow.Height - (uiTopSize + uiBottomSize);
                }, DispatcherPriority.Normal).Wait();
            }
            var size = ImageSizeCalculationHelper.GetImageSize(
                width,
                height,
                monitor.Bounds.Width,
                monitor.Bounds.Height,
                desktopMinWidth,
                desktopMinHeight,
                ImageSizeCalculationHelper.GetInterfaceSize(),
                rotation,
                IsStretched,
                75,
                monitor.Scaling,
                SettingsHelper.Settings.WindowProperties.Fullscreen,
                uiTopSize,
                uiBottomSize,
                galleryHeight,
                IsAutoFit,
                containerWidth,
                containerHeight,
                IsScrollingEnabled);

            TitleMaxWidth = size.TitleMaxWidth;
            Width = size.Width;
            Height = size.Height;
        }

        public void SetImageModel(ImageModel imageModel)
        {
            Image = imageModel?.Image ?? null; // TODO replace with broken image graphic if it is null
            FileInfo = imageModel?.FileInfo ?? null;
            if (imageModel?.EXIFOrientation.HasValue ?? false)
            {
                switch (imageModel.EXIFOrientation.Value)
                {
                    default:
                        ScaleX = 1;
                        RotationAngle = 0;
                        break;

                    case EXIFHelper.EXIFOrientation.Flipped:
                        ScaleX = -1;
                        RotationAngle = 0;
                        break;

                    case EXIFHelper.EXIFOrientation.Rotated180:
                        RotationAngle = 180;
                        ScaleX = 1;
                        break;

                    case EXIFHelper.EXIFOrientation.Rotated180Flipped:
                        RotationAngle = 180;
                        ScaleX = -1;
                        break;

                    case EXIFHelper.EXIFOrientation.Rotated270Flipped:
                        RotationAngle = 270;
                        ScaleX = -1;
                        break;

                    case EXIFHelper.EXIFOrientation.Rotated90:
                        RotationAngle = 90;
                        ScaleX = 1;
                        break;

                    case EXIFHelper.EXIFOrientation.Rotated90Flipped:
                        RotationAngle = 90;
                        ScaleX = -1;
                        break;

                    case EXIFHelper.EXIFOrientation.Rotated270:
                        RotationAngle = 270;
                        ScaleX = 1;
                        break;
                }
            }
            else
            {
                ScaleX = 1;
                RotationAngle = 0;
            }
            ZoomValue = 1;
        }

        public void SetTitle(ImageModel? imageModel, ImageIterator imageIterator)
        {
            if (imageModel is null || ImageIterator is null)
            {
                ReturnError();
                return;
            }

            if (imageModel.FileInfo is null)
            {
                ReturnError();
                return;
            }

            var titleString = TitleHelper.GetTitle(imageModel.PixelWidth, imageModel.PixelHeight, imageIterator.Index,
                imageModel.FileInfo, ZoomValue, imageIterator.Pics);
            WindowTitle = titleString[0];
            Title = titleString[1];
            TitleTooltip = titleString[2];

            return;

            void ReturnError()
            {
                WindowTitle =
                Title =
                TitleTooltip = TranslationHelper.GetTranslation("UnableToRender");
            }
        }

        public void SetTitle()
        {
            if (ImageIterator is null)
            {
                WindowTitle =
                    Title =
                        TitleTooltip = TranslationHelper.GetTranslation("UnableToRender");
                return;
            }

            var titleString = TitleHelper.GetTitle((int)Width, (int)Height, ImageIterator.Index,
                    FileInfo, ZoomValue, ImageIterator.Pics);
            WindowTitle = titleString[0];
            Title = titleString[1];
            TitleTooltip = titleString[2];
        }

        public void ResetTitle()
        {
            WindowTitle = TranslationHelper.GetTranslation("NoImage") + " - PicView";
            TitleTooltip = Title = TranslationHelper.GetTranslation("NoImage");
        }

        public void SetLoadingTitle()
        {
            WindowTitle = TranslationHelper.GetTranslation("Loading") + " - PicView";
            TitleTooltip = Title = TranslationHelper.GetTranslation("Loading");
        }

        public async Task LoadNextPic(NavigateTo navigateTo)
        {
            if (ImageIterator is null)
            {
                return;
            }
            var index = ImageIterator.GetIteration(ImageIterator.Index, navigateTo);
            if (index < 0)
            {
                return;
            }
            await LoadPicAtIndex(index);
        }

        public async Task LoadPicAtIndex(int index) => await Task.Run(async () =>
        {
            if (ImageIterator is null)
            {
                return;
            }

            try
            {
                ImageIterator.Index = index;
                var x = 0;

                var preLoadValue = ImageIterator.PreLoader.Get(index, ImageIterator.Pics);
                if (preLoadValue is not null)
                {
                    while (preLoadValue.IsLoading)
                    {
                        if (x == 0)
                        {
                            SetLoadingTitle();
                            using var image = new MagickImage();
                            image.Ping(ImageIterator.Pics[index]);
                            var thumb = image.GetExifProfile()?.CreateThumbnail();
                            if (thumb is not null)
                            {
                                var stream = new MemoryStream(thumb?.ToByteArray());
                                Image = new Bitmap(stream);
                            }
                            else
                            {
                                Image = null;
                            }
                        }

                        x++;
                        await Task.Delay(20);
                        if (ImageIterator.Index != index)
                        {
                            await ImageIterator.Preload(ImageService);
                            return;
                        }

                        if (x <= 50)
                        {
                            continue;
                        }

                        await GetPreload();
#if DEBUG
                        Trace.WriteLine("Loading timeout");
#endif
                        break;
                    }
                }

                if (preLoadValue is null)
                {
                    await GetPreload();
                }

                if (ImageIterator.Index != index)
                {
                    await ImageIterator.Preload(ImageService);
                    return;
                }

                SetImageModel(preLoadValue.ImageModel);
                SetSize(preLoadValue.ImageModel.PixelWidth, preLoadValue.ImageModel.PixelHeight, 0);
                SetTitle(preLoadValue.ImageModel, ImageIterator);
                GetIndex = ImageIterator.Index + 1;
                ImageChanged?.Invoke(this, EventArgs.Empty);
                await ImageIterator.AddAsync(ImageIterator.Index, ImageService, preLoadValue?.ImageModel);
                await ImageIterator.Preload(ImageService);
                return;

                async Task GetPreload()
                {
                    await ImageIterator.PreLoader.AddAsync(index, ImageService, ImageIterator.Pics)
                        .ConfigureAwait(false);
                    preLoadValue = ImageIterator.PreLoader.Get(index, ImageIterator.Pics);
                    if (ImageIterator.Index != index)
                    {
                        await ImageIterator.Preload(ImageService);
                        return;
                    }

                    if (preLoadValue is null)
                    {
                        throw new ArgumentNullException(nameof(LoadNextPic),
                            nameof(preLoadValue) + " is null");
                    }
                }
            }
            catch (Exception)
            {
                // TODO display exception to user
            }
        }).ConfigureAwait(false);

        public async Task LoadPicFromString(string path)
        {
            if (!File.Exists(path))
            {
                // TODO load from URL if not a file
                throw new FileNotFoundException(path);
            }

            await LoadPicFromFile(new FileInfo(path)).ConfigureAwait(false);
        }

        public async Task LoadPicFromFile(FileInfo fileInfo)
        {
            SetLoadingTitle();

            await Task.Run(async () =>
            {
                try
                {
                    ArgumentNullException.ThrowIfNull(fileInfo);

                    var imageModel = new ImageModel
                    {
                        FileInfo = fileInfo
                    };

                    ImageService ??= new ImageService();
                    await ImageService.LoadImageAsync(imageModel);
                    SetImageModel(imageModel);
                    SetSize(imageModel.PixelWidth, imageModel.PixelHeight, imageModel.Rotation);

                    ImageIterator = new ImageIterator(imageModel.FileInfo);
                    ImageIterator.Index = ImageIterator.Pics.IndexOf(fileInfo.FullName);
                    await LoadPicAtIndex(ImageIterator.Index);
                    ImageIterator.FileAdded += (_, e) => { SetTitle(); };
                    ImageIterator.FileRenamed += (_, e) => { SetTitle(); };
                    ImageIterator.FileDeleted += async (_, isSameFile) =>
                    {
                        if (isSameFile) //change if deleting current file
                        {
                            if (ImageIterator?.Index < 0 || ImageIterator?.Index >= ImageIterator?.Pics.Count)
                            {
                                return;
                            }
                            await LoadPicFromString(ImageIterator.Pics[ImageIterator.Index]);
                        }
                        else
                        {
                            SetTitle();
                        }
                    };
                }
                catch (Exception)
                {
                    if (ImageIterator is null)
                    {
                        await Dispatcher.UIThread.InvokeAsync(() => { CurrentView = new StartUpMenu(); });
                    }
                }
            });
        }

        #endregion Methods

        public MainViewModel()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                return;
            }

            SetLoadingTitle();

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                CurrentView = new ImageViewer();
                Task.Run(async () => { await LoadPicFromString(args[1]); });
            }
            else if (SettingsHelper.Settings.StartUp.OpenLastFile)
            {
                CurrentView = new ImageViewer();
                Task.Run(async () => { await LoadPicFromString(SettingsHelper.Settings.StartUp.LastFile); });
            }
            else
            {
                CurrentView = new StartUpMenu();
            }

            if (SettingsHelper.Settings.WindowProperties.AutoFit)
            {
                desktop.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                SizeToContent = SizeToContent.WidthAndHeight;
                CanResize = false;
                IsAutoFit = true;
            }
            else
            {
                CanResize = true;
                IsAutoFit = false;
                WindowHelper.InitializeWindowSizeAndPosition(desktop);
            }

            Task.Run(UpdateLanguage);

            #region Window commands

            ExitCommand = ReactiveCommand.Create(desktop.MainWindow.Close);
            MinimizeCommand = ReactiveCommand.Create(() =>
                desktop.MainWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = ReactiveCommand.Create(() =>
            {
                desktop.MainWindow.WindowState = desktop.MainWindow.WindowState == WindowState.Normal
                    ? WindowState.Maximized
                    : WindowState.Normal;
            });

            NewWindowCommand = ReactiveCommand.Create(ProcessHelper.StartNewProcess);

            ChangeAutoFitCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SettingsHelper.Settings.WindowProperties.AutoFit)
                {
                    SizeToContent = SizeToContent.Manual;
                    CanResize = true;
                    SettingsHelper.Settings.WindowProperties.AutoFit = false;
                    IsAutoFit = false;
                }
                else
                {
                    SizeToContent = SizeToContent.WidthAndHeight;
                    CanResize = false;
                    SettingsHelper.Settings.WindowProperties.AutoFit = true;
                    IsAutoFit = true;
                }
                SetSize();
                await SettingsHelper.SaveSettingsAsync().ConfigureAwait(false);
            });

            ChangeTopMostCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SettingsHelper.Settings.WindowProperties.TopMost)
                {
                    IsTopMost = false;
                    desktop.MainWindow.Topmost = false;
                    SettingsHelper.Settings.WindowProperties.TopMost = false;
                }
                else
                {
                    IsTopMost = true;
                    desktop.MainWindow.Topmost = true;
                    SettingsHelper.Settings.WindowProperties.TopMost = true;
                }

                await SettingsHelper.SaveSettingsAsync().ConfigureAwait(false);
            });
            if (SettingsHelper.Settings.WindowProperties.TopMost)
            {
                desktop.MainWindow.Topmost = true;
            }

            ChangeIncludeSubdirectoriesCommand = ReactiveCommand.Create(() =>
            {
                IncludeSubdirectories = !IncludeSubdirectories;
                SetTitle();
            });

            ToggleUICommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SettingsHelper.Settings.UIProperties.ShowInterface)
                {
                    IsInterfaceShown = false;
                    SettingsHelper.Settings.UIProperties.ShowInterface = false;
                }
                else
                {
                    IsInterfaceShown = true;
                    SettingsHelper.Settings.UIProperties.ShowInterface = true;
                }
                CloseMenuCommand.Execute(null);
                await SettingsHelper.SaveSettingsAsync().ConfigureAwait(false);
            });

            ShowExifWindowCommand = ReactiveCommand.Create(() =>
            {
            });

            #endregion Window commands

            #region Navigation Commands

            NextCommand = ReactiveCommand.Create(async () =>
            {
                if (ImageIterator is null)
                {
                    return;
                }

                await LoadNextPic(NavigateTo.Next).ConfigureAwait(false);
            });

            PreviousCommand = ReactiveCommand.Create(async () =>
            {
                if (ImageIterator is null)
                {
                    return;
                }

                await LoadNextPic(NavigateTo.Previous).ConfigureAwait(false);
            });

            FirstCommand = ReactiveCommand.Create(async () =>
            {
                if (ImageIterator is null)
                {
                    return;
                }

                await LoadNextPic(NavigateTo.First).ConfigureAwait(false);
            });

            LastCommand = ReactiveCommand.Create(async () =>
            {
                if (ImageIterator is null)
                {
                    return;
                }

                await LoadNextPic(NavigateTo.Last).ConfigureAwait(false);
            });

            ReloadCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (FileInfo is null || ImageIterator is null)
                {
                    return;
                }

                ImageIterator.PreLoader.Clear();
                CurrentView = new ImageViewer();
                Image = null;
                await LoadPicFromString(FileInfo.FullName);
            });

            #endregion Navigation Commands

            #region Menus

            CloseMenuCommand = ReactiveCommand.Create(() =>
            {
                IsFileMenuVisible = false;
                IsImageMenuVisible = false;
                IsSettingsMenuVisible = false;
                IsToolsMenuVisible = false;
            });

            ToggleFileMenuCommand = ReactiveCommand.Create(() =>
            {
                IsFileMenuVisible = !IsFileMenuVisible;
                IsImageMenuVisible = false;
                IsSettingsMenuVisible = false;
                IsToolsMenuVisible = false;
            });

            ToggleImageMenuCommand = ReactiveCommand.Create(() =>
            {
                IsFileMenuVisible = false;
                IsImageMenuVisible = !IsImageMenuVisible;
                IsSettingsMenuVisible = false;
                IsToolsMenuVisible = false;
            });

            ToggleSettingsMenuCommand = ReactiveCommand.Create(() =>
            {
                IsFileMenuVisible = false;
                IsImageMenuVisible = false;
                IsSettingsMenuVisible = !IsSettingsMenuVisible;
                IsToolsMenuVisible = false;
            });

            ToggleToolsMenuCommand = ReactiveCommand.Create(() =>
            {
                IsFileMenuVisible = false;
                IsImageMenuVisible = false;
                IsSettingsMenuVisible = false;
                IsToolsMenuVisible = !IsToolsMenuVisible;
            });

            #endregion Menus

            #region Image commands

            RotateLeftCommand = ReactiveCommand.Create(() =>
            {
                if (Image is null)
                {
                    return;
                }
                if (RotationHelper.IsValidRotation(RotationAngle))
                {
                    var nextAngle = RotationHelper.Rotate(RotationAngle, true);
                    if (nextAngle == 360)
                    {
                        RotationAngle = 0;
                    }
                    else
                    {
                        RotationAngle = nextAngle;
                    }
                }
                else
                {
                    RotationAngle = RotationHelper.NextRotationAngle(RotationAngle, true);
                }
                SetSize();
            });

            RotateRightCommand = ReactiveCommand.Create(() =>
            {
                if (Image is null)
                {
                    return;
                }
                if (RotationHelper.IsValidRotation(RotationAngle))
                {
                    var nextAngle = RotationHelper.Rotate(RotationAngle, false);
                    if (nextAngle == -90)
                    {
                        RotationAngle = 270;
                    }
                    else
                    {
                        RotationAngle = nextAngle;
                    }
                }
                else
                {
                    RotationAngle = RotationHelper.NextRotationAngle(RotationAngle, false);
                }

                SetSize();
            });

            GetFlipped = Flip;
            FlipCommand = ReactiveCommand.Create(() =>
            {
                if (Image is null)
                {
                    return;
                }
                if (ScaleX == 1)
                {
                    ScaleX = -1;
                    GetFlipped = UnFlip;
                }
                else
                {
                    ScaleX = 1;
                    GetFlipped = Flip;
                }
            });

            #endregion Image commands

            #region File commands

            OpenFileCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                FileService ??= new FileService();
                var file = await FileService.OpenFile();
                if (file is null)
                {
                    return;
                }

                CurrentView = new ImageViewer();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var path = file.Path.AbsolutePath;
                    await LoadPicFromFile(new FileInfo(path));
                }
                else
                {
                    await LoadPicFromFile(new FileInfo(file.Path.LocalPath));
                }
            });

            OpenLastFileCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var lastFile = SettingsHelper.Settings.StartUp.LastFile;
                if (string.IsNullOrEmpty(lastFile))
                {
                    return;
                }

                await LoadPicFromString(lastFile);
            });

            SaveFileCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                FileService ??= new FileService();
                await FileService.SaveFileAsync(FileInfo?.FullName);
            });

            CopyFileCommand = ReactiveCommand.Create(async () =>
            {
                var clipboard = desktop.MainWindow.Clipboard;
                var dataObject = new DataObject();
                dataObject.Set(DataFormats.Files, new[] { FileInfo?.FullName });
                await clipboard.SetDataObjectAsync(dataObject);
            });

            CopyFilePathCommand = ReactiveCommand.Create(async () =>
            {
                await desktop.MainWindow.Clipboard.SetTextAsync(FileInfo?.FullName);
            });

            PasteCommand = ReactiveCommand.Create(() => { });

            OpenWithCommand = ReactiveCommand.Create(() =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    // TODO: implement open with on mac
                }
                else
                {
                    ProcessHelper.OpenWith(FileInfo?.FullName);
                }
            });

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ShowInFolderCommand = ReactiveCommand.Create(() =>
                {
                    Process.Start("open", $"-R \"{FileInfo?.FullName}\"");
                });
            }

            RenameCommand = ReactiveCommand.Create(() => { });

            DuplicateFileCommand = ReactiveCommand.Create(() =>
            {
                FileHelper.DuplicateAndReturnFileName(FileInfo?.FullName);
            });

            PrintCommand = ReactiveCommand.Create(() => { ProcessHelper.Print(FileInfo?.FullName); });

            DeleteFileCommand = ReactiveCommand.Create(() =>
            {
                FileDeletionHelper.DeleteFileWithErrorMsg(FileInfo?.FullName, false);
            });

            RecycleFileCommand = ReactiveCommand.Create(() =>
            {
                FileDeletionHelper.DeleteFileWithErrorMsg(FileInfo?.FullName, true);
            });

            #endregion File commands

            Activator = new ViewModelActivator();
            this.WhenActivated(disposables =>
            {
                /* handle activation */
                Disposable
                    .Create(() =>
                    {
                        /* handle deactivation */
                    })
                    .DisposeWith(disposables);
            });
        }
    }
}