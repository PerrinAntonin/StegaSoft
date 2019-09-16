﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace StegaSoft
{
    public class FileDecimal
    {


        //base class, got a method who return the decimal stream of the file in param
        protected int[] StreamDecimal;
        public StorageFile file { get; set; }
        protected async Task<int[]> GetDeicmalStream(StorageFile file, int indexBegin)
        {
            IBuffer buffer = await FileIO.ReadBufferAsync(file);

            byte[] fileBytes = buffer.ToArray();

            int[] sb = new int[fileBytes.Length];

            for (int i = indexBegin; i < fileBytes.Length; i++)
            {

                sb[i - indexBegin] = fileBytes[i];

            }

            return sb;
        }

        protected async Task<byte[]> GetBytes(StorageFile file)
        {
             IBuffer buffer = await FileIO.ReadBufferAsync(file);

            byte[] fileBytes = buffer.ToArray();




            var enc = new UTF32Encoding();
            String s = "This ªªªªªªicodetring";

            // Encode the string.
            Byte[] encodedBytes = enc.GetBytes(s);


            String decodedString = enc.GetString(encodedBytes);
            var dialog = new MessageDialog(decodedString);
            await dialog.ShowAsync();

            return fileBytes;
        }

        public async void deb()
        {
            var dialog = new MessageDialog("coucou");
            await dialog.ShowAsync();
        }
    }
}
