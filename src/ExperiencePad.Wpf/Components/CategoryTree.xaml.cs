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

        private void CategoryTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;
            var oldCategory = dataContext.SelectedCategory ?? new Category();
            var newCategory = sender.CastTo<TextBox>()
                                    .DataContext
                                    .CastTo<CategoryViewModel>();

            newCategory.IsSelected = true;

            //OnSelectedItemChanged(
            //    new RoutedPropertyChangedEventArgs<object>(oldCategory, newCategory, SelectedItemChangedEvent)
            //    );
        }

        #endregion
    }
}
