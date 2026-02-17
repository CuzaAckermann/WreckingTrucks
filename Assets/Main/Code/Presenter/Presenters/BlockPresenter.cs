using System;
using UnityEngine;

public class BlockPresenter : Presenter
{
    [SerializeField] private Jelly _jelly;
    [SerializeField] private bool _isTarget;

    private bool _isManipulated;

    public override void Init()
    {
        base.Init();

        _jelly.Initialize();
    }

    public Jelly Jelly => _jelly;

    public override void Bind(Model model)
    {
        base.Bind(model);

        _jelly.Settle();
        _isManipulated = false;
    }

    protected override void Subscribe()
    {
        if (Model is Block block)
        {
            _jelly.Settle();

            block.TargetStateChanged += OnTargetStateChanged;

            block.Placeable.PositionChanged += OnManipulationStarted;
            block.Placeable.RotationChanged += OnManipulationStarted;

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

            block.Placeable.PositionChanged -= OnManipulationStarted;
            block.Placeable.RotationChanged -= OnManipulationStarted;

            block.Mover.TargetReached -= OnManipulationCompleted;
            block.Rotator.TargetReached -= OnManipulationCompleted;
        }

        base.Unsubscribe();
    }

    protected override void ResetState()
    {
        _jelly.Destroy();

        base.ResetState();
    }

    private void OnManipulationStarted()
    {
        if (_isManipulated == false)
        {
            _isManipulated = true;

            _jelly.StartShaking();
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