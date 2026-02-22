public interface IStorage<S> // S - Storagable
{
    public T Get<T>() where T : S;

    public bool TryGet<T>(out T required) where T : S;
}