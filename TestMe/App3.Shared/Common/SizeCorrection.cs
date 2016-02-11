using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Test_me_alfa
{
    /// <summary>
    /// Size corrector, actually.
    /// Makes the application's GUI scalable
    /// </summary>
    public class SizeCorrection
    {
        public double ItemHeight { get; set; }

        public double ItemFontSize { get; set; }

        public double ListViewHeight { get; set; }

        public double FlipFontSize { get; set; }

        public Thickness GridMargin { get; set; }
        public Thickness TitlesMargin { get; set; }

        public double FlipWidth {get; set;}

        public double FlipHeight { get; set; }

        public double FlyoutGridHeight { get; set; }

        public double ButtonHeight { get; set; }
        public double ButtonWidth { get; set; }

        public SizeCorrection()
        {
            ItemHeight = (Window.Current.Bounds.Height - 230) / 4;
            FlipWidth = ItemHeight * 2;

            if (Window.Current.Bounds.Height < 1100)
            {                
                ItemFontSize = 16;
                FlipFontSize = 18;
                FlipHeight = ItemHeight * 2;
                ButtonHeight = 50;
                ButtonWidth = 110;
                FlyoutGridHeight = FlipHeight + 150;
            }
            else
            {
                ItemFontSize = 20;
                FlipFontSize = 22;
                FlipHeight = ItemHeight * 3.5 / 2;
                ButtonHeight = 50;
                ButtonWidth = 150;
                FlyoutGridHeight = FlipHeight * 3 / 2;
            }

            FlipWidth = ItemHeight * 2;
            
            ListViewHeight = Window.Current.Bounds.Height - 340;
            GridMargin = new Thickness(FlipWidth + 70, 100, 20, 50);
            TitlesMargin = new Thickness(FlipWidth + 70, 80, 10, 10);
        }
    }
}
