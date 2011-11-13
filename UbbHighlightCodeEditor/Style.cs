using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace UbbHighlightCodeEditor
{
    // 样式
    public class Style
    {
        public Color Color { get; private set; }
        public bool IsBold { get; private set; }
        public bool IsItalic { get; private set; }

        public Style(Color color, bool isBold = false, bool isItalic = false)
        {
            this.Color = color;
            this.IsBold = isBold;
            this.IsItalic = isItalic;
        }

        public string Begin()
        {
            var tag = string.Format("[color=#{0:x2}{1:x2}{2:x2}]",
                this.Color.R, this.Color.G, this.Color.B);

            if (this.IsBold)
                tag += "[b]";

            if (this.IsItalic)
                tag += "[i]";

            return tag;
        }

        public string End()
        {
            var tag = string.Empty; ;

            if (this.IsItalic)
                tag += "[/i]";

            if (this.IsBold)
                tag += "[/b]";

            tag += "[/color]";

            return tag;
        }

        public override string ToString()
        {
            return this.Color.ToString() + (this.IsBold ? " Bold" : "") + 
                (this.IsItalic ? " Italic" : "");
        }
    }
}
