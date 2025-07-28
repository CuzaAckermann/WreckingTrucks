using System;
using System.Collections.Generic;

public class ModelFinalizer
{
    private readonly List<IModelDestroyNotifier> _notifiers;

    public ModelFinalizer()
    {
        _notifiers = new List<IModelDestroyNotifier>();
    }

    public void Clear()
    {
        _notifiers.Clear();
    }

    public void AddNotifier(IModelDestroyNotifier notifier)
    {
        if (notifier == null)
        {
            throw new ArgumentNullException(nameof(notifier));
        }

        if (_notifiers.Contains(notifier))
        {
            throw new InvalidOperationException(nameof(notifier));
        }

        _notifiers.Add(notifier);
    }

    public void Enable()
    {
        for (int i = 0; i < _notifiers.Count; i++)
        {
            SubscribeToNotifier(_notifiers[i]);
        }
    }

    public void Disable()
    {
        for (int i = 0; i < _notifiers.Count; i++)
        {
            UnsubscribeFromNotifier(_notifiers[i]);
        }
    }

    private void SubscribeToNotifier(IModelDestroyNotifier notifier)
    {
        notifier.ModelDestroyRequested += FinishModel;
        notifier.ModelsDestroyRequested += FinishModels;
    }

    private void UnsubscribeFromNotifier(IModelDestroyNotifier notifier)
    {
        notifier.ModelDestroyRequested -= FinishModel;
        notifier.ModelsDestroyRequested -= FinishModels;
    }

    private void FinishModels(IReadOnlyList<Model> models)
    {
        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        for (int i = models.Count - 1; i >= 0; i--)
        {
            FinishModel(models[i]);
        }
    }

    private void FinishModel(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Destroy();
    }
}