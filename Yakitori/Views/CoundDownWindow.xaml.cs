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
using System.Windows.Shapes;

namespace Yakitori.Views
{
    /// <summary>
    /// CoundDownWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CoundDownWindow : Window
    {
        public CoundDownWindow()
        {
            InitializeComponent();

            DataContext = new ViewModels.CountDownWindowViewModel();
        }

        public void Show(int timeout, Action action)
        {
            var window = new CoundDownWindow();

            window.ShowDialog();

            Task.Factory.StartNew(() =>
            {
                for (var n = timeout; n > 0; n--)
                {
                    window.Dispatcher.Invoke(() =>
                    {
                        LabelCountDown.Text = n.ToString();
                    });

                    System.Threading.Thread.Sleep(1000);
                }

                window.Dispatcher.Invoke(action);
                window.Dispatcher.Invoke(window.Close);
            });
        }

        public ViewModels.CountDownWindowViewModel ViewModel { get; private set; }
    }
}
