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

        public void MessageToBinary(string Msg)//convert the message in binary
        {
           
            MessageByte = Encoding.ASCII.GetBytes(Msg);
       
            for (int i = 0; i < MessageByte.Length; i++)
            {
                str_MsgBinary = str_MsgBinary + Convert.ToString(MessageByte[i], 2).PadLeft(8, '0');
            }
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

         
            deb();
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
