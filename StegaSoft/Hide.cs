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

        public string FinalsFiles;
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
            //a changer
            StreamDecimal = await GetDeicmalStream(file, 0);
            byte[] test = await GetBytes(file);

            //convertir la portion du fichier ou devra etre caché le message
            for (int i = HeaderFilePosition; i < endMessagePosition; i++)
            {
                string binary = Convert.ToString(StreamDecimal[i], 2).PadLeft(8, '0');
                FileBinary += binary;
            } 
            
            ComputeHide();
            FinalsFiles += Encoding.ASCII.GetString(test,0,154);

            byte[] bytes = Encoding.ASCII.GetBytes(PartFileModifed);
            FinalsFiles += Encoding.ASCII.GetString(bytes, 155,100);
            /*
            for (int i = 154; i < StreamDecimal.Length; i++)
            {

                if ( i < endMessagePosition)
                {
                    FinalsFiles += PartFileModifed[i- HeaderFilePosition];
                }
                else
                {
                    FinalsFiles += (char)StreamDecimal[i];
                    //FinalsFiles += Encoding.ASCII.GetString(test);
                }

            }
            */
            
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
