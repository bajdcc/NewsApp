using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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
        static private Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool isNew;
            mutex = new Mutex(true, "NewsApp#bajdcc-v1.0", out isNew);
            if (!isNew)
            {
                ActivateOtherWindow();
                Shutdown();
                return;
            }
            
            var ss = new SplashScreen("splash.jpg");
            ss.Show(false, true);
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                new List<Action>() {
                    () => ss.Close(TimeSpan.FromSeconds(1)),
                    () => MainWindow.Show(),
                }.ToObservable().Subscribe(x => Dispatcher.Invoke(x));
            });
            base.OnStartup(e);
        }

        private void ActivateOtherWindow()
        {
            var other = Util.AppHelper.FindWindow(null, Resources["WindowCaption"] as string);
            if (other != IntPtr.Zero)
            {
                Util.AppHelper.SetForegroundWindow(other);
                if (Util.AppHelper.IsIconic(other))
                    Util.AppHelper.OpenIcon(other);
            }
        }
    }
}
