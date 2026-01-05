using System;

public class ColorShootingState
{
    private BlockTracker _blockTracker;
    private Gun _gun;
    private ColorType _destroyableColor;

    public void Enter(BlockTracker blockTracker,
                      Gun gun,
                      ColorType colorType)
    {
        if (colorType == ColorType.Unknown)
        {
            throw new ArgumentException(nameof(colorType));
        }

        _blockTracker = blockTracker ?? throw new ArgumentNullException(nameof(blockTracker));
        _gun = gun ?? throw new ArgumentNullException(nameof(gun));
        _destroyableColor = colorType;

        _gun.Upload();

        Shoot();
    }

    public void Exit()
    {
        if (_blockTracker != null)
        {
            _blockTracker.TargetDetected -= OnTargetDetected;
        }

        if (_gun != null)
        {
            _gun.StopShooting();
            _gun.ReadyToFire -= OnReadyToFire;
        }
    }

    private void Shoot()
    {
        if (_gun.CanShoot())
        {
            if (_blockTracker.TryGetTarget(_destroyableColor, out Block target))
            {
                _gun.ReadyToFire += OnReadyToFire;
                _gun.Aim(target);
            }
            else
            {
                _blockTracker.TargetDetected += OnTargetDetected;
            }
        }
        else
        {
            Exit();
        }
    }

    private void OnTargetDetected()
    {
        _blockTracker.TargetDetected -= OnTargetDetected;

        Shoot();
    }

    private void OnReadyToFire()
    {
        _gun.ReadyToFire -= OnReadyToFire;

        Shoot();
    }
}