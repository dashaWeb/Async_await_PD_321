using IOExtensions;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
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

namespace FileCopy_het8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel model;
        public MainWindow()
        {
            InitializeComponent();
            model = new ViewModel();
            this.DataContext = model;
        }
        private void OpenSource_btn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                model.Source = dialog.FileName;
                //srcTextBox.Text = model.Source;
            }
        }

        private void OpenDest_btn(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                model.Destination = dialog.FileName;
                //destTextBox.Text = model.Destination;
            }
        }

        private async void Copy_btn(object sender, RoutedEventArgs e)
        {
            string fileName = System.IO.Path.GetFileName(model.Source);
            string destPath = System.IO.Path.Combine(model.Destination, fileName);
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
            CopyProcessInfo info = new CopyProcessInfo(fileName);
            model.AddProcess(info);
            await CopyFileAsync(model.Source, destPath, info);
            info.Percentage = 100;
            MessageBox.Show("Complate");
        }
        private Task CopyFileAsync(string src, string dest, CopyProcessInfo info)
        {
            return FileTransferManager.CopyWithProgressAsync(src, dest, (progress) =>
            {
                model.Progress = progress.Percentage;
                info.Percentage = progress.Percentage;
                info.BytesPerSecond = progress.BytesPerSecond;
            }, false);


            /*model.Progress = 0;
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

                            float res = destStream.Length / (srcStream.Length / 100);
                            model.Progress = res;

                        } while (bits > 0);
                    }
                };
            });*/
        }
    }
    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {
        private ObservableCollection<CopyProcessInfo> processees;
        public string Source { get; set; }
        public string Destination { get; set; }
        public double Progress { get; set; }
        public bool IsWaiting => Progress == 0;
        public ViewModel()
        {
            processees = new ObservableCollection<CopyProcessInfo>();
        }
        public IEnumerable<CopyProcessInfo> Processes => processees; // get
        public void AddProcess(CopyProcessInfo info)
        {
            processees.Add(info);
        }

    }
    [AddINotifyPropertyChangedInterface]
    public class CopyProcessInfo
    {
        public string FileName { get; set; }
        public double Percentage { get; set; }
        public int PercentageInt => (int)Percentage;
        public double BytesPerSecond { get; set; }
        public double MegaBytesPerSeconds => Math.Round(BytesPerSecond / 1024 / 1024, 1);
        public CopyProcessInfo(string fileName)
        {
            this.FileName = fileName;
        }
    }
}
