using UnityEngine;

public class TruckPresenter : Presenter
{
    [SerializeField] private GunPresenter _gunPresenter;
    [SerializeField] private TrunkPresenter _trunkPresenter;
    [SerializeField] private ShootingTriggerDetector _shootingTriggerDetector;

    private bool _isSubscribed;

    public override void InitializeComponents()
    {
        base.InitializeComponents();

        _gunPresenter.InitializeComponents();
        _trunkPresenter.InitializeComponents();
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
        if (_isSubscribed == false)
        {
            _gunPresenter.ShootingEnded += OnShootingEnded;

            _shootingTriggerDetector.Detected += OnDetected;
            _shootingTriggerDetector.Leaved += OnLeaved;
            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromElements()
    {
        if (_isSubscribed)
        {
            _gunPresenter.ShootingEnded -= OnShootingEnded;

            _shootingTriggerDetector.Detected -= OnDetected;
            _shootingTriggerDetector.Leaved -= OnLeaved;
            _isSubscribed = false;
        }
    }

    private void OnDetected()
    {
        if (Model is Truck truck)
        {
            truck.Shoot();
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
        _gunPresenter.SetTargetRotation(Model.Position + Vector3.right);
    }
}