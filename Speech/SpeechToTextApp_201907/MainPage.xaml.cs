using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;
using Windows.UI.Core;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace SpeechToTextApp_201907
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await SpeechRecognizeAsync();
        }


        private async Task SpeechRecognizeAsync()
        {
            var config = SpeechConfig.FromSubscription("YOUR_SPEECH_API_KEY", "southeastasia"); // Replace with your Speech API Key and location

            config.SpeechRecognitionLanguage = "ja-jp";
            var recognizer = new SpeechRecognizer(config);

            recognizer.Recognizing += Recognizer_Recognizing;
            recognizer.Recognized += Recognizer_Recognized;

            this.RecognizedTextBox.Text = "Started speech recognition...\r\n ";
            await recognizer.StartContinuousRecognitionAsync();
        }

        private async void Recognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
        {
            await this.MicOutput(e.Result.Text, false);
        }

        private async void Recognizer_Recognizing(object sender, SpeechRecognitionEventArgs e)
        {
            await this.MicOutput(e.Result.Text, true);
        }

        private async Task MicOutput(string text, bool isNotComplete)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var textlines = RecognizedTextBox.Text.Split("\r\n");
                if (isNotComplete == false) textlines[textlines.Length - 1] = text + "\r\n ";
                else textlines[textlines.Length - 1] = text;

                this.RecognizedTextBox.Text = string.Join("\r\n", textlines);
            });
        }


    }
}
