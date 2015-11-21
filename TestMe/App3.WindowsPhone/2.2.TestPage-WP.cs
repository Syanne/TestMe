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
    public sealed partial class TestPageWP : Page
    {
        /// <summary>
        /// The main method
        /// </summary>
        /// <param name="itemId">We take it from MainPage - it's file's name</param>
        private void iAmNavigted(string path)
        {
            try
            {
                //Prepare(itemId);
                hs.path = path;

                XmlReader xml = XmlReader.Create(hs.path);
                while (xml.Read())
                {
                    if (xml.IsStartElement("head"))
                    {
                        hs.numOfq = Convert.ToInt32(xml.GetAttribute("qcount"));
                        pageTitle.Text = xml.GetAttribute("name");
                        hs.minutes = Convert.ToByte(xml.GetAttribute("minute"));
                    }
                }

                //start test
                hs.position = 1;
                    Test();

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


        #region Tests from folders 1 and 2 are handling here

        /// <summary>
        /// if we're choosing true or false
        /// </summary>
        /// <param name="position">position of the question in xml</param>
        private void Test()
        {
            //if current = max
            if (hs.position > hs.numOfq) ResultHandler();

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
            tb.FontSize = 14;

            rb.Content = tb;
            rb.TabIndex = number + 1;
            rb.Tag = value;

            lv.Items.Add(rb);
            lv.IsSynchronizedWithCurrentItem.GetValueOrDefault(true);
            if (number == 0) rb.IsChecked = true;
        }

        public void ResultHandler()
        {
            try
            {
                this.Frame.Navigate(typeof(ResultWP), hs);
            }
            catch (System.FormatException)
            {
                ErrorMessage("Ошибка при подсчете результата");
            }
        }

        #endregion

        /// <summary>
        /// Calls the error message
        /// </summary>
        private async void ErrorMessage(string error)
        {
            var dial = new MessageDialog("К сожалению, в работе программы возникла ошибка. Просим прощение за неудобства", error);
            var command = await dial.ShowAsync();
            this.Frame.Navigate(typeof(BlankPage1));
        }


    }

}
