using NewsApp.Machine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace NewsApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MAX_LOG_LINES = 30;

        static MainWindow _this;
        static Queue<string> _msgQueue = new Queue<string>();
        private NewsMachine _newsMachine;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private bool scrollToEnd = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _this = this;

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "程序开始运行";
            notifyIcon.BalloonTipTitle = "NewsApp";
            notifyIcon.Text = "NewsApp Tray";
            notifyIcon.Icon = System.Drawing.SystemIcons.WinLogo;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[]
            {
                new System.Windows.Forms.MenuItem("关于", (obj, args) =>
                    System.Windows.MessageBox.Show(this, "NewsApp by bajdcc", "NewsApp")),
                new System.Windows.Forms.MenuItem("-"),
                new System.Windows.Forms.MenuItem("显示主界面", (obj, args) =>
                    Show()),
                new System.Windows.Forms.MenuItem("隐藏主界面", (obj, args) =>
                    Hide()),
                new System.Windows.Forms.MenuItem("-"),
                new System.Windows.Forms.MenuItem("显示新闻", (obj, args) =>
                    {
                        if (_newsMachine != null && _newsMachine.Overlay != null)
                        {
                            _newsMachine.Overlay.Show();
                        }
                    }),
                new System.Windows.Forms.MenuItem("隐藏新闻", (obj, args) =>
                    {
                        if (_newsMachine != null && _newsMachine.Overlay != null)
                        {
                            _newsMachine.Overlay.Hide();
                        }
                    }),
                new System.Windows.Forms.MenuItem("关闭新闻", (obj, args) =>
                    {
                        if (_newsMachine != null && _newsMachine.Overlay != null)
                        {
                            _newsMachine.Cancel(true);
                        }
                    }),
                new System.Windows.Forms.MenuItem("背景透明", (obj, args) =>
                    {
                        NewsMachine.Settings.TransparentBackground = !NewsMachine.Settings.TransparentBackground;
                        var _this = obj as System.Windows.Forms.MenuItem;
                        _this.Checked = !_this.Checked;
                        if (_newsMachine != null && _newsMachine.Overlay != null)
                        {
                            _newsMachine.Cancel(true);                            
                        }
                    })
                {
                    Checked = false
                },
                new System.Windows.Forms.MenuItem("鼠标穿透", (obj, args) =>
                    {
                        NewsMachine.Settings.TransparentWindow = !NewsMachine.Settings.TransparentWindow;
                        var _this = obj as System.Windows.Forms.MenuItem;
                        _this.Checked = !_this.Checked;
                        if (_newsMachine != null && _newsMachine.Overlay != null)
                        {
                            _newsMachine.Cancel(true);
                        }
                    })
                {
                    Checked = true
                },
                new System.Windows.Forms.MenuItem("-"),
                new System.Windows.Forms.MenuItem("手动刷新", (obj, args) =>
                    {
                        if (_newsMachine != null)
                        {
                            _newsMachine.Reset();
                        }
                    }),
                new System.Windows.Forms.MenuItem("-"),
                new System.Windows.Forms.MenuItem("退出", (obj, args) =>
                    AnimateClose())
            });

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
            if (_newsMachine != null)
            {
                InternClose();
                Close();
            }
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
            lock(_msgQueue)
            {
                _msgQueue.Enqueue(msg);
                if (_msgQueue.Count > MAX_LOG_LINES)
                    _msgQueue.Dequeue();
                if (_this != null)
                {
                    _this.InternTraceOutput(msg);
                }
            }
        }

        private void InternTraceOutput(string msg)
        {
            var text = string.Join("\n", _msgQueue);
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                this.Log.Text = text;
                this.Log.CaretIndex = this.Log.Text.Length;
                if (scrollToEnd)
                {
                    this.Log.ScrollToEnd();
                    this.Log.Focus();
                }                
            }));
        }
        private void InternClose()
        {
            _newsMachine.CloseOverlay(true);
            _newsMachine = null;
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        private void AppWindow_Closing(object sender, CancelEventArgs e)
        {
            if (_newsMachine != null)
            {
                InternClose();
            }
        }

        private void MenuItem_Click_ScrollToEnd(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem item = sender as System.Windows.Controls.MenuItem;
            scrollToEnd = item.IsChecked;
        }

        private void MenuItem_Click_Clear(object sender, RoutedEventArgs e)
        {
            Log.Text = string.Empty;
        }
    }
}
