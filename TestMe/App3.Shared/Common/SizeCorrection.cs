using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace TestMe
{
    /// <summary>
    /// Size corrector, actually.
    /// Makes the application's GUI scalable
    /// </summary>
    public class SizeCorrection
    {
        //item
        public double ItemHeight { get; set; }
        public double ItemWidth { get; set; }
        public double TextBlockWidth { get; set; }
        public double ItemFontSize { get; set; }

        //flip & flyout
        public double FlipWidth { get; set; }
        public double FlipHeight { get; set; }
        public double FlyingFlipHeight { get; set; }
        public double FlyingFlipWidth { get; set; }
        public double FlyoutGridHeight { get; set; }
        public double FlipFontSize { get; set; }

        //grids
        public Thickness GridMargin { get; set; }
        public double GridWidth { get; set; }
        public Thickness TitleMargin { get; set; }

        //buttons
        public double AppBarButtonWidth { get; set; }
        public double ButtonHeight { get; set; }
        public double ButtonWidth { get; set; }

#if WINDOWS_PHONE_APP
        public SizeCorrection()
        {
            ItemHeight = Window.Current.Bounds.Height / 8;
            ItemWidth = 300 * ItemHeight / 200;
            TextBlockWidth = Window.Current.Bounds.Width - ItemWidth - 20;
            FlipWidth = ItemHeight * 2;
            FlipHeight = Window.Current.Bounds.Height - 215;

            ItemFontSize = ItemHeight / 5;
            FlipFontSize = 16;
            ButtonHeight = 50;
            ButtonWidth = 110;
            FlyoutGridHeight = FlipHeight + 150;

            FlipWidth = ItemHeight * 2;
            FlyingFlipHeight = FlipWidth * 1.5;
            FlyingFlipWidth = FlipWidth + 10;
            GridWidth = Window.Current.Bounds.Width - FlipWidth - 100;
            GridMargin = new Thickness(FlipWidth + 70, 50, 50, 110);
            TitleMargin = new Thickness(10, 0, 10, 10);
            AppBarButtonWidth = FlipWidth / 3;
        }
#else
        public SizeCorrection()
        {
            ItemHeight = Window.Current.Bounds.Height / 5;
            if (ItemHeight < 200)
                ItemHeight = 200;
            FlipWidth = ItemHeight * 2;

            if (Window.Current.Bounds.Height < 1100)
            {
                ItemFontSize = 16;
                FlipFontSize = 18;
                FlipHeight = ItemHeight * 2 + 100;
                ButtonHeight = 50;
                ButtonWidth = 110;
                FlyoutGridHeight = FlipHeight + 150;
            }
            else
            {
                ItemFontSize = 20;
                FlipFontSize = 22;
                FlipHeight = ItemHeight * 3.5 / 2 + 100;
                ButtonHeight = 50;
                ButtonWidth = 150;
                FlyoutGridHeight = FlipHeight * 3 / 2;
            }

            ItemWidth = 300 * ItemHeight / 200;
            FlipWidth = ItemHeight * 2;
            FlyingFlipHeight = FlipWidth * 1.5;
            FlyingFlipWidth = FlipWidth + 10;
            GridWidth = Window.Current.Bounds.Width - FlipWidth - 100;
            GridMargin = new Thickness(FlipWidth + 70, 50, 50, 110);
            TitleMargin = new Thickness(10, 0, 10, 10);
            AppBarButtonWidth = FlipWidth / 3;
        }
#endif
    }
}
