using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DatingDiary.Model
{
    public class EntityHelpers<T>
    {

        public static void GenericDetach<T>(T entity)
        {
            foreach (PropertyInfo pi in entity.GetType().GetProperties())
            {
                if (pi.GetCustomAttributes(typeof(System.Data.Linq.Mapping.AssociationAttribute), false).Length > 0)
                {
                    // Property is associated to another entity
                    Type propType = pi.PropertyType;
                    // Invoke Empty contructor (set to default value)
                    ConstructorInfo ci = propType.GetConstructor(new Type[0]);
                    pi.SetValue(entity, ci.Invoke(null), null);
                }
            }
        }
    }
}
