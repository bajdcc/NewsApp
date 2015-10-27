﻿using System;
using System.ComponentModel;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
                    BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(0, 0,
                    new Duration(TimeSpan.FromSeconds(1)))));
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                Dispatcher.Invoke(() =>
                    BeginAnimation(UIElement.OpacityProperty,
                    new DoubleAnimation(0, 1,
                    new Duration(TimeSpan.FromSeconds(2)))));
            });
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
                Dispatcher.Invoke(() => Close());
            });
        }
    }
}
