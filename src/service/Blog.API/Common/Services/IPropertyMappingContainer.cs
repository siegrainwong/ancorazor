using Blog.Model.Base;

namespace Blog.Common.Services
{
    public interface IPropertyMappingContainer
    {
        void Register<T>() where T : IPropertyMapping, new();
        IPropertyMapping Resolve<TSource, TDestination>() where TDestination : BaseModel;
        bool ValidateMappingExistsFor<TSource, TDestination>(string fields) where TDestination : BaseModel;
    }
}