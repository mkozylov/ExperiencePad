using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Reflection;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using NWrath.Synergy.Common.Extensions;

namespace ExperiencePad
{
    /// <summary>
    /// Логика взаимодействия для Editor.xaml
    /// </summary>
    public partial class Editor : TextEditor
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                                                                     "Text",
                                                                     typeof(string),
                                                                     typeof(Editor),
                                                                     new FrameworkPropertyMetadata
                                                                     {
                                                                         DefaultValue = default(string),
                                                                         BindsTwoWayByDefault = true,
                                                                         PropertyChangedCallback = OnTextPropertyChanged
                                                                     });

        public new string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public Editor()
        {
            InitializeComponent();

            Loaded += Editor_Loaded;
        }

        #region Internal

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            var lineNumberMargin = (Line)TextArea.LeftMargins[1];
            lineNumberMargin.Margin = new Thickness(20, 0, 0, 0);

            TextArea.Options.HighlightCurrentLine = true;
            TextArea.Options.EnableHyperlinks = true;
            
            RegisterHighlightingDefinitions();
        }

        private void RegisterHighlightingDefinitions()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var definitionNames = assembly.GetManifestResourceNames()
                                          .Where(x => x.EndsWith(".xshd"));

            foreach (var fullName in definitionNames)
            {
                using var stream = assembly.GetManifestResourceStream(fullName);
                using var reader = new System.Xml.XmlTextReader(stream);

                var name = fullName.Split('.').Extract(s => s[s.Length - 2]);

                HighlightingManager.Instance.RegisterHighlighting(
                    name,
                    new string[0],
                    HighlightingLoader.Load(reader, HighlightingManager.Instance)
                    );
            }
        }

        protected static void OnTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var editor = (Editor)obj;

            if (editor.Document != null)
            {
                var caretOffset = editor.CaretOffset;
                var newValue = e.NewValue?.ToString() ?? string.Empty;

                editor.Document.Text = newValue;
                editor.CaretOffset = Math.Min(caretOffset, newValue.Length);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (Document != null)
            {
                Text = Document.Text;
            }

            base.OnTextChanged(e);
        }

        #endregion
    }
}
