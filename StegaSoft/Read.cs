using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StegaSoft;


namespace StegaSoft
{
    public class Read
    {
        private int[] StreamDecimal;
        public string FileName { get; set; }

        public void Operation()
        {
            StreamDecimal = GetDecs(FileName, 154);
            GetLowerBit(StreamDecimal);
        }

        private int[] GetDecs(string fileName, int indexBegin)
        {
            byte[] fileBytes = File.ReadAllBytes(fileName);
            int[] sb = new int[fileBytes.Length];

            for (int i = indexBegin; i < fileBytes.Length; i++)
            {

                sb[i - indexBegin] = fileBytes[i];

            }

            return sb;
        }

        private void GetLowerBit(int[] TabDecs)
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
                    if (i < 1616)//à remplacer
                    {
                        //Console.Write(Voctet); // On affiche le message caché dans l'image sur la console

                    }
                    Voctet = (char)0;//on réinitialise Voctet pour qu'il puisse être pret à contenir le prochain octet 
                    RangBit = 7; //on remet RangBit à 7 pour qu'il se remette à faire des paquets de 8 bits 
                }
            }
        }
    }
}




