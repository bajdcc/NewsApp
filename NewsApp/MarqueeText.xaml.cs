using NewsApp.Machine;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewsApp
{
    /// <summary>
    /// MarqueeText.xaml 的交互逻辑
    /// </summary>
    public partial class MarqueeText : UserControl
    {
        NewsMessage msg = null;
        double time;

        public double Time
        {
            get { return time; }
        }

        public MarqueeText(NewsMessage msg)
        {
            this.msg = msg;
            InitializeComponent();
            Init();            
        }

        private void Init()
        {
            if (msg.Empty)
            {
                
            }
            else
            {
                var length = 60 + Util.UIHelper.MeasureTextWidth("【】" + msg.Content + msg.Origin, TextBox.FontSize, TextBox.FontFamily.Source, TextBox.FontWeight);
                OriginText.Text = msg.Origin;
                ContentText.Text = msg.Content;
                var blank = (double)App.Current.Resources["MarqueeBlank"];
                var pps = (double)App.Current.Resources["PixelPerSeconds"];
                var storyboard = base.Resources["Storyboard1"] as Storyboard;
                storyboard.Completed += Storyboard_Completed;
                this.time = 1000 * (length + blank) / pps;
                (storyboard.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[1].KeyTime =
                    KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000 * (length + SystemParameters.WorkArea.Width) / pps));
                (storyboard.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[1].Value =
                    0 - SystemParameters.WorkArea.Width - length;
                Width = length;
                Height = TextBox.Height;
            }
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            if (Parent is Canvas)
            {
                var canvas = Parent as Canvas;
                canvas.Children.Remove(this);
            }
        }
    }
}
