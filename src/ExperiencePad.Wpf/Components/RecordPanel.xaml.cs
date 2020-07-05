using ExperiencePad.Data;
using ExperiencePad.Logic;
using ICSharpCode.AvalonEdit;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NWrath.Synergy.Common.Extensions;
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

namespace ExperiencePad
{
    /// <summary>
    /// Логика взаимодействия для RecordPanel.xaml
    /// </summary>
    public partial class RecordPanel : UserControl
    {
        public MainDataContext MainDataContext { get; set; }

        public DataManager DataManager { get; set; }

        public RecordPanel()
        {
            InitializeComponent();

            Loaded += RecordPanel_Loaded;
        }

        #region Internal

        private void RecordPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;

            dataContext.Injector.BindProperties(this);

            RecordList.ContextMenuItemClick += RecordList_ContextMenuItemClick;
        }

        private void RecordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;

            if (e.AddedItems.Count == 0)
            {
                return;
            }

            var selectedRecord = (RecordViewModel)e.AddedItems[0];

            MainDataContext.SelectedRecord.If(p => p != null, t => t.IsSelected = false);
            dataContext.SelectedRecord = selectedRecord;
            selectedRecord.IsSelected = true;
        }

        private void AddRecordButton_Click(object sender, RoutedEventArgs e)
        {
            var newRecord = new RecordViewModel
            {
                CategoryId = MainDataContext.SelectedCategory?.Id
            };

            DataManager.AddRecord(newRecord);

            MainDataContext.Records.Add(newRecord);

            MainDataContext.SelectedRecord = newRecord;
            newRecord.IsSelected = true;
            RecordList.SelectedItem = newRecord;
        }

        private void RecordList_ContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var menuItemName = menuItem.Name.ToLower();
            var menuItemRecord = (RecordViewModel)menuItem.DataContext;

            MainDataContext.SelectedDialogRecord = menuItemRecord;

            switch (menuItemName)
            {
                case "delete":
                    App.Current.MainWindow.ShowDialog(
                        Resources["DeleteRecordDialog"],
                        DeleteRecordDialog_Closing
                        );
                    break;
                default:
                    MainDataContext.SelectedDialogRecord = null;
                    throw new ArgumentException($"Команда меню записей '{menuItemName}' не найдена");
            }
        }

        private void DeleteRecordDialog_Closing(object sender, DialogClosingEventArgs e)
        {
            var record = (RecordViewModel)e.Parameter;

            MainDataContext.SelectedDialogRecord = null;

            if (record == null)
            {
                return;
            }

            DataManager.DeleteRecord(record);
            MainDataContext.Records.Remove(record);

            if (MainDataContext.SelectedRecord == record)
            {
                MainDataContext.SelectedRecord = null;
            }
        } 

        #endregion
    }
}
