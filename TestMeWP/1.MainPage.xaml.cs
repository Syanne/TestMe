using TestMe.Common;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Primitives;

namespace TestMe
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SampleDataSource sds;
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


            //selectedItem = Psych;
            //Psych.Foreground = SelectedColor;
            cvsMain.Source = sds.grps;
            TypeName.Text = sds.grps[mainFlip.SelectedIndex].GroupName;
            //flipStatic.ItemsSource = sds.grps[0].group;            
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

        /// <summary>
        /// PopUp
        /// </summary>
        /// <param name="lol"> String for Message</param>
        private async void ShowPop(string lol)
        {
            var dial = new MessageDialog(lol);
            var command = await dial.ShowAsync();
        }

        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            var dial = new MessageDialog("спасибо, что скачали приложение!\n\nприложению очень помогут ваши отзывы и оценки.\n\nесли вы обнаружили проблемы или возникли вопросы, пишите на ящик: syanne.red@gmail.com");
            dial.Commands.Add(new UICommand("оставить отзыв",
               new UICommandInvokedHandler((args) =>
               {
                   GottStore();
               }))); 
            dial.Commands.Add(new UICommand("служба поддержки",
                new UICommandInvokedHandler((args) =>
                {
                    Message();
                })));
            dial.Commands.Add(new UICommand("назад"));

            var command = await dial.ShowAsync();
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

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            GottStore();
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
            //(sender as GridViewItem).Foreground = SelectedColor;
            //selectedItem.Foreground = MainForeground;
            //selectedItem = (sender as GridViewItem);

            //var number = Convert.ToInt32((sender as GridViewItem).Tag);
            //dataGrid.ItemsSource = sds.grps[number].group;
            //flipStatic.ItemsSource = sds.grps[number].group;
        }

        private void staticFlip_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TestPage), ((StackPanel)sender).Tag);
        }

        private void gt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //var number = Convert.ToInt32(selectedItem.Tag);
            //flyingFlip.ItemsSource = sds.grps[number].group;
           
            //number = Convert.ToInt32((sender as StackPanel).Tag);
            //flyingFlip.SelectedItem = flyingFlip.Items.FirstOrDefault(s => (s as Member).UniqueId == number);
            //FlyoutBase.ShowAttachedFlyout(mainItemGrid as FrameworkElement);
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mainFlip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeName.Text = sds.grps[mainFlip.SelectedIndex].GroupName;
        }

        //private void Flyout_Opened(object sender, object e)
        //{
        //    mainItemGrid.Opacity = 0.3;
        //    leftGrid.Opacity = 0.3;
        //}

        //private void Flyout_Closed(object sender, object e)
        //{
        //    mainItemGrid.Opacity = 1;
        //    leftGrid.Opacity = 1;
        //}

        //private void FlyoutClose_Tapped(object sender, RoutedEventArgs e)
        //{
        //    theFlyout.Hide();
        //    mainItemGrid.Opacity = 1;
        //    leftGrid.Opacity = 1;
        //}
    }
}
