using System;
using System.Collections.Generic;

public class ModelFinalizer
{
    private readonly IModelDestroyNotifier _notifier;

    //public ModelFinalizer(IModelDestroyNotifier notifier)
    //{
    //    _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
    //}

    //public void Enable()
    //{
    //    _notifier.ModelDestroyRequested += FinishModel;
    //    _notifier.ModelsDestroyRequested += FinishModels;
    //}

    //public void Disable()
    //{
    //    _notifier.ModelDestroyRequested -= FinishModel;
    //    _notifier.ModelsDestroyRequested -= FinishModels;
    //}

    public void FinishModels(IReadOnlyList<Model> models)
    {
        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        foreach (Model model in models)
        {
            FinishModel(model);
        }
    }

    public void FinishModel(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Destroy();
    }
}