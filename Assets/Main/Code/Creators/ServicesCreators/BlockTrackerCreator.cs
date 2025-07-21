using System;

public class BlockTrackerCreator
{
    private readonly float _acceptableAngle;

    public BlockTrackerCreator(float acceptableAngle)
    {
        if (acceptableAngle <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(acceptableAngle));
        }

        _acceptableAngle = acceptableAngle;
    }

    public BlockTracker Create()
    {
        return new BlockTracker(_acceptableAngle);
    }
}