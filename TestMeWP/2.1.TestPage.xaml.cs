using TestMe.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;

namespace TestMe
{
    /// <summary>
    /// Основная страница, которая обеспечивает характеристики, являющимися общими для большинства приложений.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            DialogMessageOpened = false;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack & !DialogMessageOpened)
            {
                this.Frame.Navigate(typeof(MainPage));
                e.Handled = true;
            }
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
            IAmNavigted(e.Parameter.ToString());
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
            if (task.IsEnabled == true && task.Text != "")
            {
                if (task.Text.ToLower() == task.Tag.ToString())
                {
                    testInfo.result += 1;
                }

                testInfo.myAnswers.Add(task.Text);
                task.Text = "";

                //move next
                testInfo.position += 1;
                if (testInfo.position <= testInfo.qCount)
                    TestWithTimer();
                else ResultHandler();
            }
            //logic and psycho
            else
            {
                foreach (Control rb in lv.Items)
                    if (rb is RadioButton)
                    {
                        RadioButton check = rb as RadioButton;
                        if (check.IsChecked == true)
                        {
                            testInfo.myAnswers.Add((check.Content as TextBlock).Text);
                            testInfo.result += Convert.ToInt32(check.Tag);

                            //move next
                            testInfo.position += 1;
                            if (testInfo.position <= testInfo.qCount)
                                Test();
                            else ResultHandler();
                        }
                    }
            }
        }

        /// <summary>
        /// Popup
        /// </summary>
        /// <param name="message">Message</param>
        private async void ShowPop(string message)
        {
            var dial = new MessageDialog(message);
            var command = await dial.ShowAsync();
        }

        /// <summary>
        /// adds to the backButton on the topAppBar the messageDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void backButton_Click(object sender, RoutedEventArgs e)
        {
            var dial = new MessageDialog("тест не завершен! вы действительно хотите покинуть страницу?");
                
            dial.Commands.Add(new UICommand("продолжить"));
            dial.Commands.Add(new UICommand("вернуться",
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
