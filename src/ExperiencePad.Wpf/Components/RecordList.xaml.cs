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
using ExperiencePad.Data;
using System.Collections.ObjectModel;

namespace ExperiencePad
{
    /// <summary>
    /// Логика взаимодействия для RecordList.xaml
    /// </summary>
    public partial class RecordList : ListView
    {
        public event EventHandler<RoutedEventArgs> ContextMenuItemClick;

        public RecordList()
        {
            InitializeComponent();
        }

        #region Internal

        private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ContextMenuItemClick?.Invoke(sender, e);
        } 

        #endregion
    }
}
