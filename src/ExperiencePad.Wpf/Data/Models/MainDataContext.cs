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

        public ObservableCollection<string> RecordTypes { get; set; } = new ObservableCollection<string>(new[] 
        {
            "text",
            "c#",
            "sql",
            "xml"
        });

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

        public CategoryViewModel SelectedDialogCategory
        {
            get { return _selectedDialogCategory; }
            set
            {
                _selectedDialogCategory = value;
                OnPropertyChanged();
            }
        }

        public RecordViewModel SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                OnPropertyChanged();
            }
        }

        public RecordViewModel SelectedDialogRecord
        {
            get { return _selectedDialogRecord; }
            set
            {
                _selectedDialogRecord = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CategoryViewModel> _categories = new ObservableCollection<CategoryViewModel>();
        private ObservableCollection<RecordViewModel> _records = new ObservableCollection<RecordViewModel>();
        private CategoryViewModel _selectedCategory;
        private CategoryViewModel _selectedDialogCategory;
        private RecordViewModel _selectedRecord;
        private RecordViewModel _selectedDialogRecord;

        public MainDataContext(IServiceProvider injector)
        {
            Injector = injector;
        }
    }
}
