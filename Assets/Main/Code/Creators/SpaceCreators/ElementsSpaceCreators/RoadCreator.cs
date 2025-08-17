using System;

public class RoadCreator
{
    private readonly BezierCurveSettings _roadSettings;

    public RoadCreator(BezierCurveSettings roadSettings)
    {
        _roadSettings = roadSettings ? roadSettings : throw new ArgumentNullException(nameof(roadSettings));
    }

    public Road Create(BezierCurve path)
    {
        return new Road(path, _roadSettings);
    }
}