using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using UbbHighlightCodeEditor.Languages;
using System.Text.RegularExpressions;

namespace UbbHighlightCodeEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 字段和构造方法

        string _text = null;

        // 暂存UBB代码
        string _codeDiscuz;

        //  暂存渲染后的RTF段落
        Paragraph _codeParagraph;

        // 使用边框
        bool _hasBoarder = false;

        public MainWindow()
        {
            _codeDiscuz = string.Empty;
            _codeParagraph = new Paragraph();

            InitializeComponent();

            this.comboLanguage.Items.Add(new CSharp());
            this.comboLanguage.Items.Add(new Python());

            this.comboLanguage.SelectedIndex = 0;
        }

        #endregion

        #region 控件事件处理

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // 语言
            var lang = (ILanguage)comboLanguage.SelectedItem;
            
            // 解析代码
            var pieces = Highlighter.Process(textCode.Code, lang);

            RenderCode(textCode.Code, pieces, lang);
            ShowCodeForUbb(textCode.Code, pieces, lang);

            // 显示结果
            textCode.SetContent(_codeParagraph);
        }

        private void btnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(GetUbbCode());
                MessageBox.Show("已复制到剪贴板中!");
            }
            catch
            {
                MessageBox.Show("访问剪贴板时遇到异常，复制失败");
            }
        }

        private void btnFromClipboard_Click(object sender, RoutedEventArgs e)
        {
            var text = Clipboard.GetText().Replace("\t", "    ");
            textCode.CaretPosition.InsertTextInRun(text);
        }

        private void checkUbb_CheckChanged(object sender, RoutedEventArgs e)
        {
            textCode.IsUbbView = checkUbb.IsChecked ?? false;
        }

        private void checkHasBorder_CheckChanged(object sender, RoutedEventArgs e)
        {
            _hasBoarder = checkHasBorder.IsChecked ?? false;
            ViewChanged(null, textCode.IsUbbView);
        }

        #endregion

        #region 代码高亮处理
        void RenderCode(string code, List<CodePiece> pieces, ILanguage lang)
        {
            var paragraph = new Paragraph();

            int index = 0;
            for (int i = 0; i < pieces.Count; i++)
            {
                var piece = pieces[i];

                // 每个高亮之前的部分
                var pieceCur = code.Substring(index, piece.Index - index);
                paragraph.Inlines.Add(pieceCur);

                // 高亮的代码块
                var span = new Span();
                span.Foreground = new SolidColorBrush(lang.Styles[piece.Style].Color);

                pieceCur = code.Substring(piece.Index, piece.Length);
                span.Inlines.Add(pieceCur);
                paragraph.Inlines.Add(span);

                index = piece.Index + piece.Length;

                // 最后的部分
                if (i == pieces.Count - 1)
                {
                    pieceCur = code.Substring(index, code.Length - index);
                    paragraph.Inlines.Add(pieceCur);
                }
            }

            _codeParagraph = paragraph;
        }

        void ShowCodeForUbb(string code, List<CodePiece> pieces, ILanguage lang)
        {
            int index = 0;
            var sb = new StringBuilder();
            sb.Append("[font=Consolas]");
            for (int i = 0; i < pieces.Count; i++)
            {
                var piece = pieces[i];

                // 每个高亮之前的部分
                var pieceCur = ProcessUbbTag(code.Substring(index, piece.Index - index));
                sb.Append(pieceCur);

                // 高亮的代码块
                var style = lang.Styles[piece.Style];
                pieceCur = ProcessUbbTag(code.Substring(piece.Index, piece.Length));

                sb.Append(style.Begin() + pieceCur + style.End());

                index = piece.Index + piece.Length;

                // 最后的部分
                if (i == pieces.Count - 1)
                {
                    pieceCur = ProcessUbbTag(code.Substring(index, code.Length - index));
                    sb.Append(pieceCur);
                }
            }
            sb.Append("[/font]");
            _codeDiscuz = sb.ToString();
        }

        // 处理代码中与Ubb有冲突的部分
        string ProcessUbbTag(string s)
        {
            if (s.IndexOf('[') != -1 && s.IndexOf(']') != -1)
            {
                if (Re.CheckUbb.IsMatch(s))
                {
                    s = Re.CheckUbb.Replace(s, @"[[i][/i]${tag}]");
                }
            }

            return s;
        }

        // 取出Ubb代码
        string GetUbbCode()
        {
            var code = _codeDiscuz;
            if (_hasBoarder)
            {
                code = "[table=100%,WhiteSmoke][tr][td]" + code + "[/td][/tr][/table]";
            }

            return code;
        }

        #endregion

        #region 视图变化

        private void ViewChanged(object sender, bool isUbb)
        {
            if (isUbb)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(GetUbbCode());

                textCode.SetContent(paragraph);
            }
            else
            {
                textCode.SetContent(_codeParagraph);
            }
        }

        #endregion

        private void textAbout_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var form = new AboutWindow();
            form.Owner = this;
            form.ShowDialog();
        }
    }
}
