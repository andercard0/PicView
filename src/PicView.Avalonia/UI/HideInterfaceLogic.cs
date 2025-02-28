﻿using Avalonia.Controls;
using Avalonia.Threading;
using PicView.Avalonia.Animations;
using PicView.Avalonia.Gallery;
using PicView.Avalonia.Navigation;
using PicView.Avalonia.ViewModels;
using PicView.Avalonia.WindowBehavior;
using PicView.Core.Calculations;
using PicView.Core.Config;
using PicView.Core.Gallery;
using PicView.Core.Localization;

namespace PicView.Avalonia.UI;

public static class HideInterfaceLogic
{
    #region Toggle UI
    /// <summary>
    /// Toggle between showing the full interface and hiding it
    /// </summary>
    /// <param name="vm">The view model. </param>
    public static async Task ToggleUI(MainViewModel vm)
    {
        if (SettingsHelper.Settings.UIProperties.ShowInterface)
        {
            vm.IsUIShown = false;
            SettingsHelper.Settings.UIProperties.ShowInterface = false;
            vm.IsTopToolbarShown = false;
            vm.IsBottomToolbarShown = false;
            vm.GetIsShowingUITranslation = TranslationHelper.Translation.ShowUI;
            if (!GalleryFunctions.IsFullGalleryOpen)
            {
                if (!SettingsHelper.Settings.Gallery.ShowBottomGalleryInHiddenUI)
                {
                    vm.GalleryMode = GalleryMode.Closed;
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        if (UIHelper.GetGalleryView.Bounds.Height > 0)
                        {
                            vm.GalleryMode = GalleryMode.BottomToClosed;
                        }
                    });
                    vm.IsGalleryShown = false;
                }
                else
                {
                    vm.IsGalleryShown = SettingsHelper.Settings.Gallery.ShowBottomGalleryInHiddenUI;
                }
            }
        }
        else
        {
            vm.IsUIShown = true;
            vm.IsTopToolbarShown = true;
            vm.GetIsShowingUITranslation = TranslationHelper.Translation.HideUI;
            if (SettingsHelper.Settings.UIProperties.ShowBottomNavBar)
            {
                vm.IsBottomToolbarShown = true;
                vm.BottombarHeight = SizeDefaults.BottombarHeight;
            }
            SettingsHelper.Settings.UIProperties.ShowInterface = true;
            vm.TitlebarHeight = SizeDefaults.TitlebarHeight;
            if (!GalleryFunctions.IsFullGalleryOpen)
            {
                if (SettingsHelper.Settings.Gallery.IsBottomGalleryShown)
                {
                    if (NavigationHelper.CanNavigate(vm))
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            if (UIHelper.GetGalleryView.Bounds.Height <= 0)
                            {
                                vm.GalleryMode = GalleryMode.Closed;
                                GalleryFunctions.OpenBottomGallery(vm);
                            }
                        });
                        _ = GalleryLoad.LoadGallery(vm, vm.FileInfo.DirectoryName);
                    }

                    vm.IsGalleryShown = true;
                }
                else
                {
                    vm.IsGalleryShown = false;
                }
            }
        }
        
        WindowResizing.SetSize(vm);
        UIHelper.CloseMenus(vm);
        await SettingsHelper.SaveSettingsAsync();
    }
    
    /// <summary>
    /// Toggle between showing the bottom toolbar and hiding it
    /// </summary>
    /// <param name="vm">The view model. </param>
    public static async Task ToggleBottomToolbar(MainViewModel vm)
    {
        if (SettingsHelper.Settings.UIProperties.ShowBottomNavBar)
        {
            vm.IsBottomToolbarShown = false;
            SettingsHelper.Settings.UIProperties.ShowBottomNavBar = false;
            vm.IsBottomToolbarShownSetting = false;
            vm.GetIsShowingBottomToolbarTranslation = TranslationHelper.Translation.ShowBottomToolbar;
        }
        else
        {
            vm.IsBottomToolbarShown = true;
            SettingsHelper.Settings.UIProperties.ShowBottomNavBar = true;
            vm.IsBottomToolbarShownSetting = true;
            vm.BottombarHeight = SizeDefaults.BottombarHeight;
            vm.GetIsShowingBottomToolbarTranslation = TranslationHelper.Translation.HideBottomToolbar;
        }
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            WindowResizing.SetSize(vm);
        });
        
        await SettingsHelper.SaveSettingsAsync();
    }
    
    #endregion

    #region HoverButtons
    
    public static void AddHoverButtonEvents(Control parent, MainViewModel vm)
    {
        parent.PointerEntered += async delegate
        {
            await DoHoverButtonAnimation(isShown:true, parent, vm);
        };
        parent.PointerExited += async delegate
        {
            await DoHoverButtonAnimation(isShown: false, parent, vm);
        };
    }
    
    public static void AddHoverButtonEvents(Control parent, Control childControl, MainViewModel vm)
    {
        childControl.PointerEntered += delegate
        {
            if (!SettingsHelper.Settings.UIProperties.ShowAltInterfaceButtons)
            {
                return;
            }
            
            if (vm.ImageIterator is null)
            {
                parent.Opacity = 0;
                childControl.Opacity = 0;
                return;
            }

            if (vm.ImageIterator.ImagePaths?.Count <= 1)
            {
                parent.Opacity = 0;
                childControl.Opacity = 0;
                return;
            }

            if (childControl.IsPointerOver)
            {
                parent.Opacity = 1;
                childControl.Opacity = 1;
            }
        };
        parent.PointerEntered += async delegate
        {
            if (!SettingsHelper.Settings.UIProperties.ShowAltInterfaceButtons)
            {
                return;
            }
            
            await DoHoverButtonAnimation(isShown:true, parent, childControl, vm);
        };
        parent.PointerExited += async delegate
        {
            await DoHoverButtonAnimation(isShown: false, parent, childControl, vm);
        };
        UIHelper.GetMainView.PointerExited += async delegate
        {
            var x = 0;
            while (_isHoverButtonAnimationRunning)
            {
                await Task.Delay(10);
                x++;
                if (x > 20)
                {
                    if (!childControl.IsPointerOver)
                    {
                        parent.Opacity = 0;
                        childControl.Opacity = 0;
                    }
                    break;
                }
            }

            if (parent.Opacity > 0)
            {
                await DoHoverButtonAnimation(isShown: false, parent, childControl, vm);
            }
        };
    }
    
    private static bool _isHoverButtonAnimationRunning;
    
    private static async Task DoHoverButtonAnimation(bool isShown, Control parent, MainViewModel vm)
    {
        if (_isHoverButtonAnimationRunning || !SettingsHelper.Settings.UIProperties.ShowAltInterfaceButtons)
        {
            return;
        }

        if (vm.ImageIterator is null)
        {
            parent.Opacity = 0;
            return;
        }

        if (vm.ImageIterator.ImagePaths?.Count <= 1)
        {
            parent.Opacity = 0;
            return;
        }
        _isHoverButtonAnimationRunning = true;
        var from = isShown ? 0d : 1d;
        var to = isShown ? 1d : 0d;
        var speed = isShown ? 0.3 : 0.45;
        var anim = AnimationsHelper.OpacityAnimation(from, to, speed);
        await anim.RunAsync(parent);
        _isHoverButtonAnimationRunning = false;
    }
    private static async Task DoHoverButtonAnimation(bool isShown, Control parent, Control childControl, MainViewModel vm)
    {
        if (_isHoverButtonAnimationRunning || !SettingsHelper.Settings.UIProperties.ShowAltInterfaceButtons)
        {
            return;
        }

        if (vm.ImageIterator is null)
        {
            parent.Opacity = 0;
            childControl.Opacity = 0;
            return;
        }

        if (vm.ImageIterator.ImagePaths?.Count <= 1)
        {
            parent.Opacity = 0;
            childControl.Opacity = 0;
            return;
        }
        _isHoverButtonAnimationRunning = true;
        var from = isShown ? 0d : 1d;
        var to = isShown ? 1d : 0d;
        var speed = isShown ? 0.3 : 0.45;
        var anim = AnimationsHelper.OpacityAnimation(from, to, speed);
        await anim.RunAsync(childControl);
        _isHoverButtonAnimationRunning = false;
    }
    
    #endregion

    public static async Task ToggleBottomGalleryShownInHiddenUI(MainViewModel vm)
    {
        SettingsHelper.Settings.Gallery.ShowBottomGalleryInHiddenUI = !SettingsHelper.Settings.Gallery
            .ShowBottomGalleryInHiddenUI;
        vm.IsBottomGalleryShownInHiddenUI = SettingsHelper.Settings.Gallery.ShowBottomGalleryInHiddenUI;

        if (!GalleryFunctions.IsFullGalleryOpen)
        {
            if (!SettingsHelper.Settings.UIProperties.ShowInterface && !SettingsHelper.Settings.Gallery
                    .ShowBottomGalleryInHiddenUI)
            {
                vm.IsGalleryShown = false;
            }
            else
            {
                vm.IsGalleryShown = SettingsHelper.Settings.Gallery.IsBottomGalleryShown;
            }
        }
        
        await SettingsHelper.SaveSettingsAsync();
    }

    public static async Task ToggleFadeInButtonsOnHover(MainViewModel vm)
    {
        SettingsHelper.Settings.UIProperties.ShowAltInterfaceButtons = !SettingsHelper.Settings
            .UIProperties.ShowAltInterfaceButtons;
        
        vm.GetIsShowingFadingUIButtonsTranslation = SettingsHelper.Settings.UIProperties.ShowAltInterfaceButtons
            ? TranslationHelper.Translation.DisableFadeInButtonsOnHover
            : TranslationHelper.Translation.ShowFadeInButtonsOnHover;
        
        await SettingsHelper.SaveSettingsAsync();
    }
}
