using UnityEngine;

public class TruckPresenter : Presenter
{
    [SerializeField] private GunPresenter _gunPresenter;
    [SerializeField] private TrunkPresenter _trunkPresenter;
    [SerializeField] private GameObjectTriggerDetector _triggerDetector;

    private SpaceDetectorWaitingState<ShootingSpace> _spaceDetectorWaitingState;
    private CompletionWaitingState _gunPresenterWaitingState;

    private TriggerDetector<ShootingSpace> _shootingSpaceTriggerDetector;

    private bool _isInitialized = false;

    public override void Init()
    {
        _shootingSpaceTriggerDetector = new TriggerDetector<ShootingSpace>(_triggerDetector);
        _spaceDetectorWaitingState = new SpaceDetectorWaitingState<ShootingSpace>(_shootingSpaceTriggerDetector);
        _gunPresenterWaitingState = new CompletionWaitingState(_gunPresenter);

        base.Init();

        _gunPresenter.Init();
        _trunkPresenter.Init();

        _isInitialized = true;
    }

    public override void Bind(Model model)
    {
        base.Bind(model);

        if (model is Truck truck)
        {
            _gunPresenter.Bind(truck.Gun);
            _trunkPresenter.Bind(truck.Trunk);
        }
    }

    protected override void Subscribe()
    {
        SubscribeToElements();

        base.Subscribe();
    }

    protected override void Unsubscribe()
    {
        UnsubscribeFromElements();

        base.Unsubscribe();
    }

    protected override void OnPositionChanged()
    {
        base.OnPositionChanged();

        _gunPresenter.ChangePosition();
        _trunkPresenter.ChangePosition();
    }

    private void SubscribeToElements()
    {
        if (_isInitialized)
        {
            _gunPresenterWaitingState.Enter(OnShootingEnded);
            _spaceDetectorWaitingState.Enter(OnDetected, OnLeaved);
        }
    }

    private void UnsubscribeFromElements()
    {
        if (_isInitialized)
        {
            _gunPresenterWaitingState.Exit();
            _spaceDetectorWaitingState.Exit();
        }
    }

    private void OnDetected(ShootingSpace _)
    {
        if (Model is Truck truck)
        {
            truck.StartShooting();
        }
    }

    private void OnLeaved()
    {
        if (Model is Truck truck)
        {
            truck.StopShooting();
        }
    }

    private void OnShootingEnded()
    {
        _gunPresenter.SetTargetRotation(Model.Placeable.Position + Vector3.right);
    }
}