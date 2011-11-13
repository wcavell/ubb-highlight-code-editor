using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;

namespace UbbHighlightCodeEditor
{
    public static class Highlighter
    {
        /// <summary>
        /// 处理过程方法
        /// </summary>
        /// <param name="code">输入的代码</param>
        /// <param name="lang">指定的语言</param>
        /// <returns>返回高亮的代码片段</returns>
        public static List<CodePiece> Process(string code, ILanguage lang)
        {
            var regexes = lang.Regexes.Keys.ToArray();
            var matches = new Match[regexes.Length];

            int index = 0;
            var codePieces = new List<CodePiece>();
            while (index < code.Length)
            {
                int i = 0;
                for (; i < matches.Length; i++)
                {
                    // 是否需要匹配
                    if (matches[i] != null && matches[i].Index >= index)
                        continue;

                    matches[i] = regexes[i].Match(code, index);
                }

                // 取最前的匹配
                i = GetFirstMatch(matches);
                if (i == -1)
                    break;

                // 添加到代码片段
                codePieces.Add(
                    new CodePiece(lang.Regexes[regexes[i]], matches[i])
                );

                // 更新Index，继续迭代
                index = matches[i].Index + matches[i].Length;
            }

            return codePieces;
        }

        // 取得第一个匹配
        private static int GetFirstMatch(Match[] matches)
        {
            int i = 0;
            int k = -1;
            int min = int.MaxValue;
            for (; i < matches.Length; i++)
            {
                var match = matches[i];
                if (match != null && match.Success && match.Index < min)
                {
                    k = i;
                    min = match.Index;
                }
            }

            return k;
        }
    }
}
