using System;
using System.Collections.Generic;
using System.Text;

namespace ExperiencePad.Data
{
    public class RecordViewModel : ModelBase
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

        public Guid? CategoryId
        {
            get { return _categoryId; }
            set
            {
                _categoryId = value;
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

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
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

        private Guid _id;
        private Guid? _categoryId;
        private string _title;
        private string _body;
        private DateTime _createDate;
        private int _order;
        private string _type = "text";
        private bool _isSelected;

        public static implicit operator Record(RecordViewModel viewModel)
        {
            if (viewModel == null)
            {
                return null;
            }

            return viewModel.DeepMap<Record>();
        }

        public static implicit operator RecordViewModel(Record entity)
        {
            if (entity == null)
            {
                return null;
            }

            return entity.DeepMap<RecordViewModel>();
        }
    }
}
