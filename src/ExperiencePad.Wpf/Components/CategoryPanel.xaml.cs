using ExperiencePad.Data;
using ExperiencePad.Logic;
using MaterialDesignThemes.Wpf;
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
        public MainDataContext MainDataContext { get; set; }

        public DataManager DataManager { get; set; }

        public CategoryPanel()
        {
            InitializeComponent();

            Loaded += CategoryPanel_Loaded;
        }

        #region Internal

        private void CategoryPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = (MainDataContext)DataContext;

            dataContext.Injector.BindProperties(this);

            CategoryTree.ContextMenuItemClick += CategoryTree_ContextMenuItemClick;

            var categoryTree = DataManager.GetCategoryTree();

            dataContext.Categories = new ObservableCollection<CategoryViewModel>(categoryTree);
        }

        private void CategoryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var category = (CategoryViewModel)e.NewValue;
            var records = DataManager.GetCategoryRecords(category.Id);

            MainDataContext.SelectedCategory.If(p => p != null, t => t.IsSelected = false);

            MainDataContext.SelectedCategory = category;
            category.IsSelected = true;
            MainDataContext.Records = new ObservableCollection<RecordViewModel>(records);

            if (MainDataContext.SelectedRecord != null)
            {
                var selectedRecord = MainDataContext.Records
                                                    .FirstOrDefault(x => x.Id == MainDataContext.SelectedRecord.Id);

                if (selectedRecord != null)
                {
                    selectedRecord.CategoryId = MainDataContext.SelectedRecord.CategoryId;
                    selectedRecord.Title = MainDataContext.SelectedRecord.Title;
                    selectedRecord.Body = MainDataContext.SelectedRecord.Body;
                    selectedRecord.Type = MainDataContext.SelectedRecord.Type;
                    selectedRecord.Order = MainDataContext.SelectedRecord.Order;
                    selectedRecord.IsSelected = MainDataContext.SelectedRecord.IsSelected;
                }
            }
        }

        private void CategoryTree_ContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var menuItemName = menuItem.Name.ToLower();
            var menuItemCategory = (CategoryViewModel)menuItem.DataContext;

            MainDataContext.SelectedDialogCategory = menuItemCategory;

            switch (menuItemName)
            {
                case "new":
                    var newCategory = new CategoryViewModel
                    {
                        Parent = menuItemCategory,
                        ParentId = menuItemCategory.Id
                    };
                  
                    ShowNewCategoryDialog(newCategory);
                    break;
                case "rename":
                    App.Current.MainWindow.ShowDialog(
                        Resources["RenameCategoryDialog"],
                        RenameCategoryDialog_Closing
                        );
                    break;
                case "delete":
                    App.Current.MainWindow.ShowDialog(
                        Resources["DeleteCategoryDialog"],
                        DeleteCategoryDialog_Closing
                        );
                    break;
                default:
                    MainDataContext.SelectedDialogCategory = null;
                    throw new ArgumentException($"Команда меню категорий '{menuItemName}' не найдена");
            }
        }

        private void ShowNewCategoryDialog(CategoryViewModel newCategory)
        {
            MainDataContext.SelectedDialogCategory = newCategory;

            App.Current.MainWindow.ShowDialog(
                Resources["NewCategoryDialog"],
                NewCategoryDialog_Closing
                );
        }

        private void NewCategoryDialog_Closing(object sender, DialogClosingEventArgs e)
        {
            var category = (CategoryViewModel)e.Parameter;

            MainDataContext.SelectedDialogCategory = null;

            if (category == null)
            {
                return;
            }

            DataManager.AddCategory(category);

            if (category.Parent == null)
            {
                MainDataContext.Categories.Add(category);
            }
            else
            {
                category.Parent
                        .Children
                        .Add(category);
            }
        }
        private void RenameCategoryDialog_Closing(object sender, DialogClosingEventArgs e)
        {
            var category = (CategoryViewModel)e.Parameter;

            MainDataContext.SelectedDialogCategory = null;

            if (category == null)
            {
                return;
            }

            DataManager.RenameCategory(category, category.Name);
        }


        private void DeleteCategoryDialog_Closing(object sender, DialogClosingEventArgs e)
        {
            var category = (CategoryViewModel)e.Parameter;

            MainDataContext.SelectedDialogCategory = null;

            if (category == null)
            {
                return;
            }

            DataManager.DeleteCategory(category);

            if (category.Parent == null)
            {
                MainDataContext.Categories.Remove(category);
            }
            else
            {
                category.Parent
                        .Children
                        .Remove(category);
            }

            if (MainDataContext.SelectedCategory == category)
            {
                MainDataContext.SelectedCategory = null;
            }
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var newCategory = new CategoryViewModel();

            ShowNewCategoryDialog(newCategory);
        }

        #endregion
    }
}
