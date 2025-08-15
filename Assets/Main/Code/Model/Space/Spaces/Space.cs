using System;
using System.Collections.Generic;

public class Space<F> : IModelAddedNotifier, IModelDestroyNotifier where F : Field
{
    protected readonly Filler Filler;

    private readonly Mover _mover;

    public Space(F field, Mover mover, Filler filler)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        Filler = filler ?? throw new ArgumentNullException(nameof(filler));
    }

    public event Action<Model> ModelAdded;
    public event Action Wasted;



    public event Action<Model> ModelDestroyRequested;

    public event Action<IReadOnlyList<Model>> ModelsDestroyRequested;

    public event Action<IModel> InterfaceModelDestroyRequested;

    public event Action<IReadOnlyList<IModel>> InterfaceModelsDestroyRequested;



    public F Field { get; private set; }

    public void Clear()
    {
        ModelsDestroyRequested?.Invoke(Field.GetModels());
        Field.Clear();
        _mover.Clear();
        Filler.Clear();
    }

    public virtual void Prepare()
    {
        Filler.PrepareFilling();
        Field.ContinueShiftModels();
    }

    public virtual void Enable()
    {
        Field.ModelAdded += OnModelAdded;
        Field.Devastated += OnDevastated;

        Field.Enable();
        Filler.Enable();
        _mover.Enable();
    }

    public virtual void Disable()
    {
        Field.Disable();
        Filler.Disable();
        _mover.Disable();

        Field.ModelAdded -= OnModelAdded;
        Field.Devastated -= OnDevastated;
    }

    private void OnModelAdded(Model model)
    {
        ModelAdded?.Invoke(model);
    }

    private void OnDevastated()
    {
        Wasted?.Invoke();
    }
}