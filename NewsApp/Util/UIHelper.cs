using System.Windows;
using System.Windows.Media;

namespace NewsApp.Util
{
    public class UIHelper
    {
        static public double MeasureTextWidth(string text, double fontSize, string fontFamily, FontWeight fontWeight)
        {
            var formattedText = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(fontFamily.ToString()),
                fontSize,
                Brushes.Black
            );
            formattedText.SetFontWeight(fontWeight);
            return formattedText.WidthIncludingTrailingWhitespace;
        }
    }
}
