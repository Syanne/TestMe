using Test_me_alfa.Common;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Test_me_alfa
{
    public sealed partial class TestPage : Page
    {
        /// <summary>
        /// The main method
        /// </summary>
        /// <param name="itemId">We take it from MainPage - it's file's name</param>
        private async void iAmNavigted(int itemId)
        {
            try
            {
                //Prepare(itemId);
                hs.path = new StringBuilder("Data/");
                int temp = (int)itemId / 100;
                hs.testNum = itemId;

                hs.path.Append(temp + "/" + itemId + ".xml");

                XmlReader xml = XmlReader.Create(hs.path.ToString());
                while (xml.Read())
                {
                    if (xml.IsStartElement("head"))
                    {
                        hs.numOfq = Convert.ToInt32(xml.GetAttribute("qcount"));
                        pageTitle.Text = xml.GetAttribute("name");
                        hs.minutes = Convert.ToByte(xml.GetAttribute("minute"));
                    }
                }
                if (temp == 3)
                {
                    var dial = new MessageDialog("Задания в этой категории имеют 2 особенности.\n\nПервая - ответ вам нужно дать одним словом или цифрой, иногда - фразой (если указано в названии теста).\nЕсли требуется ответить 1-м словом, то ответ должен быть в Именительном падеже и без предлога.\n\nВторая особенность - в нижнем правом углу указано время, за которое нужно дать ответ.\nЕсли не успеете, то автоматически откроется следующий вопрос.\n\nДля управления с клавиатуры используйте Tab и Enter.\n\nУдачи!");
                    dial.Commands.Add(new UICommand("Вперед!"));
                    var command = await dial.ShowAsync();

                    task.IsEnabled = true;
                    hs.taskAns = new StringBuilder();
                    TaskHandler(1);
                }
                else
                {
                    task.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    task.IsEnabled = false;
                    timeChamber.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Test(1);
                }
            }
            catch (IOException)
            {
                ErrorMessage("Ошибка загрузки данных");
            }
            catch (System.Exception)
            {
                ErrorMessage("Ошибка в кодировке данных");
            }
        }

        #region Tests from folder 3

        /// <summary>
        /// Tests from folder 3 are handling here
        /// </summary>
        /// <param name="position"></param>
        private void TaskHandler(int position)
        {
            hs.min = hs.minutes;
            hs.sec = 0;
            if (hs.position == 1) CreateTime();
            hs.timer.Start();

            XmlReader xml = XmlReader.Create(hs.path.ToString());
            while (xml.Read())
            {
                if (Convert.ToInt32(xml.GetAttribute("id")) == position)
                {
                    tbQuest.Text = position.ToString() + ") " + xml.GetAttribute("name").ToString();

                    hs.taskAns.Clear();
                    hs.taskAns.Append(xml.GetAttribute("ans").ToLower());
                }
            }
        }

        public void CreateTime()
        {
            hs.timer = new DispatcherTimer();

            hs.timer.Interval = new TimeSpan(0, 0, 0, 1);
            hs.timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, object e)
        {
            //if application is not on the screen now
            //loooooooooooooooooooooooool
            if (Window.Current.Visible == false) return;

            if (hs.min > 0 || hs.sec > 0)
            {
                if (hs.sec > 0)
                    hs.sec--;
                else
                {
                    hs.min--;
                    hs.sec = 59;
                }
                timeChamber.Text = String.Format("{0:00}:{1:00}", hs.min, hs.sec);
            }
            else TimerHandler();

        }

        public void TimerHandler()
        {
                hs.position += 1;
                if (hs.position < hs.numOfq)
                {
                    TaskHandler(hs.position);
                }
                else ResultHandler();
        }

        #endregion

        #region Tests from folders 1 and 2 are handling here

        /// <summary>
        /// if we're choosing true or false
        /// </summary>
        /// <param name="position">position of the question in xml</param>
        private void Test(int position)
        {
            if (position > hs.numOfq) ResultHandler();

                    lv.Items.Clear();

            XmlReader xml = XmlReader.Create(hs.path.ToString());
            while (xml.Read())
            {
                //look for a question
                if (Convert.ToInt32(xml.GetAttribute("number")) == position)
                {
                    tbQuest.Text = position.ToString() + ". " + xml.GetAttribute("name").ToString();

                    XmlReader inner = xml.ReadSubtree();
                    int i = 0;

                    while (inner.Read())
                    {
                        if (inner.IsStartElement("ans") == true)
                        {
                            CreateRB(i, inner.GetAttribute("means").ToString(), Convert.ToInt32(inner.GetAttribute("value")));
                            i++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create RadioButtons
        /// </summary>
        /// <param name="number">number of rb</param>
        private void CreateRB(int number, string ans, int value)
        {
            RadioButton rb = new RadioButton();
            TextBlock tb = new TextBlock();
            tb.Text = ans;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontSize = 18;
            rb.Content = tb;
            rb.TabIndex = number + 1;
            rb.Tag = value;

            lv.Items.Add(rb);
            lv.IsSynchronizedWithCurrentItem.GetValueOrDefault(true);
        }

        #endregion

        /// <summary>
        /// Calls the error message
        /// </summary>
        private async void ErrorMessage(string error)
        {
            var dial = new MessageDialog("К сожалению, в работе программы возникла ошибка. Просим прощение за неудобства", error);
            var command = await dial.ShowAsync();
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// fuckin' result :)
        /// </summary>
        public void ResultHandler()
        {
            try
            {
                if (task.IsEnabled == true)
                {
                    hs.timer.Stop();
                    hs.timer.Tick -= timer_Tick;
                }
                this.Frame.Navigate(typeof(GraphPage), hs);
            }
            catch (System.FormatException)
            {
                ErrorMessage("Ошибка при подсчете результата");
            }
        }

    }

}
