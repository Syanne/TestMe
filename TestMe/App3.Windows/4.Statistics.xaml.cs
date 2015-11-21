using Test_me_alfa.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;

// Документацию по шаблону элемента "Основная страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=234237

namespace Test_me_alfa
{
    /// <summary>
    /// Основная страница, которая обеспечивает характеристики, являющимися общими для большинства приложений.
    /// </summary>
    public sealed partial class Statistics : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// Эту настройку можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        static bool hooligan;

        /// <summary>
        /// NavigationHelper используется на каждой странице для облегчения навигации и 
        /// управление жизненным циклом процесса
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public Statistics()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            if (Window.Current.Bounds.Width <= Window.Current.Bounds.Height)
                SizeCorrector(Window.Current.Bounds.Width, Window.Current.Bounds.Width);
            else
                SizeCorrector(Window.Current.Bounds.Height, Window.Current.Bounds.Width);

            hooligan = false;
            LoadFromFile();


            descBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }


        /// <summary>
        /// Loads from file all information, then puts it into the ListView
        /// </summary>
        private void LoadFromFile()
        {
            XDocument doc;
                var folder = ApplicationData.Current.LocalFolder;
            try
            {
                //file loading
                doc = XDocument.Load(folder.Path + @"\PersData.xml");

                    //get all descendants
                    IEnumerable<XElement> collect = doc.Root.Elements();

                    //put each into the LV
                    for (int i = 0; i < Convert.ToInt32(doc.Root.FirstAttribute.Value); i++)
                    {
                        TextBlock tb = new TextBlock();
                        tb.Tag = collect.ElementAt(i).Attribute("max").Value + "-" + collect.ElementAt(i).Attribute("result").Value;

                        if (collect.ElementAt(i).LastAttribute.Name == "desc")
                            tb.Tag += "-" + collect.ElementAt(i).Attribute("desc").Value;
                        else tb.Tag += "-empty";


                        tb.Text = String.Format("{0}. {1}", i+1, collect.ElementAt(i).Attribute("name").Value);
                        tb.TextWrapping = TextWrapping.Wrap;
                        tb.FontSize = 20;
                        tb.Height = 80;

                        lvMain.Items.Add(tb);
                        lvMain.IsSynchronizedWithCurrentItem.GetValueOrDefault(true);
                    }
            }
            catch
            {
                doc = XDocument.Load(@"PersData.xml");

                var collect = doc.Root.Elements();

                TextBlock tb = new TextBlock();
                tb.Tag = collect.FirstOrDefault().Attribute("max").Value + "-" + collect.FirstOrDefault().Attribute("result").Value;

                tb.Text = collect.FirstOrDefault().Attribute("name").Value;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.FontSize = 20;

                lvMain.Items.Add(tb);
                lvMain.IsSynchronizedWithCurrentItem.GetValueOrDefault(true);
            }
        }


        /// <summary>
        /// PopUp
        /// </summary>
        /// <param name="lol"> String for Message</param>
        private async void ShowPop(string lol)
        {
            var dial = new MessageDialog(lol);
            var command = await dial.ShowAsync();

            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// It takes info from selected item's tag, transforms it, puts into the List, then - into the Chart
        /// </summary>
        /// <param name="hs">Our source is Struct</param>
        private void CreateGraph(object Tag)
        {
            try
            {
                //It has to split text from Tag
                string lol = Tag.ToString();
                string[] split = lol.Split(new Char[] { '-' });

                List<Graph> gList = new List<Graph>();

                //Put info into the Chart
                gList.Add(new Graph() { Title = "Максимум", Result = Convert.ToInt32(Convert.ToInt32(split[0])) - Convert.ToInt32(split[1]) });
                gList.Add(new Graph() { Title = "Ваш результат", Result = Convert.ToInt32(split[1]) });

                if (hooligan == true)
                {
                    Charty.Series.Clear();
                    hooligan = true;
                }
                (Charty.Series[0] as PieSeries).ItemsSource = gList;
                if (split[2] != "empty")
                    descBlock.Text = split[2];
                else ChangeTextBtn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            catch 
            {
                ShowPop("Произошла ошибка при построении графика.");
            }
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Window Size Correction


        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void pageRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            SizeCorrector(e.Size.Height, e.Size.Width);
        }


        private void SizeCorrector(double Height, double Width)
        {
            Charty.Width = Width / 2 - 100;

            descBlock.Width = Width / 2;
            ChangeTextBtn.Margin = new Thickness(Width / 2, 0, 0, 100);

            lvMain.Width = Width / 2 - 50;
                pageTitle.FontSize = Height / 25;
                Charty.Height = Height - 380;

            if (Window.Current.Bounds.Width < 700)
                ShowPop("Чтобы увидеть результаты, переверните экран планшета или откройте приложение на весь экран");

        }

        #endregion

        /// <summary>
        /// Menu Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }


        //which item selected - that info in the Chart
        private void lvMain_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is TextBlock)
            {
                TextBlock r = e.ClickedItem as TextBlock;

                gName.Text = r.Text;
                CreateGraph(r.Tag);
            }
        }


        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (Charty.IsEnabled)
            {
                Charty.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                Charty.IsEnabled = false;
                descBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ChangeTextBtn.Content = "График";

            }
            else
            {
                Charty.Visibility = Windows.UI.Xaml.Visibility.Visible;
                Charty.IsEnabled = true;
                descBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ChangeTextBtn.Content = "Текст результата";
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(MainPage));
        }

    }
}
