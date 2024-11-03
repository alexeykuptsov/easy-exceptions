using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.TypeInspectors
{
    /// <summary>
    /// Returns the properties of a type that are readable.
    /// </summary>
    public sealed class ReadablePropertiesTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeResolver typeResolver;
        private readonly bool includeNonPublicProperties;
        private readonly Func<Exception, object, string, string>? exceptionHandler;

        public ReadablePropertiesTypeInspector(
            ITypeResolver typeResolver,
            bool includeNonPublicProperties,
            Func<Exception, object, string, string>? exceptionHandler = null)
        {
            this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
            this.includeNonPublicProperties = includeNonPublicProperties;
            this.exceptionHandler = exceptionHandler;
        }

        private static bool IsValidProperty(PropertyInfo property)
        {
            return property.CanRead
                && property.GetGetMethod(true)!.GetParameters().Length == 0;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
        {
            return type
                .GetProperties(includeNonPublicProperties)
                .Where(IsValidProperty)
                .Select(p => (IPropertyDescriptor)new ReflectionPropertyDescriptor(p, typeResolver, exceptionHandler));
        }

        private sealed class ReflectionPropertyDescriptor : IPropertyDescriptor
        {
            private readonly PropertyInfo propertyInfo;
            private readonly ITypeResolver typeResolver;
            private readonly Func<Exception, object, string, string>? exceptionHandler;

            public ReflectionPropertyDescriptor(
                PropertyInfo propertyInfo,
                ITypeResolver typeResolver,
                Func<Exception, object, string, string>? exceptionHandler)
            {
                this.propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
                this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
                this.exceptionHandler = exceptionHandler;
                ScalarStyle = ScalarStyle.Any;
            }

            public string Name => propertyInfo.Name;
            public Type Type => propertyInfo.PropertyType;
            public Type? TypeOverride { get; set; }
            public int Order { get; set; }
            public bool CanWrite => propertyInfo.CanWrite;
            public ScalarStyle ScalarStyle { get; set; }

            public void Write(object target, object? value)
            {
                propertyInfo.SetValue(target, value, null);
            }

            public T? GetCustomAttribute<T>() where T : Attribute
            {
                var attributes = propertyInfo.GetAllCustomAttributes<T>();
                return (T?)attributes.FirstOrDefault();
            }

            public IObjectDescriptor Read(object target)
            {
                object? propertyValue;
                if (exceptionHandler != null)
                {
                    try
                    {
                        propertyValue = propertyInfo.ReadValue(target);
                    }
                    catch (TargetInvocationException e)
                    {
                        return new ObjectDescriptor(
                            exceptionHandler(e.InnerException!, target, propertyInfo.Name),
                            typeof(string),
                            typeof(string),
                            ScalarStyle.Any);
                    }
                }
                else
                {
                    propertyValue = propertyInfo.ReadValue(target);
                }

                var actualType = TypeOverride ?? typeResolver.Resolve(Type, propertyValue);
                return new ObjectDescriptor(propertyValue, actualType, Type, ScalarStyle);
            }
        }
    }
}
