﻿namespace PicView.Core.Localization;

public record LanguageModel
{
    public string? Loading { get; set; }
    public string? NoImage { get; set; }
    public string? Files { get; set; }
    public string? File { get; set; }
    
    public string? Cut { get; set; }
    public string? SelectAll { get; set; }
    public string? PercentComplete { get; set; }
    public string? CropMessage { get; set; }
    public string? ClipboardImage { get; set; }
    public string? Base64Image { get; set; }
    public string? Close { get; set; }
    public string? RestoreDown { get; set; }
    public string? Maximize { get; set; }
    public string? Minimize { get; set; }
    public string? Fullscreen { get; set; }
    public string? NewWindow { get; set; }
    public string? About { get; set; }
    public string? InfoWindowTitle { get; set; }
    public string? ApplicationShortcuts { get; set; }
    public string? ImageControl { get; set; }
    public string? FileManagement { get; set; }
    public string? DragFileTo { get; set; }
    public string? DragImage { get; set; }
    public string? AdditionalFunctions { get; set; }
    public string? Reload { get; set; }
    public string? Credits { get; set; }
    public string? IconsUsed { get; set; }
    public string? GithubRepo { get; set; }
    public string? ViewLicenseFile { get; set; }
    public string? Version { get; set; }
    public string? ChangeKeybindingText { get; set; }
    public string? ResetButtonText { get; set; }
    public string? SelectGalleryThumb { get; set; }
    public string? PressKey { get; set; }
    public string? ChangeKeybindingTooltip { get; set; }
    public string? ScrollAndRotate { get; set; }
    public string? ScrollUp { get; set; }
    public string? ScrollDown { get; set; }
    public string? ScrollToTop { get; set; }
    public string? ScrollToBottom { get; set; }
    public string? SetStarRating { get; set; }
    public string? _1Star { get; set; }
    public string? _2Star { get; set; }
    public string? _3Star { get; set; }
    public string? _4Star { get; set; }
    public string? _5Star { get; set; }
    public string? RemoveStarRating { get; set; }
    public string? WindowScaling { get; set; }
    public string? NormalWindow { get; set; }
    public string? FillHeight { get; set; }
    public string? AutoFitWindow { get; set; }
    public string? InterfaceConfiguration { get; set; }
    public string? HideShowInterface { get; set; }
    public string? ToggleFullscreen { get; set; }
    public string? WindowManagement { get; set; }
    public string? InfoWindow { get; set; }
    public string? EscCloseTooltip { get; set; }
    public string? CloseApp { get; set; }
    public string? MoveWindow { get; set; }
    public string? CenterWindow { get; set; }
    public string? HighlightColor { get; set; }
    public string? Theme { get; set; }
    public string? DarkTheme { get; set; }
    public string? LightTheme { get; set; }
    public string? ChangingThemeRequiresRestart { get; set; }
    public string? SetCurrentImageAsWallpaper { get; set; }
    public string? Fill { get; set; }
    public string? Center { get; set; }
    public string? Fit { get; set; }
    public string? Tile { get; set; }
    public string? Stretch { get; set; }
    public string? Language { get; set; }
    public string? CtrlToZoom { get; set; }
    public string? ScrollToZoom { get; set; }
    public string? ScrollDirection { get; set; }
    public string? Reverse { get; set; }
    public string? Forward { get; set; }
    public string? GeneralSettings { get; set; }
    public string? Appearance { get; set; }
    public string? MiscSettings { get; set; }
    public string? SearchSubdirectory { get; set; }
    public string? StayTopMost { get; set; }
    public string? StayCentered { get; set; }
    public string? ColoredWindowBorder { get; set; }
    public string? ShowButtonsInHiddenUI { get; set; }
    public string? Apply { get; set; }
    public string? AdjustTimingForSlideshow { get; set; }
    public string? AdjustTimingForZoom { get; set; }
    public string? SecAbbreviation { get; set; }
    public string? RestartApp { get; set; }
    public string? CheckForUpdates { get; set; }
    public string? AllowZoomOut { get; set; }
    public string? AdjustNavSpeed { get; set; }
    public string? ToggleTaskbarProgress { get; set; }
    public string? ShowFileSavingDialog { get; set; }
    public string? ShowBottomToolbar { get; set; }
    public string? HideBottomToolbar { get; set; }
    public string? BottomGalleryItemSize { get; set; }
    public string? ExpandedGalleryItemSize { get; set; }
    public string? ShowBottomGalleryWhenUiIsHidden { get; set; }
    public string? ImageAliasing { get; set; }
    public string? HighQuality { get; set; }
    public string? NearestNeighbor { get; set; }
    public string? NegativeColors { get; set; }
    public string? BlackAndWhite { get; set; }
    public string? ColorTone { get; set; }
    public string? OldMovie { get; set; }
    public string? Bloom { get; set; }
    public string? Gloom { get; set; }
    public string? Monochrome { get; set; }
    public string? WaveWarper { get; set; }
    public string? Underwater { get; set; }
    public string? BandedSwirl { get; set; }
    public string? Swirl { get; set; }
    public string? Ripple { get; set; }
    public string? RippleAlt { get; set; }
    public string? Blur { get; set; }
    public string? DirectionalBlur { get; set; }
    public string? TelescopicBlur { get; set; }
    public string? Pixelate { get; set; }
    public string? Embossed { get; set; }
    public string? SmoothMagnify { get; set; }
    public string? Pivot { get; set; }
    public string? PaperFold { get; set; }
    public string? PencilSketch { get; set; }
    public string? Sketch { get; set; }
    public string? ToneMapping { get; set; }
    public string? FrostyOutline { get; set; }
    public string? Bands { get; set; }
    public string? GlassTile { get; set; }
    public string? Navigation { get; set; }
    public string? NextImage { get; set; }
    public string? PrevImage { get; set; }
    public string? LastImage { get; set; }
    public string? FirstImage { get; set; }
    public string? ToggleLooping { get; set; }
    public string? Slideshow { get; set; }
    public string? NextFolder { get; set; }
    public string? PrevFolder { get; set; }
    public string? Zoom { get; set; }
    public string? ZoomIn { get; set; }
    public string? ZoomOut { get; set; }
    public string? Pan { get; set; }
    public string? MouseDrag { get; set; }
    public string? ResetZoom { get; set; }
    public string? ToggleScroll { get; set; }
    public string? Open { get; set; }
    public string? OpenWith { get; set; }
    public string? OpenFileDialog { get; set; }
    public string? OpenLastFile { get; set; }
    public string? ShowInFolder { get; set; }
    public string? Save { get; set; }
    public string? Print { get; set; }
    public string? RecentFiles { get; set; }
    public string? FileProperties { get; set; }
    public string? DeleteFile { get; set; }
    public string? Folder { get; set; }
    public string? FullPath { get; set; }
    public string? RenameFile { get; set; }
    public string? UnsupportedFile { get; set; }
    public string? SortFilesBy { get; set; }
    public string? FileName { get; set; }
    public string? FileSize { get; set; }
    public string? CreationTime { get; set; }
    public string? FileExtension { get; set; }
    public string? LastAccessTime { get; set; }
    public string? LastWriteTime { get; set; }
    public string? Random { get; set; }
    public string? Ascending { get; set; }
    public string? Descending { get; set; }
    public string? Settings { get; set; }
    public string? Scrolling { get; set; }
    public string? Looping { get; set; }
    public string? FitToWindow { get; set; }
    public string? ShowUI { get; set; }
    public string? HideUI { get; set; }
    public string? ChangeBackground { get; set; }
    public string? ChangeBackgroundTooltip { get; set; }
    public string? Copy { get; set; }
    public string? AddedToClipboard { get; set; }
    public string? CopyFile { get; set; }
    public string? DuplicateFile { get; set; }
    public string? FileCopy { get; set; }
    public string? FileCopyPath { get; set; }
    public string? FileCopyPathMessage { get; set; }
    public string? FileCutMessage { get; set; }
    public string? CopyImage { get; set; }
    public string? CopiedImage { get; set; }
    public string? CopyImageTooltip { get; set; }
    public string? FilePaste { get; set; }
    public string? ImageInfo { get; set; }
    public string? Image { get; set; }
    public string? Width { get; set; }
    public string? Height { get; set; }
    public string? Date { get; set; }
    public string? Created { get; set; }
    public string? Modified { get; set; }
    public string? Size { get; set; }
    public string? Pixels { get; set; }
    public string? SizeMp { get; set; }
    public string? MegaPixels { get; set; }
    public string? Resolution { get; set; }
    public string? Dpi { get; set; }
    public string? DiskSize { get; set; }
    public string? AspectRatio { get; set; }
    public string? Portrait { get; set; }
    public string? Landscape { get; set; }
    public string? Square { get; set; }
    public string? PrintSizeIn { get; set; }
    public string? PrintSizeCm { get; set; }
    public string? Centimeters { get; set; }
    public string? Inches { get; set; }
    public string? BitDepth { get; set; }
    public string? SizeTooltip { get; set; }
    public string? Del { get; set; }
    public string? Ctrl { get; set; }
    public string? Shift { get; set; }
    public string? Alt { get; set; }
    public string? Space { get; set; }
    public string? Enter { get; set; }
    public string? Esc { get; set; }
    public string? NumpadPlus { get; set; }
    public string? NumpadMinus { get; set; }
    public string? MouseWheel { get; set; }
    public string? MouseKeyForward { get; set; }
    public string? MouseKeyBack { get; set; }
    public string? DoubleClick { get; set; }
    public string? Left { get; set; }
    public string? Right { get; set; }
    public string? Up { get; set; }
    public string? Down { get; set; }
    public string? SavingFileFailed { get; set; }
    public string? UnexpectedError { get; set; }
    public string? NoImages { get; set; }
    public string? UnableToRender { get; set; }
    public string? DropToLoad { get; set; }
    public string? SetAs { get; set; }
    public string? SetAsWallpaper { get; set; }
    public string? SetAsLockScreenImage { get; set; }
    public string? Crop { get; set; }
    public string? RotateRight { get; set; }
    public string? RotateLeft { get; set; }
    public string? Rotated { get; set; }
    public string? Flip { get; set; }
    public string? Flipped { get; set; }
    public string? Unflip { get; set; }
    public string? Orientation { get; set; }
    public string? BadArchive { get; set; }
    public string? PasswordArchive { get; set; }
    public string? SentFileToRecycleBin { get; set; }
    public string? DeletedFile { get; set; }
    public string? AnErrorOccuredWhenDeleting { get; set; }
    public string? ScrollingEnabled { get; set; }
    public string? ScrollingDisabled { get; set; }
    public string? ConvertedToBase64 { get; set; }
    public string? LoopingEnabled { get; set; }
    public string? LoopingDisabled { get; set; }
    public string? Applying { get; set; }
    public string? ShowInfoWindow { get; set; }
    public string? ShowAllSettingsWindow { get; set; }
    public string? ToggleBackgroundColor { get; set; }
    public string? ShowImageGallery { get; set; }
    public string? GoToImageAtSpecifiedIndex { get; set; }
    public string? AdjustZoomLevel { get; set; }
    public string? PasteImageFromClipholder { get; set; }
    public string? SendCurrentImageToRecycleBin { get; set; }
    public string? StartSlideshow { get; set; }
    public string? CloseGallery { get; set; }
    public string? ShowBottomGallery { get; set; }
    public string? HideBottomGallery { get; set; }
    public string? StretchImage { get; set; }
    public string? OptimizeImage { get; set; }
    public string? Effects { get; set; }
    public string? EffectsTooltip { get; set; }
    public string? CropPicture { get; set; }
    public string? ShowImageInfo { get; set; }
    public string? ColorPickerTool { get; set; }
    public string? ColorPickerToolTooltip { get; set; }
    public string? ShowResizeWindow { get; set; }
    public string? Resize { get; set; }
    public string? ResizeImage { get; set; }
    public string? NoResize { get; set; }
    public string? BatchResize { get; set; }
    public string? SourceFolder { get; set; }
    public string? OutputFolder { get; set; }
    public string? ConvertTo { get; set; }
    public string? NoConversion { get; set; }
    public string? Compression { get; set; }
    public string? Lossless { get; set; }
    public string? Lossy { get; set; }
    public string? Quality { get; set; }
    
    public string? SaveAs { get; set; }
    public string? Percentage { get; set; }
    public string? GenerateThumbnails { get; set; }
    public string? Thumbnail { get; set; }
    public string? Start { get; set; }
    public string? Cancel { get; set; }
    public string? None { get; set; }
    public string? ApplicationStartup { get; set; }
    public string? OpenInSameWindow { get; set; }
    public string? NoChange { get; set; }
    public string? Meter { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Altitude { get; set; }
    public string? Authors { get; set; }
    public string? DateTaken { get; set; }
    public string? Copyright { get; set; }
    public string? CameraModel { get; set; }
    public string? ResolutionUnit { get; set; }
    public string? ColorRepresentation { get; set; }
    public string? CompressedBitsPixel { get; set; }
    public string? ExposureTime { get; set; }
    public string? ExposureBias { get; set; }
    public string? ExposureProgram { get; set; }
    public string? Title { get; set; }
    public string? Subject { get; set; }
    public string? Software { get; set; }
    public string? Uncalibrated { get; set; }
    public string? CameraMaker { get; set; }
    public string? FNumber { get; set; }
    public string? Fstop { get; set; }
    public string? FocalLength { get; set; }
    public string? MaxAperture { get; set; }
    public string? DigitalZoom { get; set; }
    public string? ISOSpeed { get; set; }
    public string? NotDefined { get; set; }
    public string? Manual { get; set; }
    public string? Normal { get; set; }
    public string? AperturePriority { get; set; }
    public string? ShutterPriority { get; set; }
    public string? CreativeProgram { get; set; }
    public string? FocalLength35mm { get; set; }
    public string? MeteringMode { get; set; }
    public string? Contrast { get; set; }
    public string? Saturation { get; set; }
    public string? Sharpness { get; set; }
    public string? WhiteBalance { get; set; }
    public string? LensModel { get; set; }
    public string? High { get; set; }
    public string? Low { get; set; }
    public string? Soft { get; set; }
    public string? Hard { get; set; }
    public string? Auto { get; set; }
    public string? Flash { get; set; }
    public string? FlashMode { get; set; }
    public string? FlashEnergy { get; set; }
    public string? FlashFired { get; set; }
    public string? FlashDidNotFire { get; set; }
    public string? StrobeReturnLightDetected { get; set; }
    public string? StrobeReturnLightNotDetected { get; set; }
    public string? RedEyeReduction { get; set; }
    public string? Brightness { get; set; }
    public string? LightSource { get; set; }
    public string? Unknown { get; set; }
    public string? OtherTxt { get; set; }
    public string? Daylight { get; set; }
    public string? Fluorescent { get; set; }
    public string? FineWeather { get; set; }
    public string? CloudyWeather { get; set; }
    public string? Shade { get; set; }
    public string? DaylightFluorescent { get; set; }
    public string? DayWhiteFluorescent { get; set; }
    public string? CoolWhiteFluorescent { get; set; }
    public string? WhiteFluorescent { get; set; }
    public string? PhotometricInterpretation { get; set; }
    public string? ExifVersion { get; set; }
    public string? LensMaker { get; set; }
    public string? Uniform { get; set; }
    public string? UniformToFill { get; set; }
    public string? FillSquare { get; set; }
    public string? GallerySettings { get; set; }
    public string? GalleryThumbnailStretch { get; set; }
    public string? BottomGalleryThumbnailStretch { get; set; }
    public string? SideBySide { get; set; }
    public string? SideBySideTooltip { get; set; }
    public string? GlassTheme { get; set; }
    
    public string? Reset { get; set; }
    public string? AdvanceBy10Images { get; set; }
    public string? AdvanceBy100Images { get; set; }
    public string? GoBackBy10Images { get; set; }
    public string? GoBackBy100Images { get; set; }
}