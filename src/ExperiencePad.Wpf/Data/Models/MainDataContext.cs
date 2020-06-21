using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExperiencePad.Data
{
    public class MainDataContext : ModelBase
    {
        public IServiceProvider Injector { get; set; }

        public ObservableCollection<CategoryViewModel> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RecordViewModel> Records
        {
            get { return _records; }
            set
            {
                _records = value;
                OnPropertyChanged();
            }
        }

        public CategoryViewModel SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public RecordViewModel SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                if (value != null 
                    && value.Title == null
                    && value.Body == null)
                {
                    _draftRecord = value;
                }

                _selectedRecord = value ?? _draftRecord;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CategoryViewModel> _categories = new ObservableCollection<CategoryViewModel>();
        private ObservableCollection<RecordViewModel> _records = new ObservableCollection<RecordViewModel>();
        private CategoryViewModel _selectedCategory;
        private RecordViewModel _selectedRecord;
        private RecordViewModel _draftRecord = new Record();

        public MainDataContext(IServiceProvider injector)
        {
            Injector = injector;

            _selectedRecord = _draftRecord;
        }
    }
}
