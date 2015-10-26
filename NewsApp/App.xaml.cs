using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NewsApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            bool isNew;
            var mutex = new Mutex(true, "NewsApp#bajdcc-v1.0", out isNew);
            if (!isNew)
            {
                Shutdown();
            }

            var ss = new SplashScreen("splash.jpg");
            ss.Show(false, true);
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                Dispatcher.Invoke(() =>
                {
                    ss.Close(TimeSpan.FromSeconds(1));
                });
                Dispatcher.Invoke(() =>
                {
                    MainWindow.Show();
                });
            });
            base.OnStartup(e);
        }
    }
}
