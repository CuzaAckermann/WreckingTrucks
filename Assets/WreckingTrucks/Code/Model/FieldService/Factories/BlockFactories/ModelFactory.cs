public abstract class ModelFactory : IFactory<Model>
{
    private readonly Pool<Model> _modelPool;

    public ModelFactory(int initialPoolSize, int maxPoolCapacity)
    {
        _modelPool = new Pool<Model>(CreateModel,
                                     PrepareModel,
                                     ResetModel,
                                     DestroyModel,
                                     initialPoolSize,
                                     maxPoolCapacity);
    }

    public Model Create()
    {
        return _modelPool.GetElement();
    }

    public void Clear()
    {
        _modelPool.Clear();
    }

    #region Pool Callback
    protected abstract Model CreateModel();

    private void PrepareModel(Model model)
    {
        model.Destroyed += ReturnModel;
    }

    private void ResetModel(Model model)
    {
        model.Destroyed -= ReturnModel;
    }

    private void DestroyModel(Model model)
    {
        model?.Destroy();
    }
    
    private void ReturnModel(Model model)
    {
        if (model != null)
        {
            _modelPool.Release(model);
        }
    }
    #endregion
}