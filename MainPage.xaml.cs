using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Ocr;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Optica
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            textbox.Visibility = Visibility.Collapsed;
        }
        StorageFile file;
        CameraCaptureUI captureUI = new CameraCaptureUI();
        public async void button_Click(object sender, RoutedEventArgs e)
        {
            image.Visibility = Visibility.Visible;
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            file = await picker.PickSingleFileAsync();
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                image.Source = bitmapImage;
            }
        }



        public async void button_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            image.Visibility = Visibility.Visible;
            CameraCaptureUI dialog = new CameraCaptureUI();
            Size aspectRatio = new Size(16, 9);
            dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

            file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                image.Source = bitmapImage;
            }


        }
        string link;
        public async void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            Language ocrLanguage = new Language("en");
            OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(ocrLanguage);

            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                OcrResult x = await ocrEngine.RecognizeAsync(softwareBitmap);
                var dialog = new MessageDialog(x.Text, "Extracted Text");
                link = x.Text;
                image.Visibility = Visibility.Collapsed;
                textbox.Visibility = Visibility.Visible;
                textbox.Text = link;
            }

        }
        public async void button_Click111(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(textbox.Text));
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            textbox.Text = link;
        }


    }
}
