using UnityEngine;

public class CartrigeBoxFieldInformer : ElementInformer
{
    [SerializeField] private AmountDisplay _cartrigeBoxAmountDisplay;
    [SerializeField] private BezierCurveLineRenderer _cartrigeBoxBorderRenderer;
    [SerializeField] private BorderSettings _borderSettings;

    private float _height;

    private FieldBoundaryPlacer _fieldBoundaryPlacer;

    private CartrigeBoxField _cartrigeBoxField;

    protected override void PrepareForInit(float height)
    {
        _height = height;

        _fieldBoundaryPlacer = new FieldBoundaryPlacer();

        _cartrigeBoxBorderRenderer.Init();
    }

    protected override void PrepareForShowing(CreatedSignal<Level> levelCreatedSignal)
    {
        _cartrigeBoxField = levelCreatedSignal.Creatable.CartrigeBoxField;
        _cartrigeBoxAmountDisplay.Init(_cartrigeBoxField);
    }

    protected override void Show()
    {
        _cartrigeBoxAmountDisplay.On();
        _cartrigeBoxBorderRenderer.DrawBorders(_fieldBoundaryPlacer.PlaceBezierCurve(_cartrigeBoxField,
                                                                                     _borderSettings,
                                                                                     _height));
    }

    protected override void Hide()
    {
        _cartrigeBoxAmountDisplay.Off();
        _cartrigeBoxBorderRenderer.Clear();
    }
}