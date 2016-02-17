using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace TestMe
{
    public sealed partial class MainPage : Page
    {
        SampleDataSource sds;

        /// <summary>
        /// PopUp
        /// </summary>
        /// <param name="lol"> String for Message</param>
        private async void ShowPop(string lol)
        {
            var dial = new MessageDialog(lol);
            var command = await dial.ShowAsync();
        }

        private async void AboutMessage()
        {
            var dial = new MessageDialog("спасибо, что скачали приложение!\n\nприложению очень помогут ваши отзывы и оценки.\n\nесли вы обнаружили проблемы или возникли вопросы, пишите на ящик: syanne.red@gmail.com");
            dial.Commands.Add(new UICommand("оставить отзыв",
               new UICommandInvokedHandler((args) =>
               {
                   GottStore();
               })));
            dial.Commands.Add(new UICommand("служба поддержки",
                new UICommandInvokedHandler((args) =>
                {
                    Message();
                })));
            dial.Commands.Add(new UICommand("назад"));

            var command = await dial.ShowAsync();
        }
    }
}
