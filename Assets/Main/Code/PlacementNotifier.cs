using System;

public class PlacementNotifier
{
    private readonly ModelProduction _modelProduction;
    private readonly Storage<Model> _modelStorage;

    public PlacementNotifier(ModelProduction modelProduction)
    {
        Validator.ValidateNotNull(modelProduction);

        _modelProduction = modelProduction;
        _modelStorage = new Storage<Model>();

        SubscribeToModelProduction();
    }

    public event Action<Model> Placed;

    private void SubscribeToModelProduction()
    {
        _modelProduction.ModelCreated += OnModelCreated;
    }

    private void UnsubscribeFromModelProduction()
    {
        _modelProduction.ModelCreated -= OnModelCreated;
    }

    private void OnModelCreated(Model model)
    {
        model.Destroyed += OnDestroyed;
        //model.Placeable.PositionChanged += OnPositionChanged;

        _modelStorage.Add(model);
    }

    private void OnDestroyed(IDestroyable destroyable)
    {

    }

    private void OnPositionChanged(Placeable model)
    {

    }
}