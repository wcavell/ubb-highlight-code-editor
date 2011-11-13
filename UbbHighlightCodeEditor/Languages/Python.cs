using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace UbbHighlightCodeEditor.Languages
{
    class Python : ILanguage
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
        string _keywords =  "and|assert|break|class|continue|def|del|elif|else|" +
						"except|exec|finally|for|from|global|if|import|in|is|" +
						"lambda|not|or|pass|print|raise|return|try|yield|while";

		string _funcs = "__import__|abs|all|any|apply|basestring|bin|bool|buffer|callable|" +
					"chr|classmethod|cmp|coerce|compile|complex|delattr|dict|dir|" +
					"divmod|enumerate|eval|execfile|file|filter|float|format|frozenset|" +
					"getattr|globals|hasattr|hash|help|hex|id|input|int|intern|" +
					"isinstance|issubclass|iter|len|list|locals|long|map|max|min|next|" +
					"object|oct|open|ord|pow|print|property|range|raw_input|reduce|" +
					"reload|repr|reversed|round|set|setattr|slice|sorted|staticmethod|" +
					"str|sum|super|tuple|type|type|unichr|unicode|vars|xrange|zip";

		string _special = "None|True|False|self|cls|class_";

        public Python()
        {
            this.Regexes = new Dictionary<Regex, string>
            {
				{ Re.SingleLinePerlComments, "Comment" },
				{ new Regex(@"('''|\""\""\"")([^\1])*?\1", RegexOptions.Multiline |RegexOptions.Compiled) , "Comment" },
                { new Regex(@"[ur]?(['""])([^\1\n]|)*\1", RegexOptions.Compiled ), "String" },
                { new Regex(@"\b(" + _keywords + @")\b", RegexOptions.Compiled), "Keyword" },
                { new Regex(@"\b(" + _funcs + @")\b", RegexOptions.Compiled), "Function" },
                { new Regex(@"\b(" + _special + @")\b", RegexOptions.Compiled), "Special" },
            };

            this.Styles = new Dictionary<string, Style>
            {
                { "Comment", new Style(Colors.Green) },
                { "String", new Style(Colors.Green) },
                { "Keyword", new Style(Colors.Orange) },
                { "Function", new Style(Colors.Blue) },
                { "Special", new Style(Colors.Gray) },
            };
        }

        public override string ToString()
        {
            return "Python";
        }




       
    }
}
