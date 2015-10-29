using NewsApp.Machine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace NewsApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow _this;
        static Queue<string> _msgQueue = new Queue<string>(100);
        private NewsMachine _newsMachine;
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _this = this;

            notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipText = "程序开始运行";
            notifyIcon.Text = "托盘图标";
            notifyIcon.Icon = System.Drawing.SystemIcons.WinLogo;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            var aboutMenu = new MenuItem("About", (obj, args) => System.Windows.MessageBox.Show(this, "NewsApp by bajdcc", "NewsApp"));
            var exitMenu = new MenuItem("Exit", (obj, args) => AnimateClose());

            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { aboutMenu, exitMenu });

            _newsMachine = new NewsMachine();
            _newsMachine.OnLogging += TraceOutput;            
            _newsMachine.MainDispatcher = Dispatcher;
            _newsMachine.Start();
        }

        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Activate();
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                AnimateClose();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AnimateClose();
        }

        private void AnimateClose()
        {
            InternClose();
            Close();            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        static public void TraceOutput(IMachineContext context, string msg)
        {
            _msgQueue.Enqueue(msg);
            if (_this != null)
            {
                _this.InternTraceOutput(msg);
            }
        }

        private void InternTraceOutput(string msg)
        {
            var text = string.Join("\n", _msgQueue);
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                this.Log.Text = string.Join("\n", text);
                this.Log.CaretIndex = this.Log.Text.Length;
                this.Log.ScrollToEnd();
                this.Log.Focus();
            }));
        }
        private void InternClose()
        {
            _newsMachine.Cancel();
            notifyIcon.Dispose();
        }

        private void AppWindow_Closing(object sender, CancelEventArgs e)
        {
            InternClose();
        }
    }
}
