namespace Vulcan.DapperExtensions
{
    public interface IRuntimeContextStorage
    {
        object Get(string key);

        void Set(string key, object item);

        void Remove(string key);


        bool ContainsKey(string key);
    }

}
