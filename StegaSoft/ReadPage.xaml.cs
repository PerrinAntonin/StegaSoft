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
using System.Text;

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
        public bool GetFileBool =false;

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
                imageDescript.NBytesOffset = Convert.ToInt32(ParameterMessageSkippingBytes.Text);
                if (ParameterMessageToFindLenght.Text.Length==0)//verify if there is an input
                {
                    imageDescript.deb("You should enter a size");
                    imageDescript.LenghtMessageToShow = 500;//placeholder input
                }
                else
                {
                    imageDescript.LenghtMessageToShow = Convert.ToInt32(ParameterMessageToFindLenght.Text);
                }
                if (ParameterMessageToFindStart.Text.Length != 0)
                {
                    imageDescript.StartAtPosition = Int32.Parse(ParameterMessageToFindStart.Text);
                }
                else
                {
                    imageDescript.StartAtPosition = 0;
                }
                if (GetFileBool == true)
                {

                    byte[] newFile = await imageDescript.OperationGetHiddenFile();
                    //Sytem de Sauvegarde
                    var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                    savePicker.SuggestedStartLocation =
                        Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                    // Dropdown of file types the user can save the file as
                    savePicker.FileTypeChoices.Add("tageul", new List<string>() { ".txt" });
                    // Default file name if the user does not type one in or select a file to replace
                    savePicker.SuggestedFileName = "Newpicturefromanother";


                    Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

                    if (file != null)
                    {

                        await FileIO.WriteBytesAsync(file, newFile);

                        Windows.Storage.Provider.FileUpdateStatus status =
                            await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                        if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                        {
                            this.ReponseForSaveFile.Text = "File " + file.Name + " was saved.";
                        }
                        else
                        {
                            this.ReponseForSaveFile.Text = "File " + file.Name + " couldn't be saved.";
                        }
                    }
                    else
                    {
                        this.ReponseForSaveFile.Text = "Operation cancelled.";
                    }
                }
                else
                {
                    MessageDecoder = await imageDescript.OperationRead();
                    Result.Text = MessageDecoder;
                }
                
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

        private void ToggleSwitchGetFile_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    //truc1.IsActive = true;
                    GetFileBool = true;
                }
                else
                {
                    //truc2.IsActive = false;
                    GetFileBool = false;
                }
            }
        }


    }
}
