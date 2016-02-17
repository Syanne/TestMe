using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Phone.UI.Input;
using Windows.ApplicationModel.Store;

namespace TestMe
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            sds = new SampleDataSource();

            for (int i = 1; i <= 3; i++)
            {
                switch (i)
                {
                    case 1: sds.AddGroup(i, "психологические"); break;
                    case 2: sds.AddGroup(i, "логика и IQ"); break;
                    case 3: sds.AddGroup(i, "задачи и загадки"); break;
                    default: sds.AddGroup(i, ""); break;
                }
            }

            cvsMain.Source = sds.grps;
            TypeName.Text = sds.grps[mainFlip.SelectedIndex].GroupName;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;    
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            theFlyout.Hide();
        }

        #region Bottom Menu

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutMessage();
        }

        private async void Message()
        {
            var mailto = new Uri("mailto:?to=syanne.red@gmail.com&amp;subject=Holiday Calendar&amp;body=Hello, Syanne!");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        private void RateApp_Click(object sender, RoutedEventArgs e)
        {
            GottStore();
        }

        private async void GottStore()
        {
            var uri = new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        #endregion

        private void staticFlip_Tapped(object sender, TappedRoutedEventArgs e)
        {
            theFlyout.Hide();
            this.Frame.Navigate(typeof(TestPage), ((StackPanel)sender).Tag);
        }

        private void gt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var number = Convert.ToInt32(mainFlip.SelectedIndex);
            flyingFlip.ItemsSource = sds.grps[number].group;

            number = Convert.ToInt32((sender as StackPanel).Tag);
            flyingFlip.SelectedItem = flyingFlip.Items.FirstOrDefault(s => (s as Member).UniqueId == number);
            FlyoutBase.ShowAttachedFlyout(mainFlip as FrameworkElement);
        }
      
        private void mainFlip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeName.Text = sds.grps[mainFlip.SelectedIndex].GroupName;
        }
    }
}
