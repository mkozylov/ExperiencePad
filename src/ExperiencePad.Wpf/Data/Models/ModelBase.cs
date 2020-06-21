using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExperiencePad.Data
{
    public abstract class ModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public string ValidationError 
        { 
            get 
            { 
                var error = ValidateModel();

                OnPropertyChanged(); 

                return error; 
            } 
        }

        string IDataErrorInfo.Error => null;

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var error = ValidateProperty(columnName);

                return error;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected Dictionary<string, PropertyInfo> modelProperties;

        public ModelBase()
        {
            var type = GetType();

            modelProperties = GetTypeProperties(type);
        }

        private Dictionary<string, PropertyInfo> GetTypeProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                 .OrderByDescending(x => x.DeclaringType == type)
                                 .Distinct(new PropertyNameEqualityComparer())
                                 .ToDictionary(k => k.Name, v => v);

            return properties;
        }

        public virtual string ValidateProperty(string columnName)
        {
            if (!modelProperties.ContainsKey(columnName))
            {
                return null;
            }

            var validationResults = new List<ValidationResult>();

            Validator.TryValidateProperty(
                modelProperties[columnName].GetValue(this),
                new ValidationContext(this) { MemberName = columnName },
                validationResults
                );

            var errorMessage = validationResults.Count > 0 
                                  ? validationResults[0].ErrorMessage 
                                  : null;

            return errorMessage;
        }

        #region Internal

        protected virtual string ValidateModel()
        {
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);

            var errorStr = default(string);

            if (validationResults.Count > 0)
            {
                errorStr = validationResults.Select(x =>
                                            {
                                                var propDisplayName = modelProperties[x.MemberNames.First()].GetDisplayName();
                                                return $"{propDisplayName} - {x.ErrorMessage}";
                                            })
                                            .StringJoin("\n");
            }

            return errorStr;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var pn in propertyNames)
            {
                OnPropertyChanged(pn);
            }
        } 

        #endregion
    }
}
