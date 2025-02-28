﻿using PicView.Avalonia.ViewModels;
using PicView.Core.Config;
using PicView.Core.Localization;

namespace PicView.Avalonia.SettingsManagement;

public static class LanguageUpdater
{
    public static async Task UpdateLanguageAsync(MainViewModel vm, bool settingsExists)
    {
        if (settingsExists)
        {
            await TranslationHelper.LoadLanguage(SettingsHelper.Settings.UIProperties.UserLanguage).ConfigureAwait(false);
        }
        else
        {
            await TranslationHelper.DetermineAndLoadLanguage().ConfigureAwait(false);
        }

        vm.UpdateLanguage();

        vm.GetIsFlippedTranslation = vm.ScaleX == 1 ? vm.Flip : vm.UnFlip;
        
        vm.GetIsShowingUITranslation = !SettingsHelper.Settings.UIProperties.ShowInterface ? vm.ShowUI : vm.HideUI;
        
        vm.GetIsScrollingTranslation = SettingsHelper.Settings.Zoom.ScrollEnabled ?
            TranslationHelper.Translation.ScrollingEnabled : TranslationHelper.Translation.ScrollingDisabled;
        
        vm.GetIsShowingBottomGalleryTranslation = vm.IsGalleryShown ?
            TranslationHelper.Translation.HideBottomGallery :
            TranslationHelper.Translation.ShowBottomGallery;
        
        vm.GetIsLoopingTranslation = SettingsHelper.Settings.UIProperties.Looping
            ? TranslationHelper.Translation.LoopingEnabled
            : TranslationHelper.Translation.LoopingDisabled;
        
        vm.GetIsCtrlZoomTranslation = SettingsHelper.Settings.Zoom.CtrlZoom
            ? TranslationHelper.Translation.CtrlToZoom
            : TranslationHelper.Translation.ScrollToZoom;
        
        vm.GetIsShowingBottomToolbarTranslation = SettingsHelper.Settings.UIProperties.ShowBottomNavBar
            ? TranslationHelper.Translation.HideBottomToolbar
            : TranslationHelper.Translation.ShowBottomToolbar;
        
        vm.GetIsShowingFadingUIButtonsTranslation = SettingsHelper.Settings.UIProperties.ShowAltInterfaceButtons
            ? TranslationHelper.Translation.DisableFadeInButtonsOnHover
            : TranslationHelper.Translation.ShowFadeInButtonsOnHover;
        
        vm.GetIsUsingTouchpadTranslation = SettingsHelper.Settings.Zoom.IsUsingTouchPad
            ? TranslationHelper.Translation.UsingTouchpad
            : TranslationHelper.Translation.UsingMouse;
    }
}
