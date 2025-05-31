public interface ITickEngineUpdaterOnlyAddAndRemove
{
    public void Add(ITickable tickable);

    public void Remove(ITickable tickable);
}