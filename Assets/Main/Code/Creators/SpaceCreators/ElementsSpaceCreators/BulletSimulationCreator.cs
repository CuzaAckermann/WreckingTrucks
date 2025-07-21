using System;

public class BulletSimulationCreator
{
    private readonly ChargerCreator _chargerCreator;

    public BulletSimulationCreator(ChargerCreator chargerCreator)
    {
        _chargerCreator = chargerCreator ?? throw new ArgumentNullException(nameof(chargerCreator));
    }

    public BulletSimulation Create()
    {
        return new BulletSimulation(_chargerCreator.Create());
    }
}