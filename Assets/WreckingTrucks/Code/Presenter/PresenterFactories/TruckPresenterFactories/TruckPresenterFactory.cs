using UnityEngine;

public abstract class TruckPresenterFactory<T> : PresenterFactory where T : TruckPresenter
{
    [SerializeField] private T _truckPresenter;

    protected override Presenter GetPresenterPrefab()
    {
        return _truckPresenter;
    }
}