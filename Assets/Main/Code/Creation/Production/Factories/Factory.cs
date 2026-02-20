using System;

public abstract class Factory
{
    private readonly FactorySettings _settings;

    private Pool<IDestroyable> _poolOfElements;

    public Factory(FactorySettings factorySettings)
    {
        Validator.ValidateNotNull(factorySettings);

        _settings = factorySettings;
    }

    public abstract Type GetCreatableType();

    public virtual IDestroyable Create()
    {
        if (_poolOfElements == null)
        {
            Logger.Log("Pool is empty");

            InitPool();
        }

        return _poolOfElements.GetElement();
    }

    public void Clear()
    {
        _poolOfElements.Clear();
    }

    protected abstract IDestroyable CreateElement();

    protected virtual void SubscribeToElement(IDestroyable element)
    {
        element.Destroyed += ReturnElement;
    }

    protected virtual void UnsubscribeFromElement(IDestroyable element)
    {
        element.Destroyed -= ReturnElement;
    }

    private void InitPool()
    {
        _poolOfElements = new Pool<IDestroyable>(CreateElement,
                                                 SubscribeToElement,
                                                 UnsubscribeFromElement,
                                                 DestroyElement,
                                                 _settings.InitialPoolSize,
                                                 _settings.MaxPoolCapacity);
    }

    private void DestroyElement(IDestroyable element)
    {
        element.Destroy();
    }

    private void ReturnElement(IDestroyable element)
    {
        _poolOfElements.Release(element);
    }
}