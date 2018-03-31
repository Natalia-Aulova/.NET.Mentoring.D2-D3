using System;

namespace IQueryableExample.Services.E3SClient
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class E3SMetaTypeAttribute : Attribute
    {
        public string Name { get; }

        public E3SMetaTypeAttribute(string name)
        {
            Name = name;
        }
    }
}
