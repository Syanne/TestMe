using Test_me_alfa.Common;
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
using Windows.UI;

namespace Test_me_alfa
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SampleDataSource sds;
        GridViewItem selectedItem;

        public MainPage()
        {
            this.InitializeComponent();
            sds = new SampleDataSource();

            for (int i = 1; i <= 3; i++)
            {
                sds.AddGroup(i);
                sds.grps[i - 1].Foreground = BasicColor;
            }

            cvsMain.Source = sds.grps;
            flipStatic.ItemsSource = sds.grps[0].group;            
        }

        
        /// <summary>
        /// Follow the Test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = (sender as ListView).SelectedIndex;
            int groupId = (int)(sender as ListView).Tag;

            flyingFlip.ItemsSource = sds.grps[groupId - 1].group;
            flyingFlip.SelectedIndex = index;

            hiddenGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            
            //    this.Frame.Navigate(typeof(TestPage), ((Member)e.ClickedItem).Path);
        }
        

        #region Ads
        //private AdController myController;

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    adWrapperGrid.Loaded += adWrapperGrid_Loaded;
        //}
        //void adWrapperGrid_Loaded(object sender, RoutedEventArgs e)
        //{
        //    myController = new AdController(adWrapperGrid, "600480938");
        //    myController.LoadAd();
        //}
        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    myController.DestroyAd();
        //    adWrapperGrid.Loaded -= adWrapperGrid_Loaded;
        //    base.OnNavigatedFrom(e);
        //}

        #endregion

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
            var dial = new MessageDialog("Спасибо, что скачали приложение!\n\nПриложению очень помогут Ваши отзывы и оценки.\n\nЕсли Вы обнаружили проблемы или возникли вопросы, пишите на ящик: syanne.red@gmail.com");
            dial.Commands.Add(new UICommand("Оставить отзыв",
               new UICommandInvokedHandler((args) =>
               {
                   GottStore();
               })));
            dial.Commands.Add(new UICommand("Назад"));

            var command = await dial.ShowAsync();
        }

        #region Comments

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            GottStore();
        }

        private async void GottStore()
        {
            var uri = new Uri("ms-windows-store:PDP?PFN=36856Syanne.TestMe_x48427g2pbxee");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        #endregion

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
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

        private async void Calendar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var uri = new Uri("ms-windows-store:PDP?PFN=4b1a9631-bca2-40cf-8528-56a79e73a9d6");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
        
        private void Name_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //(sender as GridViewItem).Foreground = SelectedColor;
           // selectedItem.Foreground = BasicColor;
            //selectedItem = (sender as GridViewItem);

            var number = Convert.ToInt32((sender as GridViewItem).Tag);
            dataGrid.ItemsSource = sds.grps[number].group;
            flipStatic.ItemsSource = sds.grps[number].group;

            if (hiddenGrid.Visibility == Windows.UI.Xaml.Visibility.Visible)
                hiddenGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void staticFlip_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TestPage), ((StackPanel)sender).Tag);
        }


        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var number = Convert.ToInt32((sender as GridViewItem).Tag);
            flyingFlip.ItemsSource = sds.grps[number].group;

        }

        private void gt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var number = Convert.ToInt32(selectedItem.Tag);
            flyingFlip.ItemsSource = sds.grps[number].group;
           
            number = Convert.ToInt32((sender as Grid).Tag);
            flyingFlip.SelectedItem = flyingFlip.Items.FirstOrDefault(s => (s as Member).UniqueId == number);
            hiddenGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TestPage), (flyingFlip.SelectedItem as Member).Path);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            hiddenGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void ScrollViewer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            int index = (int)(sender as ScrollViewer).Tag - 1;
            (typeNamesListView.Items[index] as Group).Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Black) ;
        }

        private void ScrollViewer_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            int index = (int)(sender as ScrollViewer).Tag - 1;
            (typeNamesListView.Items[index] as Group).Foreground = BasicColor;
        }

    }
}
