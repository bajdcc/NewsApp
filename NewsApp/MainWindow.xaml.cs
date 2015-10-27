﻿using NewsApp.Machine;
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
        static MainWindow _this;
        static Queue<string> _msgQueue = new Queue<string>(100);
        NewsMachine _newsMachine;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _this = this;
            Observable.Start(() =>
            {
                Dispatcher.Invoke(() =>
                    BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(0, 0,
                    new Duration(TimeSpan.FromSeconds(1)))));
            }).Delay(TimeSpan.FromSeconds(3))
            .Subscribe(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(0, 1,
                    new Duration(TimeSpan.FromSeconds(2))));
                    Activate();
                });
            });

            _newsMachine = new NewsMachine();
            _newsMachine.OnLogging += TraceOutput;
            _newsMachine.Start();
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
            BeginAnimation(UIElement.OpacityProperty,
                new DoubleAnimation(1, 0,
                new Duration(TimeSpan.FromSeconds(2))));
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => Close()));
            });
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
            Dispatcher.Invoke(() =>
            {
                this.Log.Text = string.Join("\n", text);
                this.Log.ScrollToEnd();
            });
        }
    }
}
