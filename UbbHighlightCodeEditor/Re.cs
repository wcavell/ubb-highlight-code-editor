using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UbbHighlightCodeEditor
{
    // 常用正则表达式
    public class Re
    {
        public static readonly Regex MultiLineCComments = new Regex(@"/\*[\s\S]*?\*/",
            RegexOptions.Compiled | RegexOptions.Singleline);

        public static readonly Regex SingleLineCComments = new Regex(@"//.*?(?=" + Environment.NewLine + ")",
            RegexOptions.Compiled | RegexOptions.Multiline);

        public static readonly Regex SingleLinePerlComments = new Regex(@"#.*?(?=" + Environment.NewLine + ")",
            RegexOptions.Compiled | RegexOptions.Multiline);

        public static readonly Regex DoubleQuotedString = new Regex(@"""([^\""\n]|\.)*""",
            RegexOptions.Compiled);

        public static readonly Regex SingleQuotedString = new Regex(@"'([^\'\n]|\.)*'",
            RegexOptions.Compiled);

        public static readonly Regex CheckUbb = new Regex(@"\[/?(?<tag>b|i|u|qq|ra|rm|fly|img|url|wma|wmv|code|hide|list|email|quote)\]",
                   RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
