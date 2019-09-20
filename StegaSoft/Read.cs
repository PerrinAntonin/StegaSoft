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
        public int LenghtMessageToShow { get; set; }
        private string MessageDecoder;
       

        public async Task<string>  OperationRead()
        {
            StreamDecimal = await GetDeicmalStream(file, 154);
            return GetHideMessage(StreamDecimal); 
        }


        private string GetHideMessage(int[] TabDecs)
        {
            int RangBit = 7;
            char Voctet = (char)0;
            int i = 0;
            while (i != TabDecs.Length)
            {
                Voctet += (char)(Math.Abs(TabDecs[i] % 2) * Math.Pow(2, RangBit));
                i++;
                RangBit--;
                if (RangBit < 0)
                {
                    if (i < LenghtMessageToShow)//à remplacer
                    {
                        MessageDecoder += Voctet;

                    }
                    Voctet = (char)0;//on réinitialise Voctet pour qu'il puisse être pret à contenir le prochain octet 
                    RangBit = 7; //on remet RangBit à 7 pour qu'il se remette à faire des paquets de 8 bits 
                }
            }
            return MessageDecoder;
        }

        public void ClearMessage()
        {

            MessageDecoder = string.Empty;
        }
    }
}