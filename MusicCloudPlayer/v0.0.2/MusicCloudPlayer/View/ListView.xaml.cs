using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicCloudPlayer.View
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

        private void ChangeMusiCIndex(object sender, SelectionChangedEventArgs e)
        {
            App.ViewModel.PlayByIndex(CloudAudioList.SelectedIndex);
        }
    }
}
