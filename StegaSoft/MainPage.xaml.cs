﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace StegaSoft
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
           
            this.InitializeComponent();
        }

        private void HyperlinkButton_ClickRead(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReadPage));
        }
        private void HyperlinkButton_ClickWrite(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WritePage), null);
        }


    }
}

