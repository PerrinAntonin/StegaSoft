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
using System.Text;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StegaSoft
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WritePage : Page
    {
        Hide fileEncrypted = new Hide();

        private StorageFile fileToHide;

        private bool MessageToHideBool = true;
        public Windows.Storage.Streams.UnicodeEncoding UnicodeEncoding { get; private set; }
        public Windows.Storage.Streams.UnicodeEncoding Utf16LE { get; private set; }
        public Windows.Storage.Streams.UnicodeEncoding Utf32 { get; private set; }

        public WritePage()
        {
            this.InitializeComponent();
            
        }
    
        private async void SelectImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            openPicker.FileTypeFilter.Add(".bmp");
            

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {

                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var imageForDisplay = new BitmapImage();
                imageForDisplay.SetSource(stream);
                ImagePreview.Source = imageForDisplay;
                ImagePreview.Height = 150;

                fileEncrypted.file = file;
            }
        }
        private async void SelectFiletoHide_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            fileToHide = await openPicker.PickSingleFileAsync();

            if (fileToHide != null)
            {

                var stream = await fileToHide.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var imageForDisplay = new BitmapImage();
                imageForDisplay.SetSource(stream);
                ImagePreview.Source = imageForDisplay;
                ImagePreview.Height = 150;

            }
        }

        private async void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            //Sytem de Sauvegarde
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Bitmap", new List<string>() { ".bmp" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "VacationPictures";


            if (fileEncrypted.file != null && !string.IsNullOrWhiteSpace(messageToHide.Text) && !string.IsNullOrWhiteSpace(ParameterMessageToFindStart.Text) && !string.IsNullOrWhiteSpace(ParameterMessageSkippingBytes.Text))
            {                
                fileEncrypted.NBytesOffset = Int32.Parse(ParameterMessageSkippingBytes.Text);
                if (ParameterMessageToFindStart.Text.Length != 0)
                {
                    fileEncrypted.StartAtPosition = Int32.Parse(ParameterMessageToFindStart.Text);
                }
                else
                {
                    fileEncrypted.StartAtPosition = 0;
                }
                fileEncrypted.MessageToAscii();
                Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

                if (MessageToHideBool)
                {
                    fileEncrypted.MessageToHide = messageToHide.Text;
                }
                else
                {
                    IBuffer bufferToHide = await FileIO.ReadBufferAsync(fileToHide);

                    byte[] fileToHideBytes = bufferToHide.ToArray();
                    fileEncrypted.MessageToHide = fileToHideBytes.ToString();
                }

                if (file != null)
                {
                   
                    await FileIO.WriteBytesAsync(file, fileEncrypted.FinalsFiles);

                    Windows.Storage.Provider.FileUpdateStatus status =
                        await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                    {
                        this.test.Text = "File " + file.Name + " was saved.";
                    }
                    else
                    {
                        this.test.Text = "File " + file.Name + " couldn't be saved.";
                    }
                }
                else
                {
                    this.test.Text = "Operation cancelled.";
                }



            }

        }

        // force number in text box

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

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    //truc1.IsActive = true;
                    MessageToHideBool = false;
                }
                else
                {
                    //truc2.IsActive = false;
                    MessageToHideBool = true;
                }
            }
        }
    }
}

