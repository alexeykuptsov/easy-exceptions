using System;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    public sealed class PropertyDescriptor : IPropertyDescriptor
    {
        private readonly IPropertyDescriptor baseDescriptor;

        public PropertyDescriptor(IPropertyDescriptor baseDescriptor)
        {
            this.baseDescriptor = baseDescriptor;
            Name = baseDescriptor.Name;
        }

        public string Name { get; set; }

        public Type Type => baseDescriptor.Type;

        public Type? TypeOverride
        {
            get => baseDescriptor.TypeOverride;
            set => baseDescriptor.TypeOverride = value;
        }

        public int Order { get; set; }

        public ScalarStyle ScalarStyle
        {
            get => baseDescriptor.ScalarStyle;
            set => baseDescriptor.ScalarStyle = value;
        }

        public bool CanWrite => baseDescriptor.CanWrite;

        public Func<Exception, object, string, string>? ExceptionHandler { get; set; }

        public void Write(object target, object? value)
        {
            baseDescriptor.Write(target, value);
        }

        public T? GetCustomAttribute<T>() where T : Attribute
        {
            return baseDescriptor.GetCustomAttribute<T>();
        }

        public IObjectDescriptor Read(object target)
        {
            if (ExceptionHandler == null)
            {
                return baseDescriptor.Read(target);
            }

            try
            {
                return baseDescriptor.Read(target);
            }
            catch (Exception e)
            {
                return new ObjectDescriptor(
                    ExceptionHandler(e, target, Name),
                    typeof(string),
                    typeof(string));
            }
        }
    }
}
