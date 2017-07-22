using DeleteSmallImageFiles.Core;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeleteSmallImageFiles
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var Dialog = new CommonOpenFileDialog();
            Dialog.IsFolderPicker = true;
            Dialog.EnsureReadOnly = false;
            Dialog.AllowNonFileSystemItems = false;
            Dialog.DefaultDirectory = Application.Current.StartupUri.LocalPath;
            var Result = Dialog.ShowDialog();

            if (Result == CommonFileDialogResult.Ok)
            {
                Path.Text = Dialog.FileName;
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Delete.IsEnabled = false;

            foreach (var f in ImageFileLister.FindAll(Path.Text, (ifi) => 
            {
                System.Diagnostics.Debug.WriteLine($"{ifi.ToString()}");
                return true;
            }))
            {
                await Task.Run(() =>
                {
                    System.Diagnostics.Debug.WriteLine($"{f}");
                });
            }
            Delete.IsEnabled = true;
        }
    }
}
