using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace NewsApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool closed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!closed)
            {
                BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(1, 0,
                    new Duration(TimeSpan.FromSeconds(2))));
                e.Cancel = true;
                Task.Factory.StartNew(()=>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    Dispatcher.Invoke(() => Close());
                });                
                closed = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(UIElement.OpacityProperty,
                new DoubleAnimation(0, 1,
                new Duration(TimeSpan.FromSeconds(2))));
        }
    }
}
