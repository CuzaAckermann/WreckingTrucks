using System;
using System.Collections.Generic;

public class Rotater
{
    private readonly IPositionsModelsChangedNotifier _modelsAddedNotifier;
    private readonly List<Model> _rotatableModels;

    private readonly float _speedRotation;
    private readonly float _minAngleToTargetDirection;

    public Rotater(IPositionsModelsChangedNotifier modelAddedNotifier,
                   float speedRotation,
                   float minAngleToTargetDirection,
                   int capacityCollection)
    {
        if (speedRotation <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(speedRotation));
        }

        if (minAngleToTargetDirection < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minAngleToTargetDirection));
        }

        if (capacityCollection <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacityCollection));
        }

        _modelsAddedNotifier = modelAddedNotifier ?? throw new ArgumentNullException(nameof(modelAddedNotifier));
        _rotatableModels = new List<Model>(capacityCollection);
        _speedRotation = speedRotation;
        _minAngleToTargetDirection = minAngleToTargetDirection;
    }

    public void Clear()
    {
        foreach (Model model in _rotatableModels)
        {
            if (model != null)
            {
                OnDestroyed(model);
            }
        }

        Disable();
        _rotatableModels.Clear();
    }

    public void Enable()
    {
        _modelsAddedNotifier.TargetPositionsModelsChanged += AddModels;
    }

    public void Tick(float deltaTime)
    {
        if (_rotatableModels.Count == 0)
        {
            return;
        }

        float frameRotation = _speedRotation * deltaTime;

        for (int i = _rotatableModels.Count - 1; i >= 0; i--)
        {
            Model model = _rotatableModels[i];

            if (model == null)
            {
                _rotatableModels.RemoveAt(i);
                continue;
            }

            RotateModel(model, frameRotation);
        }
    }

    public void Disable()
    {
        _modelsAddedNotifier.TargetPositionsModelsChanged -= AddModels;
    }

    private void AddModels(List<Model> models)
    {
        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        foreach (var model in models)
        {
            AddModel(model);
        }
    }

    private void AddModel(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (_rotatableModels.Contains(model) == false)
        {
            _rotatableModels.Add(model);
            model.Destroyed += OnDestroyed;
        }
    }

    private void RotateModel(Model model, float frameRotation)
    {
        if (model.CurrentAngleToDirectionToTarget > _minAngleToTargetDirection || model.CurrentAngleToDirectionToTarget > frameRotation)
        {
            model.Rotate(frameRotation);
        }
        else
        {
            CompleteRotation(model);
        }
    }

    private void CompleteRotation(Model model)
    {
        OnDestroyed(model);
        model.FinishRotate();
    }

    private void OnDestroyed(Model destroyedModel)
    {
        destroyedModel.Destroyed -= OnDestroyed;
        _rotatableModels.Remove(destroyedModel);
    }
}