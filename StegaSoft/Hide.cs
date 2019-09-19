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
            //Copi du fichier
            FinalsFiles = new byte[byteLength];
            Array.Copy(File, 0, FinalsFiles,0, byteLength);
            //Ajoue de la parti avec le message caché
            ComputeHide();


        }

        //hide the data in the file
        private void ComputeHide()
        {

           int CompteurMessageIndex=0;
           StringBuilder yourBinaryString=new StringBuilder();
           for (int i = HeaderFilePosition+1 ; i < MessageSize; ++i)//will change the byte of the original file and hide new byte that contain the message
           {

                //yourBinaryString.Append(Convert.ToString(FinalsFiles[i], 2).PadLeft(8, '0'));
                //if (CompteurMessageIndex < MessageSize && i > HeaderFilePosition)
                //{
                    //string bites= bitconverter.tostring(bitconverter.getbytes(str_msgbinary[compteurmessageindex]));
                    if(FinalsFiles[i]% 2==0 )
                    {
                        if(str_MsgBinary[CompteurMessageIndex] != 0)
                        {
                            //set binary 1 string in finaslFiles
                            //byte lower = Convert.ToByte(FinalsFiles[i] & 0x00FF);
                            ++FinalsFiles[i];
                       
                        }
                        
                    }
                    else
                    {
                        if (str_MsgBinary[CompteurMessageIndex] == 0)
                        {
                            //set binary 0 string in finaslFiles
                            //byte lower = Convert.ToByte(FinalsFiles[i] & 0x00FF);
                            //FinalsFiles[i] = lower;
                            --FinalsFiles[i];
                        }
                    }
                    ++CompteurMessageIndex;
                    //yourbinarystring.append(convert.tostring(finalsfiles[i], 2).padleft(8, '0'));
                    //yourbinarystring[i] = str_msgbinary[CompteurMessageIndex];
                    //bites = bites.remove(bites.length - 1, 1) + str_msgbinary[CompteurMessageIndex];
                    //finalsfiles[i] = encoding.ascii.getbytes(bites);


               
            }



        }

        public async void deb(string test)
        {
            var dialog = new MessageDialog(test);
            await dialog.ShowAsync();
        }
        public static int GetLSB(short shortValue)
        {
            return (shortValue & 0x00FF);
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
