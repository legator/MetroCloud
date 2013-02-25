using MusicCloudPlayer.Player;
using SchedulerTV.Resources.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MusicCloudPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainViewModel ViewModel { get; private set; }
        public static IStreamMediaPlayer StreamMediaPlayer;

        public App()
        {
            ViewModel = new MainViewModel();
            StreamMediaPlayer = new WindowsMediaPlayer();
            StreamMediaPlayer.Init();
            if (MusicCloudPlayer.Properties.Settings.Default.Logged)
            {
                ViewModel.Auth();
                ViewModel.IsLogged = true;
            }
            else ViewModel.IsLogged = false;
            ViewModel.IsPlay = false;
            //ViewModel.Volume = MusicCloudPlayer.Properties.Settings.Default.Volume;
        }
    }
}
