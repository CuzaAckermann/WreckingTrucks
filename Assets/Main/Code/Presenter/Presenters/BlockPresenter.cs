using System;
using UnityEngine;

public class BlockPresenter : Presenter
{
    [SerializeField] private Jelly _jelly;
    [SerializeField] private bool _isTarget;

    private EventBus _eventBus;

    private bool _isManipulated;

    public override void Bind(Model model)
    {
        base.Bind(model);

        _jelly.Settle();
        _isManipulated = false;
    }

    public override void Init()
    {
        base.Init();

        _jelly.Initialize();
    }

    public void SetEventBus(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    protected override void Subscribe()
    {
        if (Model is Block block)
        {
            _jelly.Settle();

            block.TargetStateChanged += OnTargetStateChanged;

            block.PositionManipulator.PositionChanged += OnManipulationStarted;
            block.PositionManipulator.RotationChanged += OnManipulationStarted;

            block.Mover.TargetReached += OnManipulationCompleted;
            block.Rotator.TargetReached += OnManipulationCompleted;
        }

        base.Subscribe();
    }

    protected override void Unsubscribe()
    {
        if (Model is Block block)
        {
            block.TargetStateChanged -= OnTargetStateChanged;

            block.PositionManipulator.PositionChanged -= OnManipulationStarted;
            block.PositionManipulator.RotationChanged -= OnManipulationStarted;

            block.Mover.TargetReached -= OnManipulationCompleted;
            block.Rotator.TargetReached -= OnManipulationCompleted;
        }

        base.Unsubscribe();
    }

    protected override void ResetState()
    {
        base.ResetState();

        _jelly.Settle();
    }

    private void OnManipulationStarted()
    {
        if (_isManipulated == false)
        {
            _isManipulated = true;

            _eventBus.Invoke(new JellyShakedSignal(_jelly));
        }
    }

    private void OnManipulationCompleted(ITargetAction _)
    {
        if (_isManipulated)
        {
            _isManipulated = false;
        }
    }

    private void OnTargetStateChanged()
    {
        if (Model is Block block)
        {
            _isTarget = block.IsTargetForShooting;
        }
    }
}