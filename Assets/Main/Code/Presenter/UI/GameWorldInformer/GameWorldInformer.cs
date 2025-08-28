using UnityEngine;

public class GameWorldInformer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BorderSettings _borderSettings;
    [SerializeField] private SlotBorderSettings _slotBorderSettings;

    [Header("Elements")]
    [SerializeField] private AmountDisplay _cartrigeBoxAmountDisplay;
    [SerializeField] private AmountDisplay _planeAmountOfUsesDisplay;
    [SerializeField] private BezierCurve _road;
    [SerializeField] private Transform _planeSlotPosition;

    [Header("Borders")]
    [SerializeField] private BezierCurveLineRenderer _blockBorderRenderer;
    [SerializeField] private BezierCurveLineRenderer _cartrigeBoxBorderRenderer;
    [SerializeField] private RoadRenderer _roadRenderer;
    [SerializeField] private BezierCurveLineRenderer _planeSlotBorderRenderer;

    private Transform _transform;
    private FieldBoundaryPlacer _fieldBoundaryPlacer;
    private SlotBoundaryPlacer _slotBoundaryPlacer;

    public void Initialize(IAmountChangedNotifier cartrigeBoxAmountNotifier,
                           IAmountChangedNotifier planeSlotAmountOfUses)
    {
        _transform = transform;

        _cartrigeBoxAmountDisplay.Initialize(cartrigeBoxAmountNotifier);
        _planeAmountOfUsesDisplay.Initialize(planeSlotAmountOfUses);
        _road.Initialize();

        _fieldBoundaryPlacer = new FieldBoundaryPlacer();
        _slotBoundaryPlacer = new SlotBoundaryPlacer();
        
        _blockBorderRenderer.Initialize();
        _cartrigeBoxBorderRenderer.Initialize();
        _roadRenderer.Initialize(_road.CurvePoints, _transform.position.y);
        _planeSlotBorderRenderer.Initialize();
    }

    public void Show(Field blockField, Field cartrigeBoxField)
    {
        _cartrigeBoxAmountDisplay.On();
        _planeAmountOfUsesDisplay.On();

        _blockBorderRenderer.DrawBorders(_fieldBoundaryPlacer.PlaceBezierCurve(blockField,
                                                                               _borderSettings,
                                                                               _transform.position.y));
        _cartrigeBoxBorderRenderer.DrawBorders(_fieldBoundaryPlacer.PlaceBezierCurve(cartrigeBoxField,
                                                                                     _borderSettings,
                                                                                     _transform.position.y));
        _roadRenderer.Draw();
        _planeSlotBorderRenderer.DrawBorders(_slotBoundaryPlacer.PlaceBezierCurve(_planeSlotPosition,
                                                                                  _slotBorderSettings,
                                                                                  _transform.position.y));
    }

    public void Hide()
    {
        _cartrigeBoxAmountDisplay.Off();
        _planeAmountOfUsesDisplay.Off();
        _blockBorderRenderer.Clear();
        _cartrigeBoxBorderRenderer.Clear();
        _roadRenderer.Hide();
        _planeSlotBorderRenderer.Clear();
    }
}