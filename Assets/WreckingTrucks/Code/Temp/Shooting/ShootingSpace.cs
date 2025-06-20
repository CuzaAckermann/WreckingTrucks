using System;

public class ShootingSpace
{
    private readonly BulletSimulation _bulletSimulation;
    private readonly Rotator _gunsRotator;
    private readonly Mover _bulletsMover;

    private readonly ModelPresenterBinder _modelPresenterBinder;

    private TickEngine _tickEngine;

    public ShootingSpace(BulletSimulation bulletSimulation,
                         Rotator rotatorGuns,
                         Mover bulletsMover,
                         ModelPresenterBinder modelPresenterBinder)
    {
        _bulletSimulation = bulletSimulation ?? throw new ArgumentNullException(nameof(bulletSimulation));
        _gunsRotator = rotatorGuns ?? throw new ArgumentNullException(nameof(rotatorGuns));
        _bulletsMover = bulletsMover ?? throw new ArgumentNullException(nameof(bulletsMover));
        _modelPresenterBinder = modelPresenterBinder ?? throw new ArgumentNullException(nameof(modelPresenterBinder));

        _tickEngine = new TickEngine();
    }

    public void Clear()
    {
        _bulletsMover.Clear();
        _gunsRotator.Clear();
        _bulletSimulation.Clear();
    }

    public void Prepare()
    {
        _tickEngine.AddTickable(_gunsRotator);
        _tickEngine.AddTickable(_bulletsMover);
    }

    public void AddGun(Gun gun)
    {
        _bulletSimulation.AddGun(gun);
    }

    public void Start()
    {
        _modelPresenterBinder.Enable();
        _bulletsMover.Enable();
        _gunsRotator.Enable();

        _tickEngine.Continue();
    }

    public void Update(float deltaTime)
    {
        _tickEngine.Tick(deltaTime);
    }

    public void Stop()
    {
        _tickEngine.Pause();

        _modelPresenterBinder.Disable();
        _bulletsMover.Disable();
        _gunsRotator.Disable();
    }
}