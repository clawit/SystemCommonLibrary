using SystemCommonLibrary.IoC.Models;
using System;

namespace SystemCommonLibrary.IoC.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class FilterAttribute : Attribute
    {
        public Lifetime DependencyInjectionMode
        {
            get;
        } = Lifetime.Scoped;

        public Type ImplementInterface
        {
            get;
        } = null;

        public FilterAttribute()
        {
        }


        public FilterAttribute(Lifetime dependencyInjectionMode)
        {
            this.DependencyInjectionMode = dependencyInjectionMode;
        }
        public FilterAttribute(Type implementInterface)
        {
            this.ImplementInterface = implementInterface;
        }
        public FilterAttribute(Lifetime dependencyInjectionMode, Type implementInterface)
        {
            this.DependencyInjectionMode = dependencyInjectionMode;
            this.ImplementInterface = implementInterface;
        }
    }
}
