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

        private int MessageSize;
        private string str_MsgBinary;
        private byte[] MessageByte;

        public void MessageToBinary()//convert the message in binary
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

            StreamDecimal = await GetDeicmalStream(file, 0);
            char[] CharArray = new char[StreamDecimal.Length];

            for (int i = 0; i < StreamDecimal.Length; i++)
            {
                CharArray[i] = Convert.ToChar(StreamDecimal[i]);
           
            }
            
            FileBytebinary = Encoding.ASCII.GetBytes(CharArray);
            for (int i = 0; i < FileBytebinary.Length; i++)
            {
                FileBinary = FileBinary + Convert.ToString(FileBytebinary[i], 2).PadLeft(8, '0');
            }
            ComputeHide();

            //deb();
        }

        private StringBuilder ComputeHide()//hide the data in the file
        {
           StringBuilder FileModified= new StringBuilder(FileBinary);
           FileModified.Capacity = FileBinary.Length;
           int CompteurMessageIndex=0;
           for (int i = 154; i < FileBinary.Length; ++i)
           {
                FileModified[i] = FileBinary[i];
                if (i % 8 == 0&&CompteurMessageIndex<MessageSize) {

                    FileModified[i] = str_MsgBinary[CompteurMessageIndex];
                    ++CompteurMessageIndex;
                }
           }
            CreateFile();
            deb();
            return FileModified;
        }
        public async void CreateFile()
        {
            Windows.Storage.StorageFolder storageFolder =Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =
                await storageFolder.CreateFileAsync("BADDRAGON.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
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
        

     

    }
}
