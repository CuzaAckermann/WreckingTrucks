using System;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Model
{
    private readonly Stopwatch _stopwatch;
    private readonly float _shotCooldown;

    private readonly int _amountDestroyedRows;
    private readonly Queue<Block> _targets;

    private Field _field;

    public Plane(Gun gun,
                 Trunk trunk,
                 Stopwatch stopwatch,
                 float shotCooldown,
                 int amountDestroyedRows)
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

        _targets = new Queue<Block>();

        IsWork = false;
    }

    public event Action<Plane> TargetCheckPointReached;

    public Gun Gun { get; private set; }

    public Trunk Trunk { get; private set; }

    public bool IsWork { get; private set; }

    public void Prepare(Field field, CartrigeBox cartrigeBox)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        int amountBullets = _field.AmountLayers * _field.AmountColumns * _amountDestroyedRows;
        Gun.Upload(amountBullets);
        Trunk.SetCartrigeBox(cartrigeBox);
        IsWork = true;
    }

    public override void SetDirectionForward(Vector3 forward)
    {
        base.SetDirectionForward(forward);

        Gun.SetDirectionForward(forward);
        Trunk.SetDirectionForward(forward);
    }

    public override void Destroy()
    {
        FinishShooting();
        Gun.Destroy();
        Trunk.Destroy();
        base.Destroy();
    }

    public void StartShooting()
    {
        _field.StopShiftModels();

        DetermineTargets();
        //Gun.ShootingEnded += OnShootingEnded;

        _stopwatch.SetNotificationInterval(_shotCooldown);
        _stopwatch.IntervalPassed += Shoot;
        _stopwatch.Start();
    }

    public void FinishShooting()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= Shoot;
        
        _targets.Clear();

        //Gun.ShootingEnded -= OnShootingEnded;
        Gun.Clear();
        Gun.Destroy();

        Trunk.DeleteCartrigeBox();
        IsWork = false;

        //_field?.ContinueShiftModels();

        //if (_field != null)
        //{
        //    _stopwatch.SetNotificationInterval(1.5f);
        //    _stopwatch.IntervalPassed += ContinueShiftBlocks;
        //    _stopwatch.Start();
        //}
    }

    private void OnShootingEnded(Gun _)
    {
        FinishShooting();
    }

    private void Shoot()
    {
        Gun.Shoot(_targets.Dequeue());

        if (_targets.Count == 0)
        {
            FinishShooting();
        }
    }

    private void DetermineTargets()
    {
        foreach (Model model in _field.GetModelsForPlane(_amountDestroyedRows))
        {
            if (model is Block block)
            {
                _targets.Enqueue(block);
            }
        }
    }

    public void ContinueShiftBlocks()
    {
        //_stopwatch.IntervalPassed -= ContinueShiftBlocks;
        //_stopwatch.Stop();

        _field?.ContinueShiftModels();
    }
}