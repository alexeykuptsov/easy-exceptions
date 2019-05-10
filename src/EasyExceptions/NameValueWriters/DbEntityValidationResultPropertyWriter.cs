using System.Text;
using EasyExceptions.WritingRules;

namespace EasyExceptions.NameValueWriters
{
    public class DbEntityValidationResultPropertyWriter : ReflectionPropertyWriterBase
    {
        protected override string GetFullTypeName()
        {
            return "System.Data.Entity.Validation.DbEntityValidationResult";
        }

        public override void Write(StringBuilder resultBuilder, string name, object value)
        {
            new SimplePropertyWriter().Write(resultBuilder, name, value);
            var entryProperty = TryGetSingleReadablePropertyWithoutParameters(value, "Entry");
            if (entryProperty != null)
            {
                var entry = entryProperty.GetValue(value, new object[0]);
                var entityProperty = TryGetSingleReadablePropertyWithoutParameters(entry, "Entity");
                if (entityProperty != null)
                {
                    var entity = entityProperty.GetValue(entry, new object[0]);
                    RegularPropertiesRule.WriteNameValue(resultBuilder, name + ".Entry.Entity", entity);
                    var idProperty = TryGetSingleReadablePropertyWithoutParameters(entity, "Id");
                    if (idProperty != null)
                    {
                        var id = idProperty.GetValue(entity, new object[0]);
                        RegularPropertiesRule.WriteNameValue(resultBuilder, name + ".Entry.Entity.Id", id);
                    }
                }
            }
            WritePropertyValue(resultBuilder, name, value, "ValidationErrors");
        }
    }
}