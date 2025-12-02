using System;
using System.Collections.Generic;

public class ModelPresenterBinder : IBlockPresenterCreationNotifier
{
    private readonly ModelProduction _modelProduction;

    private readonly IModelPresenterCreator _modelPresenterCreator;
    private readonly PresenterPainter _presenterPainter;

    public ModelPresenterBinder(ModelProduction modelProduction, IModelPresenterCreator modelPresenterCreators, PresenterPainter presenterPainter)
    {
        _modelProduction = modelProduction ?? throw new ArgumentNullException(nameof(modelProduction));

        _modelPresenterCreator = modelPresenterCreators ?? throw new ArgumentNullException(nameof(modelPresenterCreators));
        _presenterPainter = presenterPainter ? presenterPainter : throw new ArgumentNullException(nameof(presenterPainter));
    }

    public event Action<BlockPresenter> BlockPresenterCreated;

    public void Clear()
    {
        UnsubscribeFromNotifiers();
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
        _modelProduction.ModelCreated += OnModelAdded;
    }

    private void UnsubscribeFromNotifiers()
    {
        _modelProduction.ModelCreated -= OnModelAdded;
    }

    private void OnModelAdded(Model model)
    {
        if (_modelPresenterCreator.TryGetPresenter(model, out Presenter presenter) == false)
        {
            return;
        }

        presenter.Bind(model);
        _presenterPainter.Paint(presenter);

        if (presenter is BlockPresenter blockPresenter)
        {
            BlockPresenterCreated?.Invoke(blockPresenter);
        }
    }
}