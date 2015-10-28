using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NewsApp.Util
{
    public class UIHelper
    {
        public double MeasureTextWidth(string text, double fontSize, string fontFamily)
        {
            var formattedText = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(fontFamily.ToString()),
                fontSize,
                Brushes.Black
            );
            return formattedText.WidthIncludingTrailingWhitespace;
        }

        /*public void AddMarqueeText(TextBlock text)
        {
            //创建动画资源
            Storyboard storyboard = new Storyboard();

            double lenth = MeasureTextWidth(text.Text, text.FontSize, text.FontFamily.Source);

            //移动动画
            {
                DoubleAnimationUsingKeyFrames WidthMove = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(WidthMove, text);
                DependencyProperty[] propertyChain = new DependencyProperty[]
                {
                    TextBlock.RenderTransformProperty,
                    TransformGroup.ChildrenProperty,
                    TranslateTransform.XProperty,
                };
                Storyboard.SetTargetProperty(WidthMove, new PropertyPath("(0).(1)[3].(2)", propertyChain));//设置动画类型
                WidthMove.KeyFrames.Add(new EasingDoubleKeyFrame(canva1.Width, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0))));//添加时间线
                WidthMove.KeyFrames.Add(new EasingDoubleKeyFrame(-lenth, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, (int)(lenth / 50)))));
                storyboard.Children.Add(WidthMove);
            }

            storyboard.RepeatBehavior = RepeatBehavior.Forever;
            storyboard.Begin();
        }*/
    }
}
