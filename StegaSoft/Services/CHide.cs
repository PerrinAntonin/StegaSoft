using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegaSoft.Services
{
    class CHide : CFileDecimal
    {
        public string MessageToHide { get; set; }

        public int HeaderFilePosition = 0;
        public int endMessagePosition;

        public byte[] FinalsFiles;


        private int MessageSize;
        private string str_MsgBinary;
        public byte[] MessageByte;

        public int StartAtPosition { set; get; }
        public int NBytesOffset { set; get; }




        public void MessageToAscii(bool MessageToHideBool)//convert the message in binary
        {
            if (MessageToHideBool == true)
            {
                MessageByte = GetBytes(MessageToHide);
            }


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
            endMessagePosition = HeaderFilePosition + (MessageSize * NBytesOffset) + StartAtPosition;

            byte[] File = await GetBytes(file);
            int byteLength = System.Buffer.ByteLength(File);
            //Copy the File
            FinalsFiles = new byte[byteLength];
            Array.Copy(File, 0, FinalsFiles, 0, byteLength);
            //Add hidden part
            ComputeHide();
            deb((MessageSize * NBytesOffset + StartAtPosition).ToString());
        }

        //hide the data in the file
        private void ComputeHide()
        {

            int CompteurMessageIndex = 0;

            for (int i = HeaderFilePosition + StartAtPosition; i < endMessagePosition; ++i)//will change the byte of the original file and hide new byte that contain the message
            {
                if ((i - HeaderFilePosition) % NBytesOffset == 0)
                {
                    if (FinalsFiles[i] % 2 == 0)
                    {
                        if (str_MsgBinary[CompteurMessageIndex].ToString() == "1")
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

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
