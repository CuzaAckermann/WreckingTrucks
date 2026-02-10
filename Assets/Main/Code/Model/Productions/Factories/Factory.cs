using System;

public abstract class Factory<T> : ICreator<T> where T : class, IDestroyable
{
    protected FactorySettings FactorySettings;

    private Pool<T> _poolOfElements;

    public Factory(FactorySettings factorySettings)
    {
        if (factorySettings == null)
        {
            throw new ArgumentNullException(nameof(factorySettings));
        }

        FactorySettings = factorySettings ?? throw new ArgumentNullException(nameof(factorySettings));
    }

    public event Action<T> Created;

    public virtual T Create()
    {
        T element = _poolOfElements.GetElement();

        Created?.Invoke(element);

        return element;
    }

    public void Clear()
    {
        _poolOfElements.Clear();
    }

    protected void InitPool(int initialPoolSize, int maxPoolCapacity)
    {
        _poolOfElements = new Pool<T>(CreateElement,
                                      SubscribeToElement,
                                      UnsubscribeFromElement,
                                      DestroyElement,
                                      initialPoolSize,
                                      maxPoolCapacity);
    }

    protected abstract T CreateElement();

    private void SubscribeToElement(T element)
    {
        element.Destroyed += ReturnElement;
    }

    private void UnsubscribeFromElement(T element)
    {
        element.Destroyed -= ReturnElement;
    }

    private void DestroyElement(T element)
    {
        element.Destroy();
    }

    private void ReturnElement(IDestroyable element)
    {
        _poolOfElements.Release((T)element);
    }
}