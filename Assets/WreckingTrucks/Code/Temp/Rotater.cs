using System;
using System.Collections.Generic;
using UnityEngine;

public class Rotater
{
    private IPositionsModelsChangedNotifier _modelsAddedNotifier;
    private List<Model> _rotatableModels;

    private float _speedRotation;
    private float _minAngle;

    public Rotater(IPositionsModelsChangedNotifier modelAddedNotifier,
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
            Stop();
        }

        _rotatableModels.Clear();
    }

    public void Start()
    {
        _modelsAddedNotifier.TargetPositionsModelsChanged += AddModel;
    }

    public void Stop()
    {
        _modelsAddedNotifier.TargetPositionsModelsChanged -= AddModel;
    }

    public void AddModel(List<Model> models)
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

    public void Tick(float deltaTime)
    {
        float frameRotation = _speedRotation * deltaTime;
        bool shouldRotate = frameRotation > _minAngle;

        for (int i = _rotatableModels.Count - 1; i >= 0; i--)
        {
            Model model = _rotatableModels[i];

            if (model == null)
            {
                _rotatableModels.RemoveAt(i);
                continue;
            }

            if (shouldRotate)
            {
                RotateModel(model, frameRotation);
            }
            else
            {
                model.FinishRotate();
                _rotatableModels.RemoveAt(i);
            }
        }

        //float frameRotation = _speedRotation * deltaTime;

        //for (int i = _rotatableModels.Count - 1; i >= 0; i--)
        //{
        //    if (_rotatableModels[i] == null)
        //    {
        //        _rotatableModels.Remove(_rotatableModels[i]);
        //        continue;
        //    }

        //    RotateModel(_rotatableModels[i], frameRotation);
        //}
    }

    private void AddModel(Model model)
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

    private void RotateModel(Model model, float frameRotation)
    {
        float rotationAmount = model.IsTurnClockwise ? frameRotation : -frameRotation;
        Quaternion rotation = Quaternion.Euler(0, rotationAmount, 0);
        model.Rotate(rotation);

        //if (frameRotation > _minAngle)
        //{

        //    if (model.IsTurnClockwise)
        //    {
        //        Quaternion rotation = Quaternion.Euler(0, frameRotation, 0);
        //        model.Rotate(rotation);
        //    }
        //    else
        //    {
        //        Quaternion rotation = Quaternion.Euler(0, -frameRotation, 0);
        //        model.Rotate(rotation);
        //    }
        //}
        //else
        //{
        //    model.FinishRotate();
        //}
    }
}