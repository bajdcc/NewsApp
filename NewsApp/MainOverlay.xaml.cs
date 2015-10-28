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
using System.Windows.Threading;

namespace NewsApp
{
    /// <summary>
    /// MainOverlay.xaml 的交互逻辑
    /// </summary>
    public partial class MainOverlay : Window
    {
        DispatcherTimer timer;

        public MainOverlay()
        {
            InitializeComponent();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (sender is MainOverlay)
            {
                (sender as MainOverlay).Topmost = true;
            }
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            uint windowLong = Util.AppHelper.GetWindowLong(hwnd, -20);
            Util.AppHelper.SetWindowLong(hwnd, -20, windowLong | 0x20);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, Timer_Tick, Dispatcher);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            time.Content = DateTime.Now.ToShortTimeString();
        }

        public void AnimationClose()
        {
            BeginAnimation(UIElement.OpacityProperty,
                new System.Windows.Media.Animation.DoubleAnimation(1, 0,
                new Duration(TimeSpan.FromSeconds(1))));
            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => Close()));
            });
        }
    }
}
