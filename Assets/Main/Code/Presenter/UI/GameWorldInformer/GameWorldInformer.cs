using System;
using UnityEngine;

public class GameWorldInformer : MonoBehaviour, ITickableCreator
{
    [Header("Settings")]
    [SerializeField] private BorderSettings _borderSettings;
    [SerializeField] private SlotBorderSettings _slotBorderSettings;

    [Header("Elements")]
    [SerializeField] private SmoothingAmountDisplay _amountBlocksInField;
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

    private GameWorld _gameWorld;
    private bool _isSubscribed = false;

    public void Initialize()
    {
        _transform = transform;

        TickableCreated?.Invoke(_amountBlocksInField);

        Hide();
    }

    public event Action<ITickable> TickableCreated;

    public void ConnectGameWorld(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));

        _amountBlocksInField.Initialize(gameWorld.BlockField);
        _cartrigeBoxAmountDisplay.Init(gameWorld.CartrigeBoxField);
        _planeAmountOfUsesDisplay.Init(gameWorld.PlaneSlot);
        _road.Initialize();

        _fieldBoundaryPlacer = new FieldBoundaryPlacer();
        _slotBoundaryPlacer = new SlotBoundaryPlacer();

        _blockBorderRenderer.Initialize();
        _cartrigeBoxBorderRenderer.Initialize();
        _roadRenderer.Initialize(_road.CurvePoints, _transform.position.y);
        _planeSlotBorderRenderer.Initialize();

        SubscribeToGameWorld();

        Show(_gameWorld.BlockField, _gameWorld.CartrigeBoxField);
    }

    private void OnEnable()
    {
        SubscribeToGameWorld();
    }

    private void OnDisable()
    {
        UnsubscribeFromGameWorld();
    }

    private void Show(Field blockField, Field cartrigeBoxField)
    {
        _amountBlocksInField.On();
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

    private void SubscribeToGameWorld()
    {
        if (_gameWorld != null && _isSubscribed == false)
        {
            _gameWorld.Destroyed += Hide;

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromGameWorld()
    {
        if (_gameWorld != null && _isSubscribed)
        {
            _gameWorld.Destroyed -= Hide;

            _isSubscribed = false;
        }
    }

    private void Hide()
    {
        _amountBlocksInField.Off();
        _cartrigeBoxAmountDisplay.Off();
        _planeAmountOfUsesDisplay.Off();
        _blockBorderRenderer.Clear();
        _cartrigeBoxBorderRenderer.Clear();
        _roadRenderer.Hide();
        _planeSlotBorderRenderer.Clear();
    }
}