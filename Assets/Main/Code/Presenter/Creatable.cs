using System;
using UnityEngine;

public abstract class Creatable : MonoBehaviour
{
    public event Action<Creatable> LifeTimeFinished;

    public abstract void Init();

    protected void OnLifeTimeFinished()
    {
        LifeTimeFinished?.Invoke(this);
    }
}
