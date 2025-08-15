using UnityEngine;

public class GameWorldInformer : MonoBehaviour
{
    [SerializeField] private AmountDisplay _cartrigeBoxAmountDisplay;
    [SerializeField] private BorderSettings _borderSettings;
    [SerializeField] private BezierCurve _road;

    [Header("Borders")]
    [SerializeField] private BezierCurveLineRenderer _blockBorderRenderer;
    [SerializeField] private BezierCurveLineRenderer _cartrigeBoxBorderRenderer;
    [SerializeField] private RoadRenderer _roadRenderer;

    private Transform _transform;
    private FieldBoundaryPlacer _fieldBoundaryPlacer;

    public void Initialize(IAmountChangedNotifier cartrigeBoxAmountNotifier)
    {
        _transform = transform;
        _cartrigeBoxAmountDisplay.Initialize(cartrigeBoxAmountNotifier);
        _fieldBoundaryPlacer = new FieldBoundaryPlacer();
        _blockBorderRenderer.Initialize();
        _cartrigeBoxBorderRenderer.Initialize();
        _road.Initialize();
        _roadRenderer.Initialize(_road.CurvePoints, _transform.position.y);
    }

    public void Show(Field blockField, Field cartrigeBoxField)
    {
        _cartrigeBoxAmountDisplay.On();

        _blockBorderRenderer.DrawBorders(_fieldBoundaryPlacer.PlaceBezierCurve(blockField,
                                                                               _borderSettings,
                                                                               _transform.position.y));
        _cartrigeBoxBorderRenderer.DrawBorders(_fieldBoundaryPlacer.PlaceBezierCurve(cartrigeBoxField,
                                                                                     _borderSettings,
                                                                                     _transform.position.y));
        _roadRenderer.Draw();
    }

    public void Hide()
    {
        _cartrigeBoxAmountDisplay.Off();
        _blockBorderRenderer.Clear();
        _cartrigeBoxBorderRenderer.Clear();
        _roadRenderer.Hide();
    }
}