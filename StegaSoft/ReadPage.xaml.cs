using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.System;
using Windows.ApplicationModel.DataTransfer;

namespace StegaSoft
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReadPage : Page
    {
        //variable pour le decryptage
        Read imageDescript = new Read();
        string MessageDecoder;

        public ReadPage()
        {
            this.InitializeComponent();

            if (Result.Text != "...")
            {
                ButtonGo.IsEnabled = false;
            }
            else
            {
                ButtonGo.IsEnabled = true;
            }
        }

        private async void SelectImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            openPicker.FileTypeFilter.Add(".bmp");
            //openPicker.FileTypeFilter.Add(".png");
            

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                //affecte l'image pour l'affichage
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var imageForDisplay = new BitmapImage();
                imageForDisplay.SetSource(stream);
                ImagePreview.Source = imageForDisplay;
                ImagePreview.Height = 150;
                //affecte l'image pour Read
                imageDescript.file = file;

            }
        }


        private async void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (imageDescript.file != null && !string.IsNullOrWhiteSpace(ParameterMessageToFindLenght.Text)  && !string.IsNullOrWhiteSpace(ParameterMessageToFindStart.Text) && !string.IsNullOrWhiteSpace(ParameterMessageSkippingBytes.Text))
            {
                imageDescript.ClearMessage();
                
                if (ParameterMessageToFindLenght.Text.Length==0)//verify if there is an input
                {
                    imageDescript.deb("You should enter a size");
                    imageDescript.LenghtMessageToShow = 500;//placeholder input
                }
                else
                {
                    imageDescript.LenghtMessageToShow = Convert.ToInt32(ParameterMessageToFindLenght.Text);
                }
                imageDescript.StartAtPosition = Int32.Parse(ParameterMessageToFindStart.Text);
                MessageDecoder = await imageDescript.OperationRead();
                Result.Text=  MessageDecoder;
            }

        }

        private void CopyResult_Click(object sender, RoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(Result.Text);
            Clipboard.SetContent(dataPackage);
        }

        // force number in text box
        private void TextBox_ParameterMessageToFindLenght(TextBox sender,
                                          TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void TextBox_ParameterMessageToFindStart(TextBox sender,
                                  TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void TextBox_ParameterMessageSkippingBytes(TextBox sender,
                                  TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }


    }
}
