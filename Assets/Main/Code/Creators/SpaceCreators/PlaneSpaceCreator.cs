using System;

public class PlaneSpaceCreator
{
    private readonly MoverCreator _moverCreator;
    private readonly RotatorCreator _rotatorCreator;
    private readonly RoadCreator _roadCreator;
    private readonly PlaneFactory _planeFactory;

    public PlaneSpaceCreator(MoverCreator moverCreator,
                             RotatorCreator rotatorCreator,
                             RoadCreator roadCreator,
                             PlaneFactory planeFactory)
    {
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _rotatorCreator = rotatorCreator ?? throw new ArgumentNullException(nameof(rotatorCreator));
        _roadCreator = roadCreator ?? throw new ArgumentNullException(nameof(roadCreator));
        _planeFactory = planeFactory ?? throw new ArgumentNullException(nameof(planeFactory));
    }

    public PlaneSpace Create(PlaneSpaceSettings planeSpaceSettings)
    {
        PlaneSlot planeSlot = new PlaneSlot(_planeFactory, planeSpaceSettings.PlaneSlotPosition);
        Road road = _roadCreator.Create(planeSpaceSettings.PathForPlane);
        Mover mover = _moverCreator.Create(planeSlot, planeSpaceSettings.MoverSettings);
        Rotator rotator = _rotatorCreator.Create(planeSlot, planeSpaceSettings.RotatorSettings);

        mover.AddNotifier(road);
        rotator.AddNotifier(road);

        return new PlaneSpace(planeSlot,
                              road,
                              mover,
                              rotator);
    }
}