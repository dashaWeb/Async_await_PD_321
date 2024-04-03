using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Async_await
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ShowRandomNumber(object sender, RoutedEventArgs e)
        {
            /*int value = GenereteValue();
            list.Items.Add(value);*/

            /*Task<int> task = Task.Run(GenereteValue);
            task.Wait(); // Freeze*/

            //list.Items.Add(task.Result); // freeze

            // async => alow method to use await keywords
            // await => wait task without freezing

            /*Task<int> task = Task.Run(GenereteValue);
            int value = await task;*/
            int value;
            if (int.TryParse(number.Text, out value))
            {
                list.Items.Add(await GenereteValueAsync(value));
            }
            else
            {
                MessageBox.Show("Error", "Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }
        /*int GenereteValue()
        {
            Thread.Sleep(rnd.Next(10000));
            return rnd.Next(1000);
        }*/
        Task<int> GenereteValueAsync(int value)
        {
            return Task.Run(() =>
             {
                 Thread.Sleep(rnd.Next(10000));
                 return rnd.Next(value);
             });
        }

    }
}
