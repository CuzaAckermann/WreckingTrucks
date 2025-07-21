using System;

public class RoadCreator
{
    private readonly PathCreator _pathCreator;

    public RoadCreator(PathCreator pathCreator)
    {
        _pathCreator = pathCreator ?? throw new ArgumentNullException(nameof(pathCreator));
    }

    public Road Create(PathSettings pathSettings)
    {
        return new Road(_pathCreator.CreatePath(pathSettings));
    }
}