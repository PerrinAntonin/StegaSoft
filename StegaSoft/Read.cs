using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StegaSoft;

using Windows.Storage;

using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace StegaSoft
{
    public class Read : FileDecimal
    {
        public int StartAtPosition { get; set; } = 0;

        public int LenghtMessageToShow { get; set; }
        private string MessageDecoder;

        public int NBytesOffset { set; get; }

        public async Task<string> OperationRead()
        {
            StreamDecimal = await GetDeicmalStream(file, 154);
            return GetHideMessage(StreamDecimal);
        }
        public async Task<byte[]> OperationGetHiddenFile()
        {
            StreamDecimal = await GetDeicmalStream(file, 154);
            return GetHideFile(StreamDecimal);
        }



        private string GetHideMessage(int[] TabDecs)
        {
            int RangBit = 7;
            char Voctet = (char)0;

            for (int i = StartAtPosition; i != TabDecs.Length; i++)
            {
                if (i % NBytesOffset == 0)
                {
                    Voctet += (char)(Math.Abs(TabDecs[i] % 2) * Math.Pow(2, RangBit));


                    RangBit--;
                    if (RangBit < 0)
                    {
                        if (i < LenghtMessageToShow)
                        {
                            MessageDecoder += Voctet;

                        }
                        Voctet = (char)0;
                        RangBit = 7;
                    }
                }

            }
            return MessageDecoder;
        }

        public void ClearMessage()//fuction used to clear the message 
        {
            MessageDecoder = string.Empty;
        }
        private byte[] GetHideFile(int[] TabDecs)
        {
            int RangBit = 7;
            List<byte> Voctet = new List<byte>();
            string binary= string.Empty;

            for (int i = StartAtPosition; i != TabDecs.Length; i++)
            {
                if (i % NBytesOffset == 0)
                {
                    binary += (Math.Abs(TabDecs[i] % 2) * Math.Pow(2, RangBit)).ToString();


                    RangBit--;
                    if (RangBit < 0)
                    {
                        
                        Voctet.Add(Convert.ToByte(binary));
                        binary = string.Empty;
                        RangBit = 7;
                    }
                }

            }


            return Voctet.ToArray();


        }
    }
}