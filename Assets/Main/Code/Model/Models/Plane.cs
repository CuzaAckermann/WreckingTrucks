using System;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Model
{
    private readonly Vector3 _gunPosition;
    private readonly Vector3 _trunkPosition;

    private readonly Stopwatch _stopwatch;
    private readonly float _shotCooldown;

    private readonly int _amountDestroyedRows;
    private readonly Queue<Block> _targets;

    private Field _field;

    public Plane(Gun gun,
                 Trunk trunk,
                 Stopwatch stopwatch,
                 float shotCooldown,
                 int amountDestroyedRows,
                 Vector3 gunPosition,
                 Vector3 trunkPosition)
    {
        if (shotCooldown <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shotCooldown));
        }

        if (amountDestroyedRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountDestroyedRows));
        }

        Gun = gun ?? throw new ArgumentNullException(nameof(gun));
        Trunk = trunk ?? throw new ArgumentNullException(nameof(trunk));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));

        _shotCooldown = shotCooldown;
        _amountDestroyedRows = amountDestroyedRows;

        _gunPosition = gunPosition;
        _trunkPosition = trunkPosition;

        _targets = new Queue<Block>();
    }

    public Gun Gun { get; private set; }

    public Trunk Trunk { get; private set; }

    // корректировка
    public event Action<Plane> TargetCheckPointReached;

    public void Prepare(Field field, CartrigeBox cartrigeBox)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        Gun.Upload();
        Trunk.SetCartrigeBox(cartrigeBox);
        SetPositionForComponents();
    }

    public override void Move(float distance)
    {
        SetPositionForComponents();
        base.Move(distance);
    }

    protected override void FinishMovement()
    {
        SetPositionForComponents();
        base.FinishMovement();
        UpdateShootingStatus();
        TargetCheckPointReached?.Invoke(this);
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        Gun.SetDirectionForward(forward);
        Trunk.SetDirectionForward(forward);

        base.SetDirectionForward(forward);
    }

    public override void Rotate(float frameRotation)
    {
        Trunk.SetDirectionForward(Forward);
        base.Rotate(frameRotation);
    }

    public override void Destroy()
    {
        FinishShooting();
        Gun.Destroy();
        Trunk.Destroy();
        base.Destroy();
    }

    public void FinishShooting()
    {
        _field.ContinueShiftModels();

        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= Shoot;

        //Gun.ShootingEnded -= FinishShooting;
        //Gun.RotateToDefault(Forward);
        Gun.Clear();
    }

    protected override void FinishRotate()
    {
        Trunk.SetDirectionForward(Forward);
        base.FinishRotate();
    }

    private void SetPositionForComponents()
    {
        Gun.SetPosition(CalculateWorldPosition(_gunPosition));
        Trunk.SetPosition(CalculateWorldPosition(_trunkPosition));
    }

    private Vector3 CalculateWorldPosition(Vector3 localPosition)
    {
        Vector3 offset = Vector3.up * localPosition.y + Forward * localPosition.z;

        return Position + offset;
    }
    // корректировка

    private void UpdateShootingStatus()
    {
        _field.StopShiftModels();
        DetermineTargets();

        //Gun.ShootingEnded += FinishShooting;

        _stopwatch.SetNotificationInterval(_shotCooldown);
        _stopwatch.IntervalPassed += Shoot;
        _stopwatch.Start();
    }

    private void Shoot()
    {
        Gun.Shoot(_targets.Dequeue());
    }

    private void DetermineTargets()
    {
        IReadOnlyList<Model> models = _field.GetModelsForPlane(_amountDestroyedRows);

        FormQueueOfTargets(models);
    }

    private void FormQueueOfTargets(IReadOnlyList<Model> models)
    {
        foreach (Model model in models)
        {
            if (model is Block block)
            {
                _targets.Enqueue(block);
            }
        }
    }
}