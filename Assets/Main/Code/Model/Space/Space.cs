using System;
using System.Collections.Generic;

public class Space<F> : IModelAddedNotifier where F : Field
{
    protected readonly Filler Filler;

    private readonly Mover _mover;
    private readonly ModelFinalizer _modelFinalizer;

    public Space(F field, Mover mover, Filler filler, ModelFinalizer modelFinalizer)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        Filler = filler ?? throw new ArgumentNullException(nameof(filler));
        _modelFinalizer = modelFinalizer ?? throw new ArgumentNullException(nameof(modelFinalizer));
    }

    public event Action<Model> ModelAdded;

    public F Field { get; private set; }

    public void Clear()
    {
        IReadOnlyList<Model> models = Field.GetModels();

        Field.Clear();
        _mover.Clear();
        Filler.Clear();

        _modelFinalizer.FinishModels(models);
    }

    public void Prepare(FillingCard fillingCard)
    {
        Filler.PrepareFilling(fillingCard);
        Field.ContinueShiftModels();
    }

    public virtual void Enable()
    {
        Field.ModelAdded += OnModelAdded;

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
    }

    private void OnModelAdded(Model model)
    {
        ModelAdded?.Invoke(model);
    }
}