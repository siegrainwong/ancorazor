using System.Collections.Generic;
using Blog.Model.Base;

namespace Blog.Common.Services
{
    public abstract class PropertyMapping<TSource, TDestination> : IPropertyMapping 
        where TDestination : BaseModel
    {
        public Dictionary<string, List<MappedProperty>> MappingDictionary { get; }

        protected PropertyMapping(Dictionary<string, List<MappedProperty>> mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
            MappingDictionary[nameof(BaseModel.Id)] = new List<MappedProperty>
            {
                new MappedProperty { Name = nameof(BaseModel.Id), Revert = false}
            };
        }
    }
}
