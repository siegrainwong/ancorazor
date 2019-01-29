using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Model.Base;

namespace Blog.Model.Mapping
{
    public class PropertyMappingContainer : IPropertyMappingContainer
    {
        protected internal readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

        public void Register<T>() where T : IPropertyMapping, new()
        {
            if (PropertyMappings.All(x => x.GetType() != typeof(T)))
            {
                PropertyMappings.Add(new T());
            }
        }

        public IPropertyMapping Resolve<TSource, TDestination>() where TDestination : BaseModel
        {
            var matchingMapping = PropertyMappings.OfType<PropertyMapping<TSource, TDestination>>().ToList();
            if (matchingMapping.Count == 1)
            {
                return matchingMapping.First();
            }

            throw new Exception($"Cannot find property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidateMappingExistsFor<TSource, TDestination>(string fields) where TDestination : BaseModel
        {
            var propertyMapping = Resolve<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    continue;
                }
                if (!propertyMapping.MappingDictionary.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
