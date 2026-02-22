public class ModelFinalizer : IApplicationAbility
{
    private readonly EventBus _eventBus;
    private readonly AutoRemovingStorage<IDestroyable> _destroyableStorage;

    public ModelFinalizer(EventBus eventBus)
    {
        Validator.ValidateNotNull(eventBus);

        _eventBus = eventBus;

        _destroyableStorage = new AutoRemovingStorage<IDestroyable>(1000);
    }

    public void Start()
    {
        _eventBus.Subscribe<CreatedSignal<IDestroyable>>(OnModelCreated);
        _eventBus.Subscribe<ClearedSignal<Level>>(DestroyModels);
    }

    public void Finish()
    {
        _eventBus.Unsubscribe<CreatedSignal<IDestroyable>>(OnModelCreated);
        _eventBus.Unsubscribe<ClearedSignal<Level>>(DestroyModels);
    }

    private void OnModelCreated(CreatedSignal<IDestroyable> createdSignal)
    {
        if (createdSignal.Creatable is not Model model)
        {
            return;
        }

        _destroyableStorage.Add(model);
    }

    private void DestroyModels(ClearedSignal<Level> _)
    {
        _destroyableStorage.ClearWithDestroy();
    }
}