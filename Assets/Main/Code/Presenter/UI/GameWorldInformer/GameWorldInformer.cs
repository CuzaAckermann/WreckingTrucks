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

        _eventBus.Subscribe<CreatedSignal<CartrigeBoxField>>(SetCartrigeBoxField);
        _eventBus.Subscribe<CreatedSignal<BlockField>>(SetBlockField);
        _eventBus.Subscribe<CreatedSignal<PlaneSlot>>(SetPlaneSlot);

        Hide(new ClearedSignal<GameWorld>());
    }

    public event Action<ITickable> TickableCreated;

    private void OnEnable()
    {
        if (_gameWorld != null)
        {
            _eventBus.Subscribe<CreatedSignal<GameWorld>>(SetGameWorld);

            _eventBus.Subscribe<CreatedSignal<CartrigeBoxField>>(SetCartrigeBoxField);
            _eventBus.Subscribe<CreatedSignal<BlockField>>(SetBlockField);
            _eventBus.Subscribe<CreatedSignal<PlaneSlot>>(SetPlaneSlot);

            _eventBus.Subscribe<ClearedSignal<GameWorld>>(Hide);
        }
    }

    private void OnDisable()
    {
        if (_gameWorld != null)
        {
            _eventBus.Unsubscribe<CreatedSignal<GameWorld>>(SetGameWorld);

            _eventBus.Unsubscribe<CreatedSignal<CartrigeBoxField>>(SetCartrigeBoxField);
            _eventBus.Unsubscribe<CreatedSignal<BlockField>>(SetBlockField);
            _eventBus.Unsubscribe<CreatedSignal<PlaneSlot>>(SetPlaneSlot);

            _eventBus.Unsubscribe<ClearedSignal<GameWorld>>(Hide);
        }
    }

    private void SetGameWorld(CreatedSignal<GameWorld> createdSignal)
    {
        _gameWorld = createdSignal.Creatable;

        _eventBus.Subscribe<ClearedSignal<GameWorld>>(Hide);

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

    private void Hide(ClearedSignal<GameWorld> _)
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

    private void SetBlockField(CreatedSignal<BlockField> blockFieldCreatedSignal)
    {
        _blockField = blockFieldCreatedSignal.Creatable;

        _amountBlocksInField.Init(_blockField);
    }

    private void SetCartrigeBoxField(CreatedSignal<CartrigeBoxField> createdCartrigeBoxFieldSignal)
    {
        _cartrigeBoxField = createdCartrigeBoxFieldSignal.Creatable;

        _cartrigeBoxAmountDisplay.Init(_cartrigeBoxField);
    }

    private void SetPlaneSlot(CreatedSignal<PlaneSlot> createdPlaneSlotSignal)
    {
        _planeSlot = createdPlaneSlotSignal.Creatable;

        _planeAmountOfUsesDisplay.Init(_planeSlot);
    }
}