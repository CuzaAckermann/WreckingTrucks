using System;

public class ModelPresenterBinder
{
    private readonly IModelSource _modelSource;
    private readonly IModelPresenterSource _presenterSource;

    public ModelPresenterBinder(IModelSource modelSource,
                                IModelPresenterSource modelPresenterSource)
    {
        _modelSource = modelSource ?? throw new ArgumentNullException(nameof(modelSource));
        _presenterSource = modelPresenterSource ?? throw new ArgumentNullException(nameof(modelPresenterSource));
    }

    public void Enable()
    {
        _modelSource.ModelAdded += OnModelAdded;
    }

    public void Disable()
    {
        _modelSource.ModelAdded -= OnModelAdded;
    }

    private void OnModelAdded(Model model)
    {
        _presenterSource.GetPresenter(model).Bind(model);
    }
}