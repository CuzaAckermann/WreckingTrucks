using System;
using UnityEngine;

public class BlockFieldInformer : ElementInformer, ITickableCreator
{
    [SerializeField] private SmoothingAmountDisplay _amountBlocksInField;
    [SerializeField] private BezierCurveLineRenderer _blockBorderRenderer;
    [SerializeField] private BorderSettings _borderSettings;

    private float _height;

    private FieldBoundaryPlacer _fieldBoundaryPlacer;

    private BlockField _blockField;

    public event Action<ITickable> TickableCreated;

    protected override void PrepareForInit(float height)
    {
        _height = height;

        _fieldBoundaryPlacer = new FieldBoundaryPlacer();

        _blockBorderRenderer.Init();

        //TickableCreated?.Invoke(_amountBlocksInField);
    }

    protected override void PrepareForShowing(CreatedSignal<Level> levelCreatedSignal)
    {
        _blockField = levelCreatedSignal.Creatable.BlockField;

        //_amountBlocksInField.Init(_blockField);
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