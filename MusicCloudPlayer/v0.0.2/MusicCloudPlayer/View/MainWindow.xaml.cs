using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicCloudPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
            App.ViewModel.Load();
        }

        private void WindowMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.GetPosition(this).Y < 38)
            {
                DragMove();
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsSearch)
            {
                SongControl.Visibility = Visibility.Collapsed;
                SongTitle.Visibility = Visibility.Collapsed;
                SongSearch.Visibility = Visibility.Visible;
            }
            else
            {
                SongTitle.Visibility = Visibility.Collapsed;
                SongSearch.Visibility = Visibility.Collapsed;
                SongControl.Visibility = Visibility.Visible;
            }
            App.ViewModel.IsSearch = !App.ViewModel.IsSearch;
        }

        private void ChangeToControl(object sender, MouseEventArgs e)
        {
            if (!App.ViewModel.IsSearch)
            {
                SongControl.Visibility = Visibility.Visible;
                SongTitle.Visibility = Visibility.Collapsed;
            }
        }

        private void ChangeToTitle(object sender, MouseEventArgs e)
        {
            if (!App.ViewModel.IsSearch)
            {
                Thread.Sleep(100);
                SongControl.Visibility = Visibility.Collapsed;
                SongTitle.Visibility = Visibility.Visible;
            }
        }

        private void ListClick(object sender, RoutedEventArgs e)
        {
            if (App.ViewModel.IsList)
            {
                this.Height = 480;
                MusicCloudPlayer.View.ListView list = new MusicCloudPlayer.View.ListView();
                AudioList.Children.Add(list);
            }
            else
            {
                this.Height = 80;
                AudioList.Children.Clear();
            }
        }

        private void AccountConnectClick(object sender, RoutedEventArgs e)
        {
            if (App.ViewModel.IsLogged)
            {
                App.ViewModel.IsLogged = !App.ViewModel.IsLogged;
                App.ViewModel.LogOff();
            } 
            else
            {
                App.ViewModel.Auth();
                App.ViewModel.IsLogged = !App.ViewModel.IsLogged;
            }
        }

        private void PinApp(object sender, RoutedEventArgs e)
        {
             App.ViewModel.IsTop = this.Topmost = !this.Topmost;
        }

        private void MinApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FindAudio(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPauseClick(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsPlay)
            {
                App.ViewModel.Play();
            }
            else App.ViewModel.Pause();
            App.ViewModel.IsPlay = !App.ViewModel.IsPlay;
        }

        private void inited(object sender, EventArgs e)
        {
            //this.DataContext = App.ViewModel;
        }

        private void PositionValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan ts = TimeSpan.FromSeconds((double)AudioPozition.Value);
            App.ViewModel.Position = ts;
        }

        private void ClickNextSong(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PlayNext();
        }
    }
}
