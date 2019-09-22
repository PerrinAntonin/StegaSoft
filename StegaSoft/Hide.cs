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

        public byte[] FinalsFiles ;
  

        private int MessageSize;
        private string str_MsgBinary;
        private byte[] MessageByte;

        public int StartAtPosition { set; get; }




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
            endMessagePosition = HeaderFilePosition + MessageSize;

            byte[] File = await GetBytes(file);
            int byteLength = System.Buffer.ByteLength(File);
            //Copy the File
            FinalsFiles = new byte[byteLength];
            Array.Copy(File, 0, FinalsFiles,0, byteLength);
            //Add hidden part
            ComputeHide();
        }

        //hide the data in the file
        private void ComputeHide()
        {

           int CompteurMessageIndex=0;

           for (int i = HeaderFilePosition+StartAtPosition ; i < endMessagePosition; ++i)//will change the byte of the original file and hide new byte that contain the message
           {
                if(FinalsFiles[i]% 2==0 )
                {
                    if(str_MsgBinary[CompteurMessageIndex].ToString() == "1")
                    {
                        ++FinalsFiles[i];
                    } 
                }
                else
                {
                    if (str_MsgBinary[CompteurMessageIndex].ToString() == "0")
                    {
                        --FinalsFiles[i];
                    }
                }
                ++CompteurMessageIndex;
            }
        }

      
    }
}