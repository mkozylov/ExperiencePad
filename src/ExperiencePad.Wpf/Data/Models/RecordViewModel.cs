using System;
using System.Collections.Generic;
using System.Text;

namespace ExperiencePad.Data
{
    public class RecordViewModel : ModelBase
    {
        public Guid Id
        {
            get { return _record.Id; }
            set
            {
                _record.Id = value;
                OnPropertyChanged();
            }
        }

        public Guid? CategoryId
        {
            get { return _record.CategoryId; }
            set
            {
                _record.CategoryId = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreateDate
        {
            get { return _record.CreateDate; }
            set
            {
                _record.CreateDate = value;
                OnPropertyChanged();
            }
        }

        public int Order
        {
            get { return _record.Order; }
            set
            {
                _record.Order = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return _record.Title; }
            set
            {
                _record.Title = value;
                OnPropertyChanged();
            }
        }

        public string Body
        {
            get { return _record.Body; }
            set
            {
                _record.Body = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get { return _record.Type; }
            set
            {
                _record.Type = value;
                OnPropertyChanged();
            }
        }


        private Record _record;

        public RecordViewModel(Record record)
        {
            _record = record;
        }

        public static implicit operator Record(RecordViewModel viewModel)
        {
            return viewModel._record;
        }

        public static implicit operator RecordViewModel(Record entity)
        {
            return new RecordViewModel(entity);
        }
    }
}
