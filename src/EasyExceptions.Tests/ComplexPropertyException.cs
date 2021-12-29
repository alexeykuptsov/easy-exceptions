using System;
using System.Collections.Generic;

namespace EasyExceptions.Tests
{
    public class ComplexPropertyException : Exception
    {
        public ComplexPropertyException() : base("Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        {
        }

        public List<EntityValidationError> EntityValidationErrors => new List<EntityValidationError>()
        {
            new EntityValidationError(),
        };

        public class EntityValidationError
        {
            public Entry Entry { get; } = new Entry();
        }
        
        public class Entry
        {
            public Entity Entity { get; } = new Entity();

            public List<ValidationError> ValidationErrors { get; } = new List<ValidationError>()
            {
                new ValidationError(),
            };
        }

        public class Entity
        {
            public Guid Id { get; } = Guid.Parse("240b10f4-11dc-4e75-b268-da922fa6d781");
        }
        
        public class ValidationError
        {
            public string PropertyName { get; } = "Author";
            public string ErrorMessage { get; } = "The Author field is required.";
        }
    }
}