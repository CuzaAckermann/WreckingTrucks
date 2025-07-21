using System;
using UnityEngine;

[Serializable]
public class PresenterFactorySettings<P> : FactorySettings where P : Presenter
{
    [SerializeField] private P _prefab;

    public P Prefab => _prefab;
}