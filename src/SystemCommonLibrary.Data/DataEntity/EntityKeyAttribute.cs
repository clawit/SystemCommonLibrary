using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EntityKeyAttribute : Attribute
    {
        public EntityKeyAttribute()
        { 
        
        }
    }
}
