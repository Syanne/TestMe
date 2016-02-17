using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls.Primitives;

namespace TestMe
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GridViewItem selectedItem;

        public MainPage()
        {
            this.InitializeComponent();
            sds = new SampleDataSource();
            
            for (int i = 1; i <= 3; i++)
                sds.AddGroup(i);

            selectedItem = Psych;
            Psych.Foreground = SelectedColor;
            dataGrid.ItemsSource = sds.grps[0].group;
            flipStatic.ItemsSource = sds.grps[0].group;            
        }
        
        /// <summary>
        /// Follow the Test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(TestPage), ((Member)e.ClickedItem).Path);
        }        

        #region Bottom Menu

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutMessage();
        }

        private void RateApp_Click(object sender, RoutedEventArgs e)
        {
            GottStore();
        }

        private async void Message()
        {
            var mailto = new Uri("mailto:?to=syanne.red@gmail.com&amp;subject=Holiday Calendar&amp;body=Hello, Syanne!");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        private async void GottStore()
        {
            var uri = new Uri("ms-windows-store:PDP?PFN=36856Syanne.TestMe_x48427g2pbxee");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void Statistics_Tapped(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Statistics));
        }
        #endregion

        #region Size Correction

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;

            //resize window
            ShowHide();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }


        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            ShowHide();
        }

        /// <summary>
        /// Changing property Height for gvMain and choosing, what to show -list or grid
        /// </summary>
        private void ShowHide()
        {
            //gvMain.Height = Window.Current.Bounds.Height -200;

            //if (Window.Current.Bounds.Width <= 800)
            //{
            //    adDuplexAd.Size = "292x60";
            //}
            //else
            //{
            //    adDuplexAd.Size = "728x90";
            //}
        }

        #endregion

        private void Name_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as GridViewItem).Foreground = SelectedColor;
            selectedItem.Foreground = MainForeground;
            selectedItem = (sender as GridViewItem);

            var number = Convert.ToInt32((sender as GridViewItem).Tag);
            dataGrid.ItemsSource = sds.grps[number].group;
            flipStatic.ItemsSource = sds.grps[number].group;
        }

        private void staticFlip_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TestPage), ((StackPanel)sender).Tag);
        }

        private void gt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var number = Convert.ToInt32(selectedItem.Tag);
            flyingFlip.ItemsSource = sds.grps[number].group;
           
            number = Convert.ToInt32((sender as StackPanel).Tag);
            flyingFlip.SelectedItem = flyingFlip.Items.FirstOrDefault(s => (s as Member).UniqueId == number);
            FlyoutBase.ShowAttachedFlyout(mainItemGrid as FrameworkElement);
        }

        private void Flyout_Opened(object sender, object e)
        {
            mainItemGrid.Opacity = 0.3;
            leftGrid.Opacity = 0.3;
        }

        private void Flyout_Closed(object sender, object e)
        {
            mainItemGrid.Opacity = 1;
            leftGrid.Opacity = 1;
        }

        private void FlyoutClose_Tapped(object sender, RoutedEventArgs e)
        {
            theFlyout.Hide();
            mainItemGrid.Opacity = 1;
            leftGrid.Opacity = 1;
        }
    }
}
