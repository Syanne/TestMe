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
        public double GridWidth { get; set; }

#if WINDOWS_PHONE_APP
        
        //main page
        public double SmallNameHeight { get; set; }
        public double DescriptionHeight { get; set; }

        //test page
        public double TestPageTitleWidth { get; set; }
        public double TestPageGridHeight { get; set; }

        public SizeCorrection()
        {
            ItemHeight = Window.Current.Bounds.Height / 10;
            TextBlockWidth = Window.Current.Bounds.Width - ItemHeight - 20;
            SmallNameHeight = ItemHeight - 10;
            FlipWidth = ItemHeight * 2;
            FlipHeight = Window.Current.Bounds.Height - 270;

            ItemFontSize = ItemHeight / 4;
            FlipFontSize = ItemFontSize - 2;
            DescriptionHeight = ItemFontSize * 2;
            FlyoutGridHeight = FlipHeight + 150;

            FlipWidth = ItemHeight * 2;
            FlyingFlipHeight = Window.Current.Bounds.Height - 300;
            FlyingFlipWidth = Window.Current.Bounds.Width;
            GridWidth = Window.Current.Bounds.Width - FlipWidth - 100;

            TestPageTitleWidth = Window.Current.Bounds.Width - 200;
            TestPageGridHeight = Window.Current.Bounds.Height - ItemHeight - 200;
        }
#else
        //buttons
        public double AppBarButtonWidth { get; set; }
        public double ButtonHeight { get; set; }
        public double ButtonWidth { get; set; }
        
        //grids
        public Thickness GridMargin { get; set; }
        public Thickness TitleMargin { get; set; }

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
