using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace UbbHighlightCodeEditor.Languages
{
    public class CSharp : ILanguage
    {
        public Dictionary<Regex, string> Regexes
        {
            get;
            private set;
        }

        public Dictionary<string, Style> Styles
        {
            get;
            private set;
        }

        // 关键字列表取自SyntaxHighlighter 3.0.83
        string _keywords = "abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|" +
                    "continue|decimal|default|delegate|do|double|else|enum|event|explicit|" +
                    "extern|false|finally|fixed|float|for|foreach|get|goto|if|implicit|in|int|" +
                    "interface|internal|is|lock|long|namespace|new|null|object|operator|out|" +
                    "override|params|private|protected|public|readonly|ref|return|sbyte|sealed|set|" +
                    "short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|" +
                    "typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|while"
                    // 似乎var和dynamic也很重要
                    + "|var";

        public CSharp()
        {
            this.Styles = new Dictionary<string, Style> {
                { "Comment", new Style(Colors.Green) },
                { "String", new Style(Colors.Brown) },
                { "Keyword", new Style(Colors.Blue) },
            };

            this.Regexes = new Dictionary<Regex, string>
            {
                { Re.MultiLineCComments, "Comment" },
                { Re.SingleLineCComments, "Comment" },
                { new Regex(@"@""([^\""\n]|\.)*""", RegexOptions.Compiled ), "String" },
                { Re.DoubleQuotedString, "String" },
                { new Regex(@"\b(" + _keywords + @")\b", RegexOptions.Compiled | RegexOptions.Singleline ), "Keyword" },
                { new Regex(@"\bpartial(?=\s+(?:class|interface|struct)\b)",RegexOptions.Compiled),	"Keyword" },
			    { new Regex(@"\byield(?=\s+(?:return|break)\b)",RegexOptions.Compiled), "Keyword" }
            };
        }

        public override string ToString()
        {
            return "C#";
        }
    }
}