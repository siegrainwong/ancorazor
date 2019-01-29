using Blog.Model.Base;

namespace Blog.Model.Mapping
{
    public interface IPropertyMappingContainer
    {
        void Register<T>() where T : IPropertyMapping, new();
        IPropertyMapping Resolve<TSource, TDestination>() where TDestination : BaseModel;
        bool ValidateMappingExistsFor<TSource, TDestination>(string fields) where TDestination : BaseModel;
    }
}