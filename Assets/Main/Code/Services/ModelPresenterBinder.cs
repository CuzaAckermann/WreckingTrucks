using System;
using System.Collections.Generic;

public class ModelPresenterBinder
{
    private readonly IModelPresenterCreator _modelPresenterCreator;
    private readonly List<IModelAddedNotifier> _notifiers;

    public ModelPresenterBinder(IModelPresenterCreator modelPresenterCreators)
    {
        _modelPresenterCreator = modelPresenterCreators ?? throw new ArgumentNullException(nameof(modelPresenterCreators));
        _notifiers = new List<IModelAddedNotifier>();
    }

    public void Clear()
    {
        UnsubscribeFromNotifiers();
        _notifiers.Clear();
    }

    public void AddNotifier(IModelAddedNotifier notifier)
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
        SubscribeToNotifiers();
    }

    public void Disable()
    {
        UnsubscribeFromNotifiers();
    }

    private void SubscribeToNotifiers()
    {
        foreach (IModelAddedNotifier notifier in _notifiers)
        {
            notifier.ModelAdded += OnModelAdded;
        }
    }

    private void UnsubscribeFromNotifiers()
    {
        foreach (IModelAddedNotifier notifier in _notifiers)
        {
            notifier.ModelAdded -= OnModelAdded;
        }
    }

    private void OnModelAdded(Model model)
    {
        _modelPresenterCreator.GetPresenter(model).Bind(model);
    }
}