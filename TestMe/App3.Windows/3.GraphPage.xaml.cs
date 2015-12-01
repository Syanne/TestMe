using Test_me_alfa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage;
using System.Xml.Linq;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// Документацию по шаблону элемента "Основная страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=234237

namespace Test_me_alfa
{
    /// <summary>
    /// Основная страница, которая обеспечивает характеристики, являющимися общими для большинства приложений.
    /// </summary>
    public sealed partial class GraphPage : Page
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

        /// <summary>
        /// NavigationHelper используется на каждой странице для облегчения навигации и 
        /// управление жизненным циклом процесса
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public GraphPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);

            if (Window.Current.Bounds.Width <= Window.Current.Bounds.Height)
                SizeCorrector(Window.Current.Bounds.Width, Window.Current.Bounds.Width);
            else
                SizeCorrector(Window.Current.Bounds.Height, Window.Current.Bounds.Width);
        }           

        private void CreateGraph(TestInfo testInfo)
        {
            try
            {
                //gets info from Tag and opens the file
                List<Graph> gList = new List<Graph>();

                gList.Add(new Graph() { Title = "Ваш результат", Result = Convert.ToInt32(testInfo.result) });

                string descXML = "";
                string nameXML = testInfo.xDoc.Element("head").Attribute("name").Value;
                string maxXML = testInfo.xDoc.Element("head").Attribute("max").Value;

                gList.Add(new Graph()
                {
                    Title = "Осталось",
                    Result = Convert.ToInt32(maxXML) - Convert.ToInt32(testInfo.result)
                });

                var results = testInfo.xDoc.Root.Element("end").Elements();
                foreach (XElement elem in results)
                {
                    if((int)testInfo.result >= Convert.ToInt32(elem.Attribute("min").Value))
                    {
                        descXML = elem.Value;
                        tbRes.Text = testInfo.result.ToString() + " балл(а, ов). \n\n" + descXML;
                        break;
                    }
                }

                //put info into the Chat
                (Charty.Series[0] as PieSeries).ItemsSource = gList;

                //info about result goes to the result-file
                xmlRedactor(nameXML, maxXML, testInfo.result.ToString(), descXML);

                //2nd and 3rd groups
                descBlock.Text += "ОТВЕТЫ: \n\n\n";
                int i = 1; 
                if (testInfo.path[7] == '3')
                    foreach (var elem in testInfo.xDoc.Root.Elements("question"))
                    {
                        descBlock.Text += String.Format("{0}) {1}\nПРАВИЛЬНЫЙ ОТВЕТ: {2}\nВЫ ОТВЕТИЛИ: {3}\n\n",
                            i, elem.Attribute("name").Value, elem.Attribute("ans").Value, testInfo.myAnswers[i - 1]);
                        i++;
                    }
                else if (testInfo.path[7] == '2')
                    foreach (var elem in testInfo.xDoc.Root.Elements("question"))
                    {
                        descBlock.Text += String.Format("{0}) {1}\nПРАВИЛЬНЫЙ ОТВЕТ: {2}\nВЫ ОТВЕТИЛИ: {3}\n\n", i, elem.Attribute("name").Value,
                            elem.Elements().Where(el => el.Attribute("value").Value == "1").First().Attribute("means").Value, 
                            testInfo.myAnswers[i - 1]);
                        i++;
                    }
                else ChangeTextBtn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
      
                //if user wants to retry
                Again.Tag = testInfo.path;
            }
            catch (System.FormatException)
            {
                Mess("Произошла ошибка при построении графика.");
            }
        }


        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (Charty.IsEnabled)
            {                
                Charty.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                Charty.IsEnabled = false;
                Scroller.Visibility = Windows.UI.Xaml.Visibility.Visible;
                descBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ChangeTextBtn.Content = "График";
            }
            else
            {
                Charty.Visibility = Windows.UI.Xaml.Visibility.Visible;
                Charty.IsEnabled = true;
                descBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                Scroller.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ChangeTextBtn.Content = "Правильные ответы";
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
            TestInfo hs = (TestInfo)e.Parameter;

            CreateGraph(hs);
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
            tbRes.FontSize = (Window.Current.Bounds.Width < 1000) ? 20 : 22;

            Scroller.Width = Width / 2;
            descBlock.Width = Width / 2 - 50;
            ChangeTextBtn.Margin = new Thickness(Width / 2, 0, 0, 100);

            if (Width > 800)
            {
                tbRes.Width = Width / 2.5;
                Charty.Visibility = Visibility.Visible;
                Charty.Height = Height - 140 * 2;
                Charty.Width = Width / 2;
            }
            else
            {
                tbRes.Width = Width - 100;
                Charty.Width = Width / 2 - 100;
                Charty.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

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

                        if(el.LastAttribute.Name == "desc")
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

            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Again Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TestPage), Again.Tag.ToString());
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
