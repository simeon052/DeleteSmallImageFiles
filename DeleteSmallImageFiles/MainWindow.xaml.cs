using DeleteSmallImageFiles.Core;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
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
            Path.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var Dialog = new CommonOpenFileDialog();
            Dialog.InitialDirectory = Path.Text;
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
                if(ifi == null)
                {
                    return false;
                }
                try
                {
                    if ((ifi.Size < int.Parse(Size.Text)) && (ifi.Width < int.Parse(Width.Text)) && (ifi.Height < int.Parse(Height.Text)))
                    {
                        System.Diagnostics.Debug.WriteLine($"{ifi.ToString()}");
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
                return false;
            }))
            {
                await Task.Run(() =>
                {
                    System.Diagnostics.Debug.WriteLine($"{f}");
                    try
                    {
                        File.Move(f, System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetFileName(f)));
                    }
                    catch
                    {
                        File.Delete(f);
                    }
                });
            }
            Delete.IsEnabled = true;
        }
    }
}
