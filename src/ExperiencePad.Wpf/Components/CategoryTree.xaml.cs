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
using NWrath.Synergy.Common.Extensions;

namespace ExperiencePad
{
    /// <summary>
    /// Логика взаимодействия для CategoryTree.xaml
    /// </summary>
    public partial class CategoryTree : TreeView
    {
        public bool IsEmpty 
        {
            get 
            {
                return ItemsSource == null || !ItemsSource.GetEnumerator().MoveNext();
            }
        }

        public event EventHandler<RoutedEventArgs> ContextMenuItemClick;

        public CategoryTree()
        {
            InitializeComponent();
        }

        #region Internal

        private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ContextMenuItemClick?.Invoke(sender, e);
        }

        #endregion

        private void TreeItemElement_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var category = (CategoryViewModel)element.DataContext;

            category.IsMouseOver = (bool)e.NewValue;
        }
    }
}
