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

    private EventBus _eventBus;

    private BlockField _blockField;
    private CartrigeBoxField _cartrigeBoxField;
    private PlaneSlot _planeSlot;

    public void Init(EventBus eventBus)
    {
        _transform = transform;
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _road.Init();

        _fieldBoundaryPlacer = new FieldBoundaryPlacer();
        _slotBoundaryPlacer = new SlotBoundaryPlacer();

        _blockBorderRenderer.Init();
        _cartrigeBoxBorderRenderer.Init();
        _roadRenderer.Init(_road.CurvePoints, _transform.position.y);
        _planeSlotBorderRenderer.Init();

        TickableCreated?.Invoke(_amountBlocksInField);

        _eventBus.Subscribe<CreatedSignal<GameWorld>>(SetGameWorld);

        _eventBus.Subscribe<CreatedCartrigeBoxFieldSignal>(SetCartrigeBoxField);
        _eventBus.Subscribe<CreatedBlockFieldSignal>(SetBlockField);
        _eventBus.Subscribe<CreatedPlaneSlotSignal>(SetPlaneSlot);

        Hide(new DestroyedGameWorldSignal());
    }

    public event Action<ITickable> TickableCreated;

    private void OnEnable()
    {
        if (_gameWorld != null)
        {
            _eventBus.Subscribe<CreatedSignal<GameWorld>>(SetGameWorld);

            _eventBus.Subscribe<CreatedCartrigeBoxFieldSignal>(SetCartrigeBoxField);
            _eventBus.Subscribe<CreatedBlockFieldSignal>(SetBlockField);
            _eventBus.Subscribe<CreatedPlaneSlotSignal>(SetPlaneSlot);

            _eventBus.Subscribe<DestroyedGameWorldSignal>(Hide);
        }
    }

    private void OnDisable()
    {
        if (_gameWorld != null)
        {
            _eventBus.Unsubscribe<CreatedSignal<GameWorld>>(SetGameWorld);

            _eventBus.Unsubscribe<CreatedCartrigeBoxFieldSignal>(SetCartrigeBoxField);
            _eventBus.Unsubscribe<CreatedBlockFieldSignal>(SetBlockField);
            _eventBus.Unsubscribe<CreatedPlaneSlotSignal>(SetPlaneSlot);

            _eventBus.Unsubscribe<DestroyedGameWorldSignal>(Hide);
        }
    }

    private void SetGameWorld(CreatedSignal<GameWorld> createdSignal)
    {
        _gameWorld = createdSignal.Creatable;

        _eventBus.Subscribe<DestroyedGameWorldSignal>(Hide);

        Show();
    }

    private void Show()
    {
        _amountBlocksInField.On();
        _cartrigeBoxAmountDisplay.On();
        _planeAmountOfUsesDisplay.On();

        _blockBorderRenderer.DrawBorders(_fieldBoundaryPlacer.PlaceBezierCurve(_blockField,
                                                                               _borderSettings,
                                                                               _transform.position.y));
        _cartrigeBoxBorderRenderer.DrawBorders(_fieldBoundaryPlacer.PlaceBezierCurve(_cartrigeBoxField,
                                                                                     _borderSettings,
                                                                                     _transform.position.y));
        _roadRenderer.Draw();
        _planeSlotBorderRenderer.DrawBorders(_slotBoundaryPlacer.PlaceBezierCurve(_planeSlotPosition,
                                                                                  _slotBorderSettings,
                                                                                  _transform.position.y));
    }

    private void Hide(DestroyedGameWorldSignal _)
    {
        _amountBlocksInField.Off();
        _cartrigeBoxAmountDisplay.Off();
        _planeAmountOfUsesDisplay.Off();
        _blockBorderRenderer.Clear();
        _cartrigeBoxBorderRenderer.Clear();
        _roadRenderer.Hide();
        _planeSlotBorderRenderer.Clear();

        _gameWorld = null;
    }

    private void SetBlockField(CreatedBlockFieldSignal createdBlockFieldSignal)
    {
        _blockField = createdBlockFieldSignal.BlockField;

        _amountBlocksInField.Init(_blockField);
    }

    private void SetCartrigeBoxField(CreatedCartrigeBoxFieldSignal createdCartrigeBoxFieldSignal)
    {
        _cartrigeBoxField = createdCartrigeBoxFieldSignal.CartrigeBoxField;

        _cartrigeBoxAmountDisplay.Init(_cartrigeBoxField);
    }

    private void SetPlaneSlot(CreatedPlaneSlotSignal createdPlaneSlotSignal)
    {
        _planeSlot = createdPlaneSlotSignal.PlaneSlot;

        _planeAmountOfUsesDisplay.Init(_planeSlot);
    }
}