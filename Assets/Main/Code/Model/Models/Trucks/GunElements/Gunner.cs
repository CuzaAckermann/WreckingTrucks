using System;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Model
{
    public Gunner(float moveSpeed,
                  float rotationSpeed)
           : base(moveSpeed,
                  rotationSpeed)
    {
        
    }

    public event Action Aimed;

    public Turret Turret { get; private set; }

    public void Prepare(Turret turret)
    {
        Turret = turret ?? throw new ArgumentNullException(nameof(turret));
    }

    public override void Destroy()
    {
        Turret.Destroy();

        base.Destroy();
    }

    public void AimAtTarget(Model target)
    {
        Turret.Aimed += OnAimed;

        Turret.SetTarget(target);
    }

    private void OnAimed()
    {
        Turret.Aimed -= OnAimed;

        Aimed?.Invoke();
    }
}