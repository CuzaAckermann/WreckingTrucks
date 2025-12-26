using System;
using System.Collections.Generic;
using System.Linq;

public abstract class ModelProcessor : ITickable
{
    protected readonly ModelProduction _modelProduction;

    //protected readonly HashSet<Model> _createdModels;
    protected readonly HashSet<Model> _activeModels;
    protected readonly HashSet<Model> _toAddActiveModels;
    protected readonly HashSet<Model> _toRemoveActiveModels;

    private bool _isRunning;
    private bool _isUpdating;

    protected abstract string ProcessorName { get; }

    protected abstract Action<Model, float> ProcessAction { get; }

    public ModelProcessor(int capacity, ModelProduction modelProduction)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be positive");
        }

        _modelProduction = modelProduction ?? throw new ArgumentNullException(nameof(modelProduction));

        //_createdModels = new HashSet<Model>();
        _activeModels = new HashSet<Model>();
        _toAddActiveModels = new HashSet<Model>();
        _toRemoveActiveModels = new HashSet<Model>();

        _isRunning = false;
        _isUpdating = false;
    }

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public void Clear()
    {
        //List<Model> modelsToClear = _createdModels.ToList();

        //foreach (var model in modelsToClear)
        //{
        //    OnDestroyed(model);
        //}
    }

    public void Enable()
    {
        if (_isRunning == false)
        {
            _isRunning = true;

            _modelProduction.ModelCreated += OnModelCreated;

            Activated?.Invoke(this);
        }
    }

    public void Disable()
    {
        if (_isRunning)
        {
            _isRunning = false;

            Deactivated?.Invoke(this);

            _modelProduction.ModelCreated -= OnModelCreated;
        }
    }

    public void Tick(float deltaTime)
    {
        if (_activeModels.Count == 0)
        {
            return;
        }

        _isUpdating = true;

        // Фильтруем null модели и те, которые помечены на удаление
        var modelsToProcess = _activeModels
            .Where(model => model != null && _toRemoveActiveModels.Contains(model) == false)
            .ToList();

        foreach (var model in modelsToProcess)
        {
            ProcessAction(model, deltaTime);
        }

        _isUpdating = false;
        ProcessChanges();
    }

    protected virtual void ProcessChanges()
    {
        // Обрабатываем удаление
        if (_toRemoveActiveModels.Count > 0)
        {
            foreach (var model in _toRemoveActiveModels)
            {
                if (_activeModels.Contains(model))
                {
                    UnsubscribeFromActiveModel(model);
                    _activeModels.Remove(model);
                }
            }

            _toRemoveActiveModels.Clear();
        }

        // Обрабатываем добавление
        if (_toAddActiveModels.Count > 0)
        {
            foreach (var model in _toAddActiveModels)
            {
                if (_activeModels.Contains(model) == false)
                {
                    _activeModels.Add(model);
                    SubscribeToActiveModel(model);
                }
            }

            _toAddActiveModels.Clear();
        }
    }

    protected virtual void OnModelCreated(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        //if (_createdModels.Contains(model))
        //{
        //    throw new InvalidOperationException($"{model} is already added to {ProcessorName}");
        //}

        //_createdModels.Add(model);

        SubscribeToCreatedModel(model);
    }

    protected virtual void OnDestroyed(Model model)
    {
        UnsubscribeFromCreatedModel(model);
        DeactivateModel(model);
        //_createdModels.Remove(model);
    }

    protected virtual void ActivateModel(Model model)
    {
        if (_isUpdating)
        {
            _toAddActiveModels.Add(model);
            _toRemoveActiveModels.Remove(model);
        }
        else
        {
            _activeModels.Add(model);
            SubscribeToActiveModel(model);
        }
    }

    protected virtual void DeactivateModel(Model model)
    {
        if (_isUpdating)
        {
            _toRemoveActiveModels.Add(model);
            _toAddActiveModels.Remove(model);
        }
        else
        {
            if (_activeModels.Remove(model))
            {
                UnsubscribeFromActiveModel(model);
            }
        }
    }

    protected abstract void SubscribeToCreatedModel(Model model);

    protected abstract void UnsubscribeFromCreatedModel(Model model);

    protected abstract void SubscribeToActiveModel(Model model);

    protected abstract void UnsubscribeFromActiveModel(Model model);
}