using System.Collections.Generic;

namespace Blog.Model.Mapping
{
    public interface IPropertyMapping
    {
        Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}