using System;

public class JellyShaker : ITickable, IAbility
{
    private readonly EventBus _eventBus;
    private readonly PresenterProduction _presenterProduction;
    private readonly JellyLockedStorage _jellyStorage;

    public JellyShaker(EventBus eventBus, PresenterProduction presenterProduction, int capacity)
    {
        Validator.ValidateNotNull(eventBus, presenterProduction);
        Validator.ValidateMin(capacity, 0, true);

        _eventBus = eventBus;
        _presenterProduction = presenterProduction;

        _jellyStorage = new JellyLockedStorage(capacity);
    }

    public event Action<IDestroyable> Destroyed;

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void Tick(float deltaTime)
    {
        if (_jellyStorage.HasActive() == false)
        {
            return;
        }

        _jellyStorage.Lock();

        foreach (Jelly jelly in _jellyStorage.GetClearedActive())
        {
            jelly.Shake(deltaTime);
        }

        _jellyStorage.Unlock();
    }

    public void Start()
    {
        _presenterProduction.Created += AddJelly;

        Activated?.Invoke(this);
    }

    public void Finish()
    {
        _presenterProduction.Created -= AddJelly;

        Deactivated?.Invoke(this);
    }

    private void AddJelly(Presenter presenter)
    {
        if (presenter is not BlockPresenter blockPresenter)
        {
            return;
        }

        _jellyStorage.Register(blockPresenter.Jelly);
    }
}