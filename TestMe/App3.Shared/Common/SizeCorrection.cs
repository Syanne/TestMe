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

        public double FlipFontSize { get; set; }

        public Thickness GridMargin { get; set; }

        public double FlipWidth {get; set;}

        public double FlipHeight { get; set; }

        public double FlyoutGridHeight { get; set; }

        public SizeCorrection()
        {
            ItemHeight = (Window.Current.Bounds.Height - 230) / 4;
            FlipWidth = ItemHeight * 2;

            if (Window.Current.Bounds.Height < 1100)
            {                
                ItemFontSize = 16;
                FlipFontSize = 18;
                FlipHeight = ItemHeight * 2;
            }
            else
            {
                ItemFontSize = 20;
                FlipFontSize = 22;
                FlipHeight = ItemHeight * 3.5 / 2;
            }

            FlipWidth = ItemHeight * 2;
            
            GridMargin = new Thickness(FlipWidth + 70, 150, 50, 100);

            FlyoutGridHeight = FlipHeight*3/2;
        }
    }
}
