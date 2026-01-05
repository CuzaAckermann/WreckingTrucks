using System;
using System.Collections.Generic;

public class ShootingState
{
    private Field _field;

    private Queue<Block> _targets;

    private Gun _gun;

    public void Enter(Field field,
                      Queue<Block> targets,
                      Gun gun)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _targets = targets ?? throw new ArgumentNullException(nameof(targets));
        _gun = gun ?? throw new ArgumentNullException(nameof(gun));

        _gun.Upload(_targets.Count);

        _field.StopShiftModels();

        Shoot();
    }

    public void Exit()
    {
        if (_gun != null)
        {
            _gun.StopShooting();
            _gun.ReadyToFire -= OnReadyToFire;
        }

        _targets?.Clear();

        _field?.ContinueShiftModels();
    }

    private void Shoot()
    {
        if (_gun.CanShoot())
        {
            if (_targets.Count > 0)
            {
                _gun.ReadyToFire += OnReadyToFire;
                _gun.Aim(_targets.Dequeue());
            }
            else
            {
                Exit();
            }
        }
        else
        {
            Exit();
        }
    }

    private void OnReadyToFire()
    {
        _gun.ReadyToFire -= OnReadyToFire;

        Shoot();
    }
}