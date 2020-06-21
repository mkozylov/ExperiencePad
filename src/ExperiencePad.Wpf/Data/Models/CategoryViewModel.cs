using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ExperiencePad.Data
{
    public class CategoryViewModel : ModelBase
    {
        public Guid Id 
        {
            get { return _category.Id; }
            set 
            {
                _category.Id = value;
                OnPropertyChanged();
            }
        }

        public Guid? ParentId
        {
            get { return _category.ParentId; }
            set
            {
                _category.ParentId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _category.Name; }
            set
            {
                _category.Name = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreateDate
        {
            get { return _category.CreateDate; }
            set
            {
                _category.CreateDate = value;
                OnPropertyChanged();
            }
        }

        public int Order
        {
            get { return _category.Order; }
            set
            {
                _category.Order = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public CategoryViewModel Parent
        {
            get { return _category.Parent; }
            set
            {
                _category.Parent = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryViewModel> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }

        private Category _category;
        private bool _isSelected;
        private ObservableCollection<CategoryViewModel> _children;

        public CategoryViewModel(Category category)
        {
            _category = category;

            var childrenCollection = _category.Children
                                              .Select(x => (CategoryViewModel)x);

            _children = new ObservableCollection<CategoryViewModel>(childrenCollection);
        }

        public static implicit operator Category(CategoryViewModel viewModel)
        {
            if (viewModel == null)
            {
                return null;
            }

            viewModel._category.Children = viewModel.Children
                                                    .Select(x => (Category)x)
                                                    .ToList();

            return viewModel._category;
        }

        public static implicit operator CategoryViewModel(Category entity)
        {
            return new CategoryViewModel(entity);
        }
    }
}
