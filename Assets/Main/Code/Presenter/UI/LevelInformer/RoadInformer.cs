using UnityEngine;

public class RoadInformer : ElementInformer
{
    [SerializeField] private BezierCurve _road;
    [SerializeField] private RoadRenderer _roadRenderer;

    protected override void PrepareForInit(float height)
    {
        _road.Init();

        _roadRenderer.Init(_road.CurvePoints, height);
    }

    protected override void PrepareForShowing(CreatedSignal<Level> levelCreatedSignal)
    {
        Logger.Log($"{nameof(PrepareForShowing)} is empty");
    }

    protected override void Show()
    {
        _roadRenderer.Draw();
    }

    protected override void Hide()
    {
        _roadRenderer.Hide();
    }
}