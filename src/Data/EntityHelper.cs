using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Talaryon.Data
{
    public static class EntityHelper
    {
        public static string GetTableName<T>()
        {
            if ((typeof(T).GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault()! as TableAttribute) is null)
                throw new Exception($"Type {typeof(T).Name} has no [Table] attribute.");
                    
            return typeof(T)
                .GetCustomAttribute<TableAttribute>()?
                .Name;
        }
        
        public static string GetKeyValue<T>(T entity)
        {
            return (string)typeof(T)
                .GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() is not null)?
                .GetValue(entity);
        }
        
        public static string GetKeyName<T>()
        {
            return (string)typeof(T)
                .GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() is not null)?
                .Name;
        }
    }
}