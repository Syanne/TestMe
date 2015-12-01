using Test_me_alfa.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace Test_me_alfa
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ResultWP : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        //HelpStruct hs;

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

        public ResultWP()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;

            //have we initialize the main source?

        }

        #region Navigation Helper

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
            TestInfo hs = (TestInfo)e.Parameter;

            CreateGraph(hs);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion



        /// <summary>
        /// It's taking some info from source into the List, then - to the Chart
        /// </summary>
        /// <param name="hs">Our source is Struct</param>
        private void CreateGraph(TestInfo hs)
        {
            try
            {
                XDocument document = XDocument.Load(hs.path.ToString());

                //gets info from Tag and opens the file
                List<Graph> gList = new List<Graph>();

                gList.Add(new Graph() { Title = "Ваш результат", Result = Convert.ToInt32(hs.result) });

                string descXML = "";
                string nameXML = document.Element("head").Attribute("name").Value;
                string maxXML = document.Element("head").Attribute("max").Value;

                gList.Add(new Graph()
                {
                    Title = "Осталось",
                    Result = Convert.ToInt32(maxXML) - Convert.ToInt32(hs.result)
                });

                var results = document.Root.Element("end").Elements().ToList();
                foreach (XElement elem in results)
                {
                    if ((int)hs.result >= Convert.ToInt32(elem.Attribute("min").Value))
                    {
                        descXML = elem.Value;
                        tbRes.Text = hs.result.ToString() + " балл(а, ов). \n\n" + descXML;
                        break;
                    }
                }

                //put info into the Chat
                (Charty.Series[0] as PieSeries).ItemsSource = gList;

                //info about result goes to the result-file
                xmlRedactor(nameXML, maxXML, hs.result.ToString(), descXML);


                //2nd and 3rd groups
                descBlock.Text = "ПРАВИЛЬНЫЕ ОТВЕТЫ: \n\n\n";
                int i = 1;
                if (hs.path[7] == '2')
                {
                    foreach (var elem in document.Root.Elements("question"))
                    {
                        descBlock.Text += String.Format("{0}) {1}\nОТВЕТ: {2}\n\n", i, elem.Attribute("name").Value,
                            elem.Elements().Where(el => el.Attribute("value").Value == "1").First().Attribute("means").Value);
                        i++;
                    }
                }
                else
                {
                    resultText.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //resultTextScroll.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }

            
                //if user wants to retry
                Again.Tag = hs.path;
            }
            catch (System.FormatException)
            {
                Mess("Произошла ошибка при построении графика.");
            }
        }

        #region XML Redactor

        /// <summary>
        /// My Favourite part of this programm
        /// Here we have to get all info from PersData, edit it, then - rewrite whole document.
        /// 2 fuckin' days...
        /// </summary>
        /// <param name="name"></param>
        /// <param name="max"></param>
        /// <param name="result"></param>
        private async void xmlRedactor(string name, string max, string result, string description)
        {
            XDocument doc;
            var folder = ApplicationData.Current.LocalFolder;
            try
            {
                //try to load doc                
                doc = XDocument.Load(folder.Path + @"\PersData.xml");

                int i = Convert.ToInt32(doc.Root.FirstAttribute.Value);
                if (Convert.ToInt32(doc.Root.FirstAttribute.Value) < 8)
                {
                    i++;
                    doc.Root.FirstAttribute.Value = Convert.ToString(i);
                }

                //collect all descendants
                IEnumerable<XElement> collect = doc.Root.Elements();

                //change position for all element
                for (int j = i - 1; j > 0; j--)
                {
                    XElement el = collect.ElementAt(j);
                    XElement ex = collect.ElementAt(j - 1);
                    el.Attribute("name").Value = ex.Attribute("name").Value;
                    el.Attribute("max").Value = ex.Attribute("max").Value;
                    el.Attribute("result").Value = ex.Attribute("result").Value;

                    if (el.LastAttribute.Name == "desc")
                        el.Attribute("desc").Value = ex.Attribute("desc").Value;
                    else
                    {
                        foreach (XElement x in collect)
                        {
                            XAttribute xa = new XAttribute(XName.Get("desc"), String.Empty);
                            x.Add(xa);
                        }
                    }

                }

                //then - the last result gets the first position at the root's subtree
                XElement zero = collect.ElementAt(0);

                zero.Attribute("name").Value = name;
                zero.Attribute("max").Value = max;
                zero.Attribute("result").Value = result;
                zero.Attribute("desc").Value = description;
            }
            catch
            {
                //another way - make the new document
                doc = XDocument.Load("PersData.xml");
                XElement el = doc.Root.Elements().First();
                el.Attribute("name").Value = name;
                el.Attribute("max").Value = max;
                el.Attribute("result").Value = result;
                el.Attribute("desc").Value = description;
            }

            //save changes
            //StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.CreateFileAsync("PersData.xml", CreationCollisionOption.ReplaceExisting);

            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, doc.FirstNode.ToString());
        }

        #endregion

        private async void Mess(string a)
        {
            var dial = new MessageDialog(a);
            var command = await dial.ShowAsync();

            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void lvMain_ItemClick(object sender, ItemClickEventArgs e)
        {

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
            this.Frame.Navigate(typeof(TestPageWP), Again.Tag.ToString());
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

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void resultText_Click(object sender, RoutedEventArgs e)
        {
            if (resultTextScroll.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                resultTextScroll.Visibility = Windows.UI.Xaml.Visibility.Visible;
                descBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;

                Charty.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                tbresScroll.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                tbRes.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                resultText.Label = "Результат";
            }
            else
            {
                resultTextScroll.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                descBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                Charty.Visibility = Windows.UI.Xaml.Visibility.Visible;
                tbresScroll.Visibility = Windows.UI.Xaml.Visibility.Visible;
                tbRes.Visibility = Windows.UI.Xaml.Visibility.Visible;

                resultText.Label = "Ответы";
            }
        }

    }
}
