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

        public int HeaderFilePosition = 0;
        public int endMessagePosition;
        private string FileBinary;
        private byte[] FileBytebinary;

        public byte[] FinalsFiles ;
        public string PartFileModifed;
  

        private int MessageSize;
        private string str_MsgBinary;
        private byte[] MessageByte;

        public void MessageToAscii()//convert the message in binary
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
            HeaderFilePosition = 154;
            endMessagePosition = HeaderFilePosition + MessageToHide.Length;

            byte[] File = await GetBytes(file);
            int byteLength = System.Buffer.ByteLength(File);
            //Ajoue du header
            FinalsFiles = new byte[byteLength];
            Array.Copy(File, 0, FinalsFiles,0, byteLength);
            //Ajoue de la parti avec le message caché

            ComputeHide();

            //Ajoue de la fin
            //Array.Copy(File, endMessagePosition, FinalsFiles, endMessagePosition, 154);

        }

        private void ComputeHide()//hide the data in the file
        {
           StringBuilder FileModified= new StringBuilder(FileBinary);

           FileModified.Capacity = FileBinary.Length;
           int CompteurMessageIndex=0;
            //a changer peut etre
           for (int i = 0; i < FileBinary.Length; ++i)
           {
                FileModified[i] = FileBinary[i];
                if (i % 7 == 0&&CompteurMessageIndex<MessageSize &&i>154) {

                    FileModified[i] = str_MsgBinary[CompteurMessageIndex];
                    ++CompteurMessageIndex;
                }
           }

            //CreateFile(FileModified);
            // a tester
            //PartFileModifed = BinaryToString(FileModified.ToString());
            PartFileModifed = FileModified.ToString();
            //deb();

        }

        public async void deb()
        {
            var dialog = new MessageDialog(file.ToString());
            await dialog.ShowAsync();
        }


        public async void OperationHide()
        {
            StreamDecimal = await GetDeicmalStream(file, 154);
        }


        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        public async void test()
        {
            StreamDecimal = await GetDeicmalStream(file, 154);
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
        }
        
    }
}
