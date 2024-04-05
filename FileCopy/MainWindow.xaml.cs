using Microsoft.Win32;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileCopy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Source { get; set; }
        public string Destination { get; set; }

        public float Progress { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Source = "C:\\Users\\konopelko\\Downloads\\SP_async_await_lib.pdf";
            Destination = "C:\\Users\\konopelko\\Desktop\\Test";

            srcTextBox.Text = Source;
            destTextBox.Text = Destination;

        }

        private void OpenSource_btn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                Source = dialog.FileName;
                srcTextBox.Text = Source;
            }
        }

        private void OpenDest_btn(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Destination = dialog.FileName;
                destTextBox.Text = Destination;
            }
        }

        private async void Copy_btn(object sender, RoutedEventArgs e)
        {
            string fileName = System.IO.Path.GetFileName(Source);
            string destPath = System.IO.Path.Combine(Destination, fileName);
            //MessageBox.Show(destPath);
            //File.Copy(Source, destPath,true);

            /*FileStream srcStream = new FileStream(Source, FileMode.Open, FileAccess.Read);
            FileStream destStream = new FileStream(destPath, FileMode.Create, FileAccess.Write);
            byte[] bytes= new byte[1024 * 8];
            int bits = 0;
            do
            {
                bits = srcStream.Read(bytes, 0, bytes.Length);
                destStream.Write(bytes, 0, bits);
            } while (bits > 0);
            srcStream.Close();
            destStream.Close();*/
            /*
                        using (FileStream srcStream = new FileStream(Source, FileMode.Open, FileAccess.Read))
                        {
                            using (FileStream destStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
                            {
                                byte[] bytes = new byte[1024 * 8];
                                int bits = 0;
                                do
                                {
                                    bits = srcStream.Read(bytes, 0, bytes.Length);
                                    destStream.Write(bytes, 0, bits);
                                    float res = srcStream.Length / destStream.Length * 100;
                                    percent.Value = res;
                                } while (bits > 0);
                            }
                        }*/
            await CopyFileAsync(Source, destPath);
        }
        private Task CopyFileAsync(string src, string dest)
        {
            return Task.Run(() =>
            {
                using (FileStream srcStream = new FileStream(src, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream destStream = new FileStream(dest, FileMode.Create, FileAccess.Write))
                    {
                        byte[] bytes = new byte[1024 * 8];
                        int bits = 0;
                        do
                        {
                            bits = srcStream.Read(bytes, 0, bytes.Length);
                            destStream.Write(bytes, 0, bits);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                float res = srcStream.Length / destStream.Length * 100;
                                percent.Value = res;
                            });
                        } while (bits > 0);
                    }
                };
            });
        }
    }

}
