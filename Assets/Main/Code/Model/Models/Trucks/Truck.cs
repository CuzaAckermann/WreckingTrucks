using System;
using UnityEngine;

public abstract class Truck : Model
{
    private readonly BlockTracker _blockTracker;
    private readonly Stopwatch _stopwatch;
    private readonly float _shotCooldown;

    private Block _target;

    public Truck(Gun gun,
                 Trunk trunk,
                 BlockTracker blockTracker,
                 Stopwatch stopwatch,
                 float shotCooldown)
    {
        if (shotCooldown < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shotCooldown));
        }

        Gun = gun ?? throw new ArgumentNullException(nameof(gun));
        Trunk = trunk ?? throw new ArgumentNullException(nameof(trunk));

        _blockTracker = blockTracker ?? throw new ArgumentNullException(nameof(blockTracker));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _shotCooldown = shotCooldown;

        Gun.SetDirectionForward(Forward);
    }

    public event Action<IModel> InterfaceDestroyed;

    public Gun Gun { get; private set; }

    public Trunk Trunk { get; private set; }

    public Type DestroyableType { get; private set; }

    public void SetDestroyableType<T>() where T : Block
    {
        DestroyableType = typeof(T);
    }

    public void Prepare(Field field, CartrigeBox cartrigeBox)
    {
        Gun.Upload();
        Trunk.SetCartrigeBox(cartrigeBox);
        _blockTracker.Prepare(field, DestroyableType);
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        base.SetDirectionForward(forward);

        Gun.SetDirectionForward(Forward);
        Trunk.SetDirectionForward(Forward);
    }

    public override void Destroy()
    {
        FinishShooting();
        Gun.Destroy();
        Trunk.Destroy();
        base.Destroy();
        InterfaceDestroyed?.Invoke(this);
    }

    private void FindTarget()
    {
        _blockTracker.FindTarget(Position);
    }

    // корректировка
    public void StartShooting()
    {
        _blockTracker.TargetDetected += OnTargetDetected;
        Gun.ShootingEnded += OnShootingEnded;

        _stopwatch.SetNotificationInterval(_shotCooldown);
        _stopwatch.IntervalPassed += FindTarget;
        _stopwatch.Start();
    }

    public void FinishShooting()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= FindTarget;

        Gun.TargetRotationReached -= Shoot;
        Gun.ShootingEnded -= OnShootingEnded;
        Gun.Clear();

        _blockTracker.TargetDetected -= OnTargetDetected;
    }

    private void OnShootingEnded(Gun _)
    {
        FinishShooting();
    }

    private void OnTargetDetected(Block target)
    {
        _stopwatch.Stop();
        _target = target;
        Gun.TargetRotationReached += Shoot;
        Gun.SetTargetRotation(_target.Position);
    }

    private void Shoot(Model _)
    {
        Gun.TargetRotationReached -= Shoot;
        Gun.Shoot(_target);
        _stopwatch.Continue();
    }
    // корректировка
}