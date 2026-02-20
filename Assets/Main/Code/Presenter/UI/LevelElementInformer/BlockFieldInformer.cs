using UnityEngine;

public class BlockFieldInformer : ElementInformer
{
    //[SerializeField] private SliderDisplay _amountBlocksInField;
    [SerializeField] private BezierCurveLineRenderer _blockBorderRenderer;
    [SerializeField] private BorderSettings _borderSettings;

    private float _height;

    private FieldBoundaryPlacer _fieldBoundaryPlacer;

    private BlockField _blockField;
    //private SmoothValueFollower _smoothValueFollower;

    public void Init1(SmoothValueFollower smoothValueFollower)
    {
        Validator.ValidateNotNull(smoothValueFollower);

        //_smoothValueFollower = smoothValueFollower;
    }

    protected override void PrepareForInit(float height)
    {
        _height = height;

        _fieldBoundaryPlacer = new FieldBoundaryPlacer();

        _blockBorderRenderer.Init();
    }

    protected override void PrepareForShowing(CreatedSignal<Level> levelCreatedSignal)
    {
        _blockField = levelCreatedSignal.Creatable.BlockField;

        //_smoothValueFollower.SetTarget(_blockField.ModelCount);

        //_amountBlocksInField.Init(_blockField.ModelCount, _smoothValueFollower);
    }

    protected override void Show()
    {
        //_amountBlocksInField.On();
        _blockBorderRenderer.DrawBorders(_fieldBoundaryPlacer.PlaceBezierCurve(_blockField,
                                                                               _borderSettings,
                                                                               _height));
    }

    protected override void Hide()
    {
        //_amountBlocksInField.Off();
        _blockBorderRenderer.Clear();
    }
}