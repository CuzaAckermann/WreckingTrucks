using System.Collections.Generic;

public class ModelFinalizer : IAbility
{
    private readonly EventBus _eventBus;

    private readonly List<IDestroyable> _createdModels;

    public ModelFinalizer(EventBus eventBus)
    {
        Validator.ValidateNotNull(eventBus);

        _eventBus = eventBus;

        _createdModels = new List<IDestroyable>();
    }

    public void Start()
    {
        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelCreated);
        _eventBus.Subscribe<ClearedSignal<Level>>(DestroyModels);
    }

    public void Finish()
    {
        _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelCreated);
        _eventBus.Unsubscribe<ClearedSignal<Level>>(DestroyModels);
    }

    private void DestroyModels(ClearedSignal<Level> _)
    {
        for (int i = _createdModels.Count - 1; i >= 0; i--)
        {
            _createdModels[i].Destroyed -= OnDestroyed;
            _createdModels[i].Destroy();
        }
    }

    private void OnModelCreated(CreatedSignal<Model> modelSignal)
    {
        Model model = modelSignal.Creatable;

        if (Validator.IsRequiredType(model, out IDestroyable destroyable) == false)
        {
            return;
        }

        if (Validator.IsContains(_createdModels, destroyable))
        {
            return;
        }

        _createdModels.Add(model);

        model.Destroyed += OnDestroyed;
    }

    private void OnDestroyed(IDestroyable destroyable)
    {
        destroyable.Destroyed -= OnDestroyed;

        if (Validator.IsContains(_createdModels, destroyable) == false)
        {
            return;
        }

        _createdModels.Remove(destroyable);
    }
}