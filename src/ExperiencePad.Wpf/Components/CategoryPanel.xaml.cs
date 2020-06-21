using ExperiencePad.Data;
using ExperiencePad.Logic;
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
    /// Логика взаимодействия для CategoryPanel.xaml
    /// </summary>
    public partial class CategoryPanel : UserControl
    {
        public DataManager DataManager { get; set; }

        public CategoryPanel()
        {
            InitializeComponent();

            Loaded += CategoryPanel_Loaded;
        }

        private void CategoryPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;

            dataContext.Injector.BindProperties(this);

            CategoryTree.ContextMenuItemClick += CategoryTree_ContextMenuItemClick;

            var categoryTree = DataManager.GetCategoryTree();

            dataContext.Categories = new ObservableCollection<CategoryViewModel>(categoryTree);
        }

        private void CategoryTree_ContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;
            var menuItem = (MenuItem)sender;
            var menuItemName = menuItem.Name.ToLower();
            var parentCategory = (CategoryViewModel)menuItem.DataContext;

            switch (menuItemName)
            {
                case "new":
                    var newCategory = new CategoryViewModel(new Category
                    {
                        Parent = parentCategory,
                        ParentId = parentCategory.Id,
                        Order = parentCategory.Children.Count + 1
                    });
                    DataManager.AddCategory(newCategory);
                    parentCategory.Children.Add(newCategory);
                    break;
                case "rename":
                    break;
                case "delete":
                    break;
                default:
                    return;
            }
        }

        private void CategoryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var dataContext = (MainDataContext)DataContext;

            var category = (CategoryViewModel)e.NewValue;
            var records = DataManager.GetCategoryRecords(category.Id);

            dataContext.SelectedCategory.If(p => p != null, t => t.IsSelected = false);

            dataContext.SelectedCategory = category;
            category.IsSelected = true;
            dataContext.Records = new ObservableCollection<RecordViewModel>(records);
        }
    }
}
