using System;

public class ShootingSpace : IModelAddedNotifier
{
    private readonly BulletSimulation _bulletSimulation;
    private readonly Mover _bulletsMover;
    private readonly Rotator _gunRotator;

    public ShootingSpace(BulletSimulation bulletSimulation,
                         Mover bulletsMover,
                         Rotator rotatorGuns)
    {
        _bulletSimulation = bulletSimulation ?? throw new ArgumentNullException(nameof(bulletSimulation));
        _gunRotator = rotatorGuns ?? throw new ArgumentNullException(nameof(rotatorGuns));
        _bulletsMover = bulletsMover ?? throw new ArgumentNullException(nameof(bulletsMover));
    }

    public event Action<Model> ModelAdded;

    public void Clear()
    {
        _bulletSimulation.Clear();
        _bulletsMover.Clear();
        _gunRotator.Clear();
    }

    public void AddGun(Gun gun)
    {
        _bulletSimulation.AddGun(gun);
    }

    public void Enable()
    {
        _bulletSimulation.ModelAdded += OnModelAdded;

        _bulletsMover.Enable();
        _gunRotator.Enable();
    }

    public void Disable()
    {
        _bulletsMover.Disable();
        _gunRotator.Disable();

        _bulletSimulation.ModelAdded -= OnModelAdded;
    }

    private void OnModelAdded(Model model)
    {
        ModelAdded?.Invoke(model);
    }
}