using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace StegaSoft
{
    public class Hide : FileDecimal
    {
        public string MessageToHide { get; set; }

        private string FileBinary;
        private byte[] FileBytebinary;

        public string FinalsFile;

        private int MessageSize;
        private string str_MsgBinary;
        private byte[] MessageByte;

        public async void MessageToAscii()//convert the message in binary
        {
           
            MessageByte = Encoding.ASCII.GetBytes(MessageToHide);
       
            for (int i = 0; i < MessageByte.Length; i++)
            {
                str_MsgBinary = str_MsgBinary + Convert.ToString(MessageByte[i], 2).PadLeft(8, '0');
            }
            MessageSize = str_MsgBinary.Length;
            FileToBinary();
        }

        public async void FileToBinary()//convert the file (image) in binary 
        {
            //a changer
            StreamDecimal = await GetDeicmalStream(file, 0);

            for (int i = 0; i < StreamDecimal.Length; i++)
            {
                string binary = Convert.ToString(StreamDecimal[i], 2);
                FileBinary += binary;
            }
             
             ComputeHide();

            //deb();
        }

        private void ComputeHide()//hide the data in the file
        {
           StringBuilder FileModified= new StringBuilder(FileBinary);

           FileModified.Capacity = FileBinary.Length;
           int CompteurMessageIndex=0;
            //a changer
           for (int i = 0; i < FileBinary.Length; ++i)
           {
                FileModified[i] = FileBinary[i];
                if (i % 7 == 0&&CompteurMessageIndex<MessageSize &&i>154) {

                    FileModified[i] = str_MsgBinary[CompteurMessageIndex];
                    ++CompteurMessageIndex;
                }
           }

            //CreateFile(FileModified);

            FinalsFile = ConvertBitsToChar(FileModified);
            //deb();

        }
        public async void CreateFile(StringBuilder message)
        {

            Windows.Storage.StorageFolder storageFolder =Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =
                await storageFolder.CreateFileAsync("BADDRAGON.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //WriteFile(sampleFile, ConvertBitsToChar(message));
        }
        public async void WriteFile(Windows.Storage.StorageFile sampleFile, string message)
        {
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, message);
        }
        public async void deb()
        {


            var dialog = new MessageDialog(FileBinary);
            await dialog.ShowAsync();
        }


        public async void OperationHide()
        {
            StreamDecimal = await GetDeicmalStream(file, 154);
            
            
        }

        public string ConvertBitsToChar(StringBuilder message)
        {
            string messageInBits= message.ToString();


            string result = "";
            while (message.Length > 0)
            {
                var first8 = messageInBits.Substring(0, 8);
                messageInBits = messageInBits.Substring(8);
                var number = Convert.ToInt32(first8, 2);
                result += (char)number;
            }
            return result;
        }




    }
}
