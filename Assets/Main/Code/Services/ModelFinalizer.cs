using System;
using System.Collections.Generic;

public class ModelFinalizer
{
    private readonly ModelProduction _modelProduction;
    private readonly List<Model> _createdModels;

    private bool _isSubscribed;

    public ModelFinalizer(ModelProduction modelProduction)
    {
        _modelProduction = modelProduction ?? throw new ArgumentNullException(nameof(modelProduction));
        _createdModels = new List<Model>();
        _isSubscribed = false;

        Enable();
    }

    public void Enable()
    {
        if (_isSubscribed == false)
        {
            _modelProduction.ModelCreated += OnModelCreated;
            _isSubscribed = true;
        }
    }

    public void Disable()
    {
        if (_isSubscribed)
        {
            _modelProduction.ModelCreated -= OnModelCreated;
            _isSubscribed = false;
        }
    }

    public void DestroyModels()
    {
        for (int i = _createdModels.Count - 1; i >= 0; i--)
        {
            _createdModels[i].Destroy();
        }
    }

    private void OnModelCreated(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (_createdModels.Contains(model))
        {
            throw new InvalidOperationException($"{model} is already added");
        }

        _createdModels.Add(model);

        model.Destroyed += OnDestroyed;
    }

    private void OnDestroyed(Model model)
    {
        model.Destroyed -= OnDestroyed;

        if (_createdModels.Contains(model) == false)
        {
            throw new InvalidOperationException($"{model} does not exist");
        }

        _createdModels.Remove(model);
    }
}