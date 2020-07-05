using ExperiencePad.Data;
using ExperiencePad.Logic;
using ICSharpCode.AvalonEdit.Highlighting;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ExperiencePad
{
    /// <summary>
    /// Логика взаимодействия для EditorPanel.xaml
    /// </summary>
    public partial class EditorPanel : UserControl
    {
        public MainDataContext MainDataContext { get; set; }

        public DataManager DataManager { get; set; }

        public EditorPanel()
        {
            InitializeComponent();

            Loaded += EditorPanel_Loaded;
        }

        private void EditorPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;

            dataContext.Injector.BindProperties(this);

            dataContext.PropertyChanged += DataContext_PropertyChanged;
        }

        private void DataContext_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedRecord" 
                && RecordTypeBox != null)
            {
                RecordTypeBox.SelectedValue = MainDataContext.SelectedRecord?.Type?.ToLower() ?? "text";
            }
        }

        private void BodyOrTitleEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control 
                && e.Key == Key.S
                )
            {
                SaveBtn.Focus();
                DataManager.UpdateRecordData(MainDataContext.SelectedRecord);
                sender.CastTo<UIElement>().Focus();
                e.Handled = true;
            }
        }

        private void SaveRecordButton_Click(object sender, RoutedEventArgs e)
        {
            DataManager.UpdateRecordData(MainDataContext.SelectedRecord);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || BodyEditor == null)
            {
                return;
            }

            var content = (string)e.AddedItems[0] ?? "text";

            if (MainDataContext.SelectedRecord != null)
            {
                MainDataContext.SelectedRecord.Type = content;
            }
            
            BodyEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(content);
        }
    }
}
