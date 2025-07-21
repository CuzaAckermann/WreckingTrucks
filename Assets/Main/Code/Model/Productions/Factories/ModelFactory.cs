public abstract class ModelFactory<M> : IModelCreator<M> where M : Model
{
    private Pool<M> _modelPool;
    
    public M Create()
    {
        return _modelPool.GetElement();
    }

    public void Clear()
    {
        _modelPool.Clear();
    }

    protected void InitializePool(int initialPoolSize, int maxPoolCapacity)
    {
        _modelPool = new Pool<M>(CreateElement,
                                 PrepareModel,
                                 ResetModel,
                                 DestroyModel,
                                 initialPoolSize,
                                 maxPoolCapacity);
    }

    protected abstract M CreateElement();

    private void PrepareModel(M model)
    {
        model.Destroyed += ReturnModel;
    }

    private void ResetModel(M model)
    {
        model.Destroyed -= ReturnModel;
    }

    private void DestroyModel(M model)
    {
        model.Destroy();
    }
    
    private void ReturnModel(Model model)
    {
        _modelPool.Release((M)model);
    }
}