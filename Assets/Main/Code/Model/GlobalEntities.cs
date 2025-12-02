using System;

public class GlobalEntities
{
    private readonly Mover _mover;
    private readonly Rotator _rotator;

    private readonly ModelFinalizer _modelFinalizer;
    private readonly ModelPresenterBinder _modelPresenterBinder;

    private readonly BlockPresenterShaker _blockPresenterShaker;

    private readonly ShootingSoundPlayer _shootingSoundPlayer;

    public GlobalEntities(Mover mover,
                          Rotator rotator,
                          ModelFinalizer modelFinalizer,
                          ModelPresenterBinder modelPresenterBinder,
                          BlockPresenterShaker blockPresenterShaker,
                          ShootingSoundPlayer shootingSoundPlayer)
    {
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _rotator = rotator ?? throw new ArgumentNullException(nameof(rotator));

        _modelFinalizer = modelFinalizer ?? throw new ArgumentNullException(nameof(modelFinalizer));
        _modelPresenterBinder = modelPresenterBinder ?? throw new ArgumentNullException(nameof(modelPresenterBinder));

        _blockPresenterShaker = blockPresenterShaker ?? throw new ArgumentNullException(nameof(blockPresenterShaker));

        _shootingSoundPlayer = shootingSoundPlayer ? shootingSoundPlayer : throw new ArgumentNullException(nameof(shootingSoundPlayer));

        _blockPresenterShaker.Prepare(_modelPresenterBinder);
    }

    public void Clear()
    {
        _mover.Clear();
        _rotator.Clear();

        _modelPresenterBinder.Clear();
        _blockPresenterShaker.Clear();
    }

    public void Enable()
    {
        _mover.Enable();
        _rotator.Enable();

        _modelPresenterBinder.Enable();
        _modelFinalizer.Enable();

        _blockPresenterShaker.Enable();
    }

    public void Disable()
    {
        _mover.Disable();
        _rotator.Disable();

        _modelPresenterBinder.Disable();
        _modelFinalizer.Disable();

        _blockPresenterShaker.Disable();
    }

    public void DestroyAll()
    {
        _modelFinalizer.DestroyModels();
    }
}