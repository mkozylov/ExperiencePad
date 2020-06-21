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
        }

        #region Internal

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
