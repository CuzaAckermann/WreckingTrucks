using System;

public class RoadSpaceCreator
{
    private readonly RoadCreator _roadCreator;
    private readonly MoverCreator _moverCreator;
    private readonly RotatorCreator _rotatorCreator;
    private readonly ModelFinalizerCreator _modelFinalizerCreator;

    public RoadSpaceCreator(RoadCreator roadCreator,
                            MoverCreator moverCreator,
                            RotatorCreator rotatorCreator,
                            ModelFinalizerCreator modelFinalizerCreator)
    {
        _roadCreator = roadCreator ?? throw new ArgumentNullException(nameof(roadCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _rotatorCreator = rotatorCreator ?? throw new ArgumentNullException(nameof(rotatorCreator));
        _modelFinalizerCreator = modelFinalizerCreator ?? throw new ArgumentNullException(nameof(modelFinalizerCreator));
    }

    public RoadSpace Create(PathSettings pathSettings, RoadSpaceSettings roadSpaceSettings)
    {
        Road road = _roadCreator.Create(pathSettings);

        return new RoadSpace(road,
                             _moverCreator.Create(road,roadSpaceSettings.MoverSettings),
                             _rotatorCreator.Create(road, roadSpaceSettings.RotatorSettings),
                             _modelFinalizerCreator.Create());
    }
}