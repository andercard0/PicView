﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static PicView.UILogic.Animations.MouseOverAnimations;

namespace PicView.UILogic.UserControls
{
    public partial class FlipButton : UserControl
    {
        public FlipButton()
        {
            InitializeComponent();

            Loaded += delegate
            {
                TheButton.PreviewMouseLeftButtonDown += delegate
                {
                    PreviewMouseButtonDownAnim(FlipButtonBrush);
                };

                TheButton.MouseEnter += delegate
                {
                    ButtonMouseOverAnim(FlipButtonBrush, true);

                    ToolTip = TheButton.IsChecked.Value ?
                        Application.Current.Resources["Unflip"] :
                        Application.Current.Resources["Flip"];
                };

                TheButton.MouseLeave += delegate
                {
                    ButtonMouseLeaveAnimBgColor(FlipButtonBrush, false);
                };

                TheButton.Click += delegate 
                {
                    TransformImage.Rotation.Flip();
                };

                TheButton.Checked += FlipButton_Checked;
                TheButton.Unchecked += FlipButton_Unchecked;
            };
        }

        // Change FlipButton's icon when (un)checked
        private void FlipButton_Unchecked(object sender, RoutedEventArgs e)
        {
            FlipPath.Data = Geometry.Parse("M192,96v64h248c4.4,0,8,3.6,8,8v240c0,4.4-3.6,8-8,8H136c-4.4,0-8-3.6-8-8v-48c0-4.4,3.6-8,8-8h248V224H192v64L64,192 L192, 96z");
        }

        private void FlipButton_Checked(object sender, RoutedEventArgs e)
        {
            FlipPath.Data = Geometry.Parse("M448,192l-128,96v-64H128v128h248c4.4,0,8,3.6,8,8v48c0,4.4-3.6,8-8,8H72c-4.4,0-8-3.6-8-8V168c0-4.4,3.6-8,8-8h248V96 L448, 192z");
        }
    }
}