using Test_me_alfa.Common;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Controls.Primitives;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace Test_me_alfa
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TestPageWP : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        TestInfo hs = new TestInfo();

        /// <summary>
        /// Эту настройку можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper используется на каждой странице для облегчения навигации и 
        /// управление жизненным циклом процесса
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public TestPageWP()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;

            //have we initialize the main source?
            hs = new TestInfo();
            hs.position = 1;
            hs.result = 0;
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Любое сохраненное состояние также является
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы, и
        /// словарь состояния, сохраненного этой страницей в ходе предыдущего
        /// сеансом. Состояние будет равно значению NULL при первом посещении страницы.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Сохраняет состояние, связанное с данной страницей, в случае приостановки приложения или
        /// удаления страницы из кэша навигации.  Значения должны соответствовать требованиям сериализации
        /// <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">Источник события; как правило, <see cref="NavigationHelper"/></param>
        /// <param name="e">Данные события, которые предоставляют пустой словарь для заполнения
        /// сериализуемым состоянием.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region Регистрация NavigationHelper

        /// Методы, предоставленные в этом разделе, используются исключительно для того, чтобы
        /// NavigationHelper для отклика на методы навигации страницы.
        /// 
        /// Логика страницы должна быть размещена в обработчиках событий для 
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// и <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// Параметр навигации доступен в методе LoadState 
        /// в дополнение к состоянию страницы, сохраненному в ходе предыдущего сеанса.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            iAmNavigted(e.Parameter.ToString());

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private void lvMain_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.OriginalSource is RadioButton)
            {
                RadioButton r = e.OriginalSource as RadioButton;
                r.IsChecked = true;
            }
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

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            ShowPop("Спасибо, что скачали это приложение!\n\nНадеюсь, оно Вам понравилось и Вы будете пользоваться им с удовольствием. Если возникли какие-либо вопросы или Вы увидели ошибку/опечатку, прошу написать о ней в комментариях на странице приложения в Windows Store.\nСвои пожелания по поводу новых тестов и критику прошу публиковать там же или присылать на ящик:\n\noverpr0tected@yandex.ru\n\nПриложение будет развиваться и дополняться, поэтому не спешите удалять, когда пройдете все тесты ;D");
            //ShowPop(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("ms-windows-store:PDP?PFN=" + Windows.ApplicationModel.Package.Current.Id.FamilyName);
            await Windows.System.Launcher.LaunchUriAsync(uri);
            //ShowPop("Источники, использованные при создании приложения:\n\nА.Карелина \"Большая энциклопедия психологических тестов\"\nН. А. Теленкова \"Тесты на все случаи жизни\"\nСайт Psychist.net\nИнтернет-ресурсы"); 
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //this.Frame.Navigate(typeof(GroupDetailPage), ((Group)group).GroupId);
        }


        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (hs.position < hs.qCount)
            {
                //logic and math
                foreach (Control rb in lv.Items)
                {
                    if (rb is RadioButton)
                    {
                        RadioButton check = rb as RadioButton;
                        if (check.IsChecked == true)
                        {
                            hs.result += Convert.ToInt32(check.Tag);
                            hs.position += 1;
                            Test();
                        }
                    }
                }
            }
            else ResultHandler();
        }


        /// <summary>
        /// adds to the backButton on the topAppBar the messageDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void backButton_Click(object sender, RoutedEventArgs e)
        {
            var dial = new MessageDialog("Тест не завершен! Вы действительно хотите покинуть страницу?");

            dial.Commands.Add(new UICommand("Продолжить"));
            dial.Commands.Add(new UICommand("Меню",
            new UICommandInvokedHandler((args) =>
            {
                this.Frame.Navigate(typeof(BlankPage1));
            })));
            var command = await dial.ShowAsync();
        }

        /// <summary>
        /// if you clicked the item, but not the radio itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lv_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is RadioButton)
            {
                RadioButton r = e.ClickedItem as RadioButton;
                r.IsChecked = true;
            }
        }

        /// <summary>
        /// if you have tapped the item...lol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lv_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is RadioButton)
            {
                RadioButton r = e.OriginalSource as RadioButton;
                r.IsChecked = true;
            }
        }

        private void btnDone_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Lv_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is RadioButton)
            {
                RadioButton r = e.OriginalSource as RadioButton;
                r.IsChecked = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
