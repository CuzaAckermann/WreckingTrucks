using System;

public class LinearMoverCreator : IMoverCreator
{
    private readonly float _movespeedSettings;

    public LinearMoverCreator(float movespeedSettings)
    {
        _movespeedSettings = movespeedSettings > 0 ? movespeedSettings : throw new ArgumentOutOfRangeException(nameof(movespeedSettings));
    }

    public IMover Create(PositionManipulator positionManipulator)
    {
        return new LinearMover(positionManipulator, _movespeedSettings);
    }
}