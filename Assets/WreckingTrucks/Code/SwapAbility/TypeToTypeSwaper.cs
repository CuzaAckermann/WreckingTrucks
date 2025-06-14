using System;
using System.Collections.Generic;
using UnityEngine;

public class TypeToTypeSwaper<FT, ST> : ITickable where FT : Model
                                                  where ST : Model
{
    private OneTypeModelSelector _selector;
    private Mover _mover;

    public TypeToTypeSwaper(OneTypeModelSelector selector, Mover mover)
    {
        _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
    }

    public void Swap<FirstType, SecondType>(Field field) where FirstType : FT
                                                         where SecondType : ST
    {
        List<Model> models = _selector.Select<FirstType>(field);
        
        for (int i = 0; i < models.Count; i++)
        {
            models[i].SetTargetPosition(new Vector3(models[i].Position.x,
                                                    20,
                                                    models[i].Position.z));
        }

        //_mover.AddModels(models);
    }

    public void Tick(float deltaTime)
    {
        _mover.Tick(deltaTime);
    }
}