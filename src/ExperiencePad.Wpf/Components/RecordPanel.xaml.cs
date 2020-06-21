using ExperiencePad.Data;
using ICSharpCode.AvalonEdit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
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
        public RecordPanel()
        {
            InitializeComponent();

            Loaded += RecordPanel_Loaded;
        }

        private void RecordPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;

            dataContext.Injector.BindProperties(this);
        }

        private void RecordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;

            if (e.AddedItems.Count == 0)
            {
                dataContext.SelectedRecord = null;
                return;
            }

            var selectedRecord = (RecordViewModel)e.AddedItems[0];

            dataContext.SelectedRecord = selectedRecord;
        }
    }
}
