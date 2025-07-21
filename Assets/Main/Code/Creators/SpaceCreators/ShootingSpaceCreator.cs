using System;

public class ShootingSpaceCreator
{
    private readonly BulletSimulationCreator _bulletSimulationCreator;
    private readonly MoverCreator _moverCreator;
    private readonly RotatorCreator _rotatorCreator;

    public ShootingSpaceCreator(BulletSimulationCreator bulletSimulationCreator,
                                MoverCreator moverCreator,
                                RotatorCreator rotatorCreator)
    {
        _bulletSimulationCreator = bulletSimulationCreator ?? throw new ArgumentNullException(nameof(bulletSimulationCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _rotatorCreator = rotatorCreator ?? throw new ArgumentNullException(nameof(rotatorCreator));
    }

    public ShootingSpace Create(ShootingSpaceSettings shootingSpaceSettings)
    {
        BulletSimulation bulletSimulation = _bulletSimulationCreator.Create();

        return new ShootingSpace(bulletSimulation,
                                 _moverCreator.Create(bulletSimulation,
                                                      shootingSpaceSettings.MoverSettings),
                                 _rotatorCreator.Create(bulletSimulation,
                                                        shootingSpaceSettings.RotatorSettings));
    }
}