using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace StegaSoft.Services
{
    class CFileDecimal
    {
        //base class, got a method who return the decimal stream of the file in param
        protected int[] StreamDecimal;
        public StorageFile file { get; set; }
        protected async Task<int[]> GetDecimalStream(StorageFile file, int indexBegin)
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

        protected async Task<Byte[]> GetBytes(StorageFile file)
        {
            IBuffer buffer = await FileIO.ReadBufferAsync(file);

            byte[] fileBytes = buffer.ToArray();

            return fileBytes;
        }

        public async void deb(string test)//debug function, print the param 
        {
            var dialog = new MessageDialog(test);
            await dialog.ShowAsync();
        }
    }
}
