using System;
using System.Collections.Generic;
using UnityEngine;

public class Rotater
{
    private IModelAddedNotifier _modelsAddedNotifier;
    private List<Model> _rotatableModels;

    private float _speedRotation;
    private float _minAngle;

    public Rotater(IModelAddedNotifier modelAddedNotifier,
                   float speedRotation,
                   float minAngle,
                   int capacityCollection)
    {
        if (speedRotation <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(speedRotation));
        }

        if (minAngle < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minAngle));
        }

        if (capacityCollection <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacityCollection));
        }

        _modelsAddedNotifier = modelAddedNotifier ?? throw new ArgumentNullException(nameof(modelAddedNotifier));
        _rotatableModels = new List<Model>(capacityCollection);
        _speedRotation = speedRotation;
        _minAngle = minAngle;
    }

    public void Clear()
    {
        foreach (Model model in _rotatableModels)
        {
            _modelsAddedNotifier.ModelAdded -= AddModel;
        }

        _rotatableModels.Clear();
    }

    public void Start()
    {
        _modelsAddedNotifier.ModelAdded += AddModel;
    }

    public void Exit()
    {
        _modelsAddedNotifier.ModelAdded -= AddModel;
    }

    public void AddModel(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (_rotatableModels.Contains(model))
        {
            throw new InvalidOperationException(nameof(model));
        }

        _rotatableModels.Add(model);
    }

    public void Tick(float deltaTime)
    {
        float frameRotation = _speedRotation * deltaTime;

        for (int i = _rotatableModels.Count - 1; i >= 0; i--)
        {
            if (_rotatableModels[i] == null)
            {
                _rotatableModels.Remove(_rotatableModels[i]);
                continue;
            }

            RotateModel(_rotatableModels[i], frameRotation);
        }
    }

    private void RotateModel(Model model, float frameRotation)
    {
        if (frameRotation > _minAngle)
        {

            if (model.IsTurnClockwise)
            {
                Quaternion rotation = Quaternion.Euler(0, frameRotation, 0);
                model.Rotate(rotation);
            }
            else
            {
                Quaternion rotation = Quaternion.Euler(0, -frameRotation, 0);
                model.Rotate(rotation);
            }
        }
        else
        {
            model.FinishRotate();
        }
    }
}