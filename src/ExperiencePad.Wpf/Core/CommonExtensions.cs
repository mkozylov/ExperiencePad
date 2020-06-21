using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExperiencePad
{
    public static class CommonExtensions
    {
        public static string GetDisplayName(this PropertyInfo prop)
        {
            return prop?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName 
                   ?? prop.Name;
        }

        public static string GetDisplayName(this object obj)
        {
            return obj?.GetType() ?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                   ?? obj.ToString();
        }

    }
}
