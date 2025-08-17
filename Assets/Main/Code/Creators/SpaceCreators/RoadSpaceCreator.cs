using System;

public class RoadSpaceCreator
{
    private readonly RoadCreator _roadCreator;
    private readonly MoverCreator _moverCreator;
    private readonly RotatorCreator _rotatorCreator;

    public RoadSpaceCreator(RoadCreator roadCreator,
                            MoverCreator moverCreator,
                            RotatorCreator rotatorCreator)
    {
        _roadCreator = roadCreator ?? throw new ArgumentNullException(nameof(roadCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _rotatorCreator = rotatorCreator ?? throw new ArgumentNullException(nameof(rotatorCreator));
    }

    public RoadSpace Create(RoadSpaceSettings roadSpaceSettings)
    {
        Road road = _roadCreator.Create(roadSpaceSettings.PathForTrucks);

        return new RoadSpace(road,
                             _moverCreator.Create(road,roadSpaceSettings.MoverSettings),
                             _rotatorCreator.Create(road, roadSpaceSettings.RotatorSettings));
    }
}