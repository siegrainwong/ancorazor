namespace Blog.Model.Mapping
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
