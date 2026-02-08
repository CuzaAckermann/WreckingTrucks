using System;

public class LinearRotatorCreator : IRotatorCreator
{
    private readonly float _rotationSpeedSettings;

    public LinearRotatorCreator(float rotationSpeedSettings)
    {
        _rotationSpeedSettings = rotationSpeedSettings > 0 ? rotationSpeedSettings : throw new ArgumentOutOfRangeException(nameof(rotationSpeedSettings));
    }

    public IRotator Create(PositionManipulator positionManipulator)
    {
        return new LinearRotator(positionManipulator, _rotationSpeedSettings);
    }
}
