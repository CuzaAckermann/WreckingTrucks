public abstract class ModelFactory<M> where M : Model
{
    private readonly Pool<M> _modelPool;

    public ModelFactory(int initialPoolSize, int maxPoolCapacity)
    {
        _modelPool = new Pool<M>(CreateModel,
                                 PrepareModel,
                                 ResetModel,
                                 DestroyModel,
                                 initialPoolSize,
                                 maxPoolCapacity);
    }

    public M Create()
    {
        return _modelPool.GetElement();
    }

    public void Clear()
    {
        _modelPool.Clear();
    }

    protected abstract M CreateModel();

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
        model?.Destroy();
    }
    
    private void ReturnModel(Model model)
    {
        if (model != null)
        {
            _modelPool.Release((M)model);
        }
    }
}