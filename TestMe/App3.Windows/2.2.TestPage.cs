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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Test_me_alfa
{
    public sealed partial class TestPage : Page
    {
        /// <summary>
        /// The main method
        /// </summary>
        /// <param name="itemId">We take it from MainPage - it's file's name</param>
        private void iAmNavigted(string path)
        {
            try
            {
                //a path to the test-file 
                hs.path = path;
                XmlReader xml = XmlReader.Create(path);

                //read header
                while (xml.Read())
                {
                    if (xml.IsStartElement("head"))
                    {
                        hs.numOfq = Convert.ToInt32(xml.GetAttribute("qcount"));
                        pageTitle.Text = xml.GetAttribute("name");
                        hs.minutes = Convert.ToByte(xml.GetAttribute("minute"));
                    }
                }
                    hs.position = 1;
                //then - chose method
                if (path[7] == '3')
                {
                    task.IsEnabled = true;
                    TaskHandler();
                }
                else
                {
                    task.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    task.IsEnabled = false;
                    timeChamber.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Test();
                }
            }
            catch (IOException e)
            {
                ErrorMessage(e.Message);
            }
            catch (System.Exception e)
            {
                ErrorMessage(e.Message);
            }
        }

        #region Tests from folder 3

        /// <summary>
        /// Tests from folder 3 are handling here
        /// </summary>
        /// <param name="position"></param>
        private void TaskHandler()
        {
            //current question number
            quesChamber.Text = String.Format("{0}/{1}", hs.position, hs.numOfq);

            task.Focus(FocusState.Keyboard);
            //timer
            hs.min = hs.minutes;
            hs.sec = 0;
            if (hs.position == 1) CreateTime();
            hs.timer.Start();

            //load tasks from xml-file
            XmlReader xml = XmlReader.Create(hs.path.ToString());
            while (xml.Read())
            {
                if (Convert.ToInt32(xml.GetAttribute("id")) == hs.position)
                {
                    tbQuest.Text = hs.position.ToString() + ") " + xml.GetAttribute("name").ToString();

                    task.Tag = xml.GetAttribute("ans").ToLower();
                }
            }
        }

        /// <summary>
        /// Create timer, if current test from the third group
        /// </summary>
        public void CreateTime()
        {
            hs.timer = new DispatcherTimer();

            hs.timer.Interval = new TimeSpan(0, 0, 0, 1);
            hs.timer.Tick += timer_Tick;
        }

        /// <summary>
        /// Event Handler for timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, object e)
        {
            if(hs.min == 0 && hs.sec <=30)
                timeChamber.Foreground = new SolidColorBrush(Colors.Red);
            else timeChamber.Foreground = new SolidColorBrush(Colors.Black);

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

        /// <summary>
        /// if the time is out
        /// </summary>
        public void TimerHandler()
        {
                hs.position += 1;
                if (hs.position < hs.numOfq)
                {
                    TaskHandler();
                }
                else ResultHandler();
        }

        #endregion

        #region Tests from folders 1 and 2 handles here

        /// <summary>
        /// if we're choosing true or false
        /// </summary>
        private void Test()
        {
            //current question
            quesChamber.Text = String.Format("{0}/{1}", hs.position, hs.numOfq);
            lv.Items.Clear();

            //read the question
            XmlReader xml = XmlReader.Create(hs.path.ToString());
            while (xml.Read())
            {
                //look for a question
                if (Convert.ToInt32(xml.GetAttribute("number")) == hs.position)
                {
                    tbQuest.Text = hs.position.ToString() + ". " + xml.GetAttribute("name").ToString();

                    //answers
                    XmlReader inner = xml.ReadSubtree();
                    int i = 1;

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
            tb.FontSize = FontSizeOnPage;
            tb.Margin = new Thickness(0, (19 - FontSizeOnPage), 0, 0);
            tb.Padding = new Thickness(5, 0, 0, 10);

            rb.Content = tb;
            rb.TabIndex = number;
            rb.Tag = value;
            rb.Margin = new Thickness(5, 10, 5, 10);

            lv.Items.Add(rb);
            lv.IsSynchronizedWithCurrentItem.GetValueOrDefault(true);
            if (number == 1) rb.IsChecked = true;
        }

        #endregion

        /// <summary>
        /// Calls the error message
        /// </summary>
        private async void ErrorMessage(string error)
        {
            var dial = new MessageDialog("К сожалению, в работе программы возникла ошибка.", error);
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
