using System;

public abstract class ModelFactory<M> : IModelCreator<M> where M : Model
{
    protected readonly ModelSettings ModelSettings;

    private Pool<M> _modelPool;

    public ModelFactory(FactorySettings factorySettings,
                        ModelSettings modelSettings)
    {
        if (factorySettings == null)
        {
            throw new ArgumentNullException(nameof(factorySettings));
        }

        ModelSettings = modelSettings ?? throw new ArgumentNullException(nameof(modelSettings));
    }

    public event Action<M> ModelCreated;

    public virtual M Create()
    {
        M model = _modelPool.GetElement();

        ModelCreated?.Invoke(model);

        return model;
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