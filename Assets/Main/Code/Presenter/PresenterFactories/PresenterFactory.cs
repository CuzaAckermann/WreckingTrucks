using System;
using UnityEngine;

public abstract class PresenterFactory<P> : MonoBehaviourFactory<P>, IPresenterCreator where P : Presenter
{
    public Presenter CreatePresenter()
    {
        return Create();
    }
}