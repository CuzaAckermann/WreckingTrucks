using System;

public class RoadCreator
{
    private readonly BezierCurve _mainPath;
    private readonly BezierCurveSettings _roadSettings;

    public RoadCreator(BezierCurve bezierCurve, BezierCurveSettings roadSettings)
    {
        _mainPath = bezierCurve ? bezierCurve : throw new ArgumentNullException(nameof(bezierCurve));
        _roadSettings = roadSettings ? roadSettings : throw new ArgumentNullException(nameof(roadSettings));
    }

    public Road Create()
    {
        return new Road(_mainPath, _roadSettings);
    }
}