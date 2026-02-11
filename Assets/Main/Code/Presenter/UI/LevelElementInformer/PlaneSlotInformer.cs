using UnityEngine;

public class PlaneSlotInformer : ElementInformer
{
    [SerializeField] private SlotBorderSettings _slotBorderSettings;
    [SerializeField] private AmountDisplay _planeAmountOfUsesDisplay;
    [SerializeField] private Transform _planeSlotPosition;
    [SerializeField] private BezierCurveLineRenderer _planeSlotBorderRenderer;

    private SlotBoundaryPlacer _slotBoundaryPlacer;
    private IAmount _remainingUses;
    private ModelSlot<Plane> _planeSlot;
    private float _height;

    protected override void PrepareForInit(float height)
    {
        _height = height;
        _slotBoundaryPlacer = new SlotBoundaryPlacer();

        _planeSlotBorderRenderer.Init();
    }

    protected override void PrepareForShowing(CreatedSignal<Level> levelCreatedSignal)
    {
        _planeSlot = levelCreatedSignal.Creatable.PlaneSlot;

        _planeAmountOfUsesDisplay.Init(_planeSlot.RemainingUses);
    }

    protected override void Show()
    {
        _planeAmountOfUsesDisplay.On();

        _planeSlotBorderRenderer.DrawBorders(_slotBoundaryPlacer.PlaceBezierCurve(_planeSlotPosition,
                                                                                  _slotBorderSettings,
                                                                                  _height));
    }

    protected override void Hide()
    {
        _planeAmountOfUsesDisplay.Off();
        _planeSlotBorderRenderer.Clear();
    }
}