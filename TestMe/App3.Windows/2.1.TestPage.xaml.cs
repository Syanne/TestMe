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
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Controls.Primitives;

namespace Test_me_alfa
{
    /// <summary>
    /// Основная страница, которая обеспечивает характеристики, являющимися общими для большинства приложений.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        HelpStruct hs;

        private double FontSizeOnPage
        {
            get
            {
                 return (Window.Current.Bounds.Width < 1000) ? 20 : 26;
            }
        }

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

        public TestPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            

            if (Window.Current.Bounds.Width <= Window.Current.Bounds.Height)
                SizeCorrector(Window.Current.Bounds.Width, Window.Current.Bounds.Width);
            else 
                SizeCorrector(Window.Current.Bounds.Height, Window.Current.Bounds.Width);

            hs = new HelpStruct();
            hs.position = 1;
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


        /// <summary>
        /// The main button's handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (hs.position < hs.numOfq)
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
                //task
                if (task.IsEnabled == true && task.Text != "")
                {
                        if (task.Text.ToLower() == task.Tag.ToString())
                        {
                            hs.result += 1;
                        }
                        hs.position += 1;
                        task.Text = "";

                        TaskHandler();
                }
            }
            //fuckin' result
            else ResultHandler();

        }

        /// <summary>
        /// Popup
        /// </summary>
        /// <param name="lol">Message</param>
        private async void ShowPop(string lol)
        {
            var dial = new MessageDialog(lol);
            var command = await dial.ShowAsync();
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
               dial.Commands.Add(new UICommand("Вернуться в Главное меню",
               new UICommandInvokedHandler((args) =>
               {
                   this.Frame.Navigate(typeof(MainPage));
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
        /// if you tapped the item...lol
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

        #region Window Size Correction

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            if (task.Visibility == Windows.UI.Xaml.Visibility.Visible)
                task.Focus(FocusState.Keyboard);

        }

        private void pageRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {

            if (Window.Current.Bounds.Width <= Window.Current.Bounds.Height)
                SizeCorrector(e.Size.Width, e.Size.Width);
            else
                SizeCorrector(e.Size.Height, e.Size.Width);
        }

        private void SizeCorrector(double Height, double Width)
        {
            if (Window.Current.Bounds.Width > 700)
            {
                pageTitle.Visibility = Visibility.Visible;
                tbQuest.FontSize = Height / 35;
                tbQuest.Margin = new Thickness(100, 40, 100, 40);
            }
            else
            {
                pageTitle.Visibility = Visibility.Collapsed;
                tbQuest.FontSize = 22;
                tbQuest.Margin = new Thickness(40, 40, 40, 40);
            }

            if (task.IsEnabled == true)
                task.Width = Width / 2;
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
        #endregion

    }
}
