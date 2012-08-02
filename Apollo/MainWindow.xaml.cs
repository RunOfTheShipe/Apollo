using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Apollo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _SelectedFilePath;
        public string SelectedFilePath
        {
            get { return _SelectedFilePath; }
            set { _SelectedFilePath = value; OnPropertyChanged("SelectedFilePath"); }
        }

        private bool _IsFileOpen;
        public bool IsFileOpen
        {
            get { return _IsFileOpen; }
            set { _IsFileOpen = value; OnPropertyChanged("IsFileOpen"); }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string strPropertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(strPropertyName));
        }
        #endregion

        public MainWindow()
        {
            DataContext = this;

            SelectedFilePath = String.Empty;
            IsFileOpen = true;

            InitializeComponent();
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            // Prompt the user to 
            SelectedFilePath = "File Selected";
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            IsFileOpen = true;
        }

        private void SaveCloseFile_Click(object sender, RoutedEventArgs e)
        {
            IsFileOpen = false;
        }
    }
}
