namespace UrlShortener.Services.Contracts
{
    public interface ICacheService
    {
        void PutItem<T>(string key, T item);
        T FindItem<T>(string key);
    }
}