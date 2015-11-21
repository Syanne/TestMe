using Test_me_alfa.Common;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.ViewManagement;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace Test_me_alfa
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        //SampleDataSource sds;
        public BlankPage1()
        {
            this.InitializeComponent();
            //have we initialize the main source?     
            SampleDataSource sds = new SampleDataSource();
            
            for (int i = 1; i <= 3; i++)
                sds.AddGroup(i, 4);

            cvsMain.Source = sds.grps;
        }

        /// <summary>
        /// вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void lvMain_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (((Member)e.ClickedItem).UniqueId < 10)
                this.Frame.Navigate(typeof(GroupDetailPage), (((Member)e.ClickedItem).UniqueId));

            else this.Frame.Navigate(typeof(TestPageWP), ((Member)e.ClickedItem).Path);
        }

        #region Bottom Menu

        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            var dial = new MessageDialog("Спасибо, что скачали приложение!\n\nПару слов об обновлениях.\nвыпускаются они теперь 3-4 раза в месяц.\nПриложению очень помогут Ваши отзывы и оценки. Уделите минуту своего времени - и выпуски будут появляться чаще!\n\nПо всем вопросам Вы можете обратиться на почтовый ящик: overpt0tected@yandex.ru");
            dial.Commands.Add(new UICommand("Оставить отзыв",
               new UICommandInvokedHandler((args) =>
               {
                   GottStore();
               })));
            dial.Commands.Add(new UICommand("Назад"));

            var command = await dial.ShowAsync();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var dial = new MessageDialog("Расскажите, какие тесты Вы хотите увидеть в следующем выпуске. Вы окажете неоценимую помощь в улучшении приложения!");
            dial.Commands.Add(new UICommand("Написать",
               new UICommandInvokedHandler((args) =>
               {
                   GottStore();
               })));
            dial.Commands.Add(new UICommand("Назад"));

            var command = await dial.ShowAsync();
        }

        private async void GottStore()
        {
            var uri = new Uri("ms-windows-store:PDP?PFN=" + Windows.ApplicationModel.Package.Current.Id.FamilyName);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(Statistics));
        }
        #endregion

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {    // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            this.Frame.Navigate(typeof(GroupDetailPage), ((Group)group).GroupId);
        }

        
    }
}
