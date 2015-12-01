using System;
using System.IO;
using System.Xml;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Xml.Linq;

namespace Test_me_alfa
{
    public sealed partial class TestPage : Page
    {
        /// <summary>
        /// The main method
        /// </summary>
        /// <param name="itemId">We take it from MainPage - it's file's name</param>
        private void IAmNavigted(string filePath)
        {
            if (filePath[7] == '3')
            {
                testInfo = new TestInfo(filePath, true);
                task.IsEnabled = true;
                TestWithTimer();
            }
            else
            {
                testInfo = new TestInfo(filePath, false);
                task.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                task.IsEnabled = false;
                Test();
            }

            pageTitle.Text = testInfo.xDoc.Root.Attribute("name").Value;
        }

        #region Tests from folder 3

        /// <summary>
        /// Tests from folder 3 are handling here
        /// </summary>
        /// <param name="position"></param>
        private void TestWithTimer()
        {
            //current question number
            quesChamber.Text = String.Format("{0}/{1}", testInfo.position, testInfo.qCount);

            task.Focus(FocusState.Keyboard);

            //timer
            testInfo.min = testInfo.minutes;
            testInfo.sec = 0;
            if (testInfo.position == 1) 
                testInfo.timer.Tick += timer_Tick;
            testInfo.timer.Start();

            var element = testInfo.xDoc.Root.Elements("question").
                ElementAt(testInfo.position - 1);

            tbQuest.Text = testInfo.position.ToString() + ". " + element.Attribute("name").Value;
            task.Tag = element.Attribute("ans").Value;
        }
        
        /// <summary>
        /// Event Handler for timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, object e)
        {
            if (testInfo.min == 0 && testInfo.sec <= 30)
                timeChamber.Foreground = new SolidColorBrush(Colors.Red);
            else timeChamber.Foreground = new SolidColorBrush(Colors.Black);

            if (testInfo.min > 0 || testInfo.sec > 0)
            {
                if (testInfo.sec > 0)
                    testInfo.sec--;
                else
                {
                    testInfo.min--;
                    testInfo.sec = 59;
                }
                timeChamber.Text = String.Format("{0:00}:{1:00}", testInfo.min, testInfo.sec);
            }
            else TimerHandler();

        }

        /// <summary>
        /// if the time is out
        /// </summary>
        public void TimerHandler()
        {
                testInfo.position += 1;
                if (testInfo.position < testInfo.qCount)
                {
                    TestWithTimer();
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
            quesChamber.Text = String.Format("{0}/{1}", testInfo.position, testInfo.qCount);
            lv.Items.Clear();

            //get a question
            var element = testInfo.xDoc.Root.Elements("question").
                ElementAt(testInfo.position - 1);

            tbQuest.Text = testInfo.position.ToString() + ". " + element.Attribute("name").Value;
            int i = 1;
            foreach (var ans in element.Elements("ans"))
            {
                CreateRB(i, ans.Attribute("means").Value, Convert.ToInt32(ans.Attribute("value").Value));
                ++i;
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
                    testInfo.timer.Stop();
                    testInfo.timer.Tick -= timer_Tick;
                }
                this.Frame.Navigate(typeof(GraphPage), testInfo);
            }
            catch (System.FormatException)
            {
                ErrorMessage("Ошибка при подсчете результата");
            }
        }
    }

}
