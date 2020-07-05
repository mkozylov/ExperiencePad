using Newtonsoft.Json;
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
            get { return _id; }
            set 
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public Guid? ParentId
        {
            get { return _parentId; }
            set
            {
                _parentId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                _createDate = value;
                OnPropertyChanged();
            }
        }

        public int Order
        {
            get { return _order; }
            set
            {
                _order = value;
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

        public bool IsMouseOver
        {
            get { return _isMouseOver; }
            set
            {
                _isMouseOver = value;
                OnPropertyChanged();
            }
        }

        public CategoryViewModel Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
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

        private Guid _id;
        private Guid? _parentId;
        private string _name;
        private DateTime _createDate;
        private int _order;
        private bool _isSelected;
        private bool _isMouseOver;
        private CategoryViewModel _parent;
        private ObservableCollection<CategoryViewModel> _children = new ObservableCollection<CategoryViewModel>();


        public static implicit operator Category(CategoryViewModel viewModel)
        {
            if (viewModel == null)
            {
                return null;
            }

            return viewModel.DeepMap<Category>();
        }

        public static implicit operator CategoryViewModel(Category entity)
        {
            if (entity == null)
            {
                return null;
            }

            return entity.DeepMap<CategoryViewModel>();
        }
    }
}
