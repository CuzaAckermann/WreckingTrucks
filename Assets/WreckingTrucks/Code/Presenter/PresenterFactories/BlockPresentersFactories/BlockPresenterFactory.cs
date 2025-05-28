using UnityEngine;

public abstract class BlockPresenterFactory<T> : PresenterFactory where T : BlockPresenter
{
    [SerializeField] private T _blockPresenter;

    protected override Presenter GetPresenterPrefab()
    {
        return _blockPresenter;
    }
}