using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace UbbHighlightCodeEditor
{
    public interface ILanguage
    {
        /// <summary>
        /// 正则表达式字典
        /// </summary>
        Dictionary<Regex, string> Regexes { get; }

        /// <summary>
        /// 样式字典
        /// </summary>
        Dictionary<string, Style> Styles { get; }
    }
}
