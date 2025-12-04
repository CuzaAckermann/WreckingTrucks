using System;
using UnityEngine;

public class BlockPresenter : Presenter
{
    [SerializeField] private Jelly _jelly;
    [SerializeField] private bool _isTarget;

    private bool _isManipulated;

    public event Action<BlockPresenter> ManipulationStarted;
    public event Action<BlockPresenter> ManipulationCompleted;

    public event Action<BlockPresenter> HesitationFinished;

    public Jelly Jelly => _jelly;

    public override void Bind(Model model)
    {
        base.Bind(model);

        _jelly.Settle();
    }

    public override void Init()
    {
        base.Init();

        _jelly.Initialize();
    }

    protected override void Subscribe()
    {
        if (Model is Block block)
        {
            _jelly.Settle();

            block.TargetStateChanged += OnTargetStateChanged;

            block.PositionChanged += OnManipulationStarted;
            block.RotationChanged += OnManipulationStarted;

            block.TargetPositionReached += OnManipulationCompleted;
            block.TargetRotationReached += OnManipulationCompleted;

            _jelly.HesitationFinished += OnHesitationFinished;
        }

        base.Subscribe();
    }

    protected override void Unsubscribe()
    {
        if (Model is Block block)
        {
            block.TargetStateChanged -= OnTargetStateChanged;

            block.PositionChanged -= OnManipulationStarted;
            block.RotationChanged -= OnManipulationStarted;

            block.TargetPositionReached -= OnManipulationCompleted;
            block.TargetRotationReached -= OnManipulationCompleted;

            _jelly.HesitationFinished -= OnHesitationFinished;
        }

        base.Unsubscribe();
    }

    private void OnManipulationStarted()
    {
        if (_isManipulated == false)
        {
            _isManipulated = true;
            ManipulationStarted?.Invoke(this);
        }
    }

    private void OnManipulationCompleted(Model _)
    {
        if (_isManipulated)
        {
            _isManipulated = false;
            ManipulationCompleted?.Invoke(this);
        }
    }

    private void OnTargetStateChanged()
    {
        if (Model is Block block)
        {
            _isTarget = block.IsTargetForShooting;
        }
    }

    private void OnHesitationFinished()
    {
        HesitationFinished?.Invoke(this);
    }
}