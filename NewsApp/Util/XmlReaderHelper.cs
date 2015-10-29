using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NewsApp.Util
{
    internal class DateTimeParser
    {
        private static string[] _dateFormats = new[] {
                @"ddd, dd MMM yyyy HH:mm:ss tt \C\S\T",
                @"ddd, dd MMM yyyy hh:mm:ss tt \C\S\T",
                @"ddd, dd MMM yyyy HH:mm:ss tt \E\S\T",
                @"ddd, dd MMM yyyy hh:mm:ss tt \E\S\T",
                @"ddd, dd MMM yyyy HH:mm:ss \E\S\T",
            };

        private static DateTimeStyles _styles = DateTimeStyles.AllowLeadingWhite
            | DateTimeStyles.AllowInnerWhite
            | DateTimeStyles.AllowTrailingWhite
            | DateTimeStyles.AllowWhiteSpaces;

        public string Parse(string dateTime)
        {
            // Attempt default DateTime parse
            DateTime dt;

            if (!DateTime.TryParse(dateTime, out dt))
            {
                // Parse using custom formats
                if (!DateTime.TryParseExact(dateTime, _dateFormats,
                    CultureInfo.InvariantCulture, _styles, out dt))
                {
                    // Throw exception if custom formats can't parse the string.
                    throw new FormatException("dateTime is not a valid DateTime format.");
                }
            }

            return dt.ToUniversalTime().ToString("R", CultureInfo.InvariantCulture);
        }
    }

    class MultiDateFormatXmlReader : XmlTextReader
    {
        private bool readingDate = false;
        private static DateTimeParser _parser = new DateTimeParser();

        public MultiDateFormatXmlReader(Stream s) : base(s) { }

        public MultiDateFormatXmlReader(string inputUri) : base(inputUri) { }

        public override void ReadStartElement()
        {
            if (string.Equals(base.NamespaceURI, string.Empty, StringComparison.InvariantCultureIgnoreCase) &&
                (string.Equals(base.LocalName, "lastBuildDate", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(base.LocalName, "pubDate", StringComparison.InvariantCultureIgnoreCase)))
            {
                readingDate = true;
            }
            base.ReadStartElement();
        }

        public override void ReadEndElement()
        {
            if (readingDate)
            {
                readingDate = false;
            }
            base.ReadEndElement();
        }

        public override string ReadString()
        {
            var text = base.ReadString();
            if (readingDate)
            {
                return _parser.Parse(text);
            }
            return text;
        }
    }
}
