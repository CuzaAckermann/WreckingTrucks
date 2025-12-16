using UnityEngine;

public class PlanePresenter : Presenter
{
    [SerializeField] private GunPresenter _gunPresenter;
    [SerializeField] private TrunkPresenter _trunkPresenter;
    [SerializeField] private GameObjectTriggerDetector _triggerDetector;

    private TriggerDetector<ShootingSpace> _shootingSpaceTriggerDetector;

    private bool _isSubscribedToGunPresenter;
    private bool _isSubscribedToShootingSpaceTriggerDetector;

    private bool _isInitialized = false;

    public override void Init()
    {
        _shootingSpaceTriggerDetector = new TriggerDetector<ShootingSpace>(_triggerDetector);

        base.Init();

        _gunPresenter.Init();
        _trunkPresenter.Init();

        _isInitialized = true;
    }

    public override void Bind(Model model)
    {
        base.Bind(model);

        if (model is Plane plane)
        {
            _gunPresenter.Bind(plane.Gun);
            _trunkPresenter.Bind(plane.Trunk);
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
            if (_isSubscribedToGunPresenter == false)
            {
                //_gunPresenter.ShootingEnded += OnShootingEnded;

                _isSubscribedToGunPresenter = true;
            }

            if (_isSubscribedToShootingSpaceTriggerDetector == false)
            {
                _shootingSpaceTriggerDetector.Detected += OnDetected;
                _shootingSpaceTriggerDetector.Leaved += OnLeaved;

                _shootingSpaceTriggerDetector.Enable();

                _isSubscribedToShootingSpaceTriggerDetector = true;
            }
        }
    }

    private void UnsubscribeFromElements()
    {
        if (_isInitialized)
        {
            if (_isSubscribedToGunPresenter)
            {
                //_gunPresenter.ShootingEnded -= OnShootingEnded;

                _isSubscribedToGunPresenter = false;
            }

            if (_isSubscribedToShootingSpaceTriggerDetector)
            {
                _shootingSpaceTriggerDetector.Disable();

                _shootingSpaceTriggerDetector.Detected -= OnDetected;
                _shootingSpaceTriggerDetector.Leaved -= OnLeaved;

                _isSubscribedToShootingSpaceTriggerDetector = false;
            }
        }
    }

    private void OnDetected(ShootingSpace _)
    {
        if (Model is Plane plane)
        {
            plane.StartShooting();
        }
    }

    private void OnLeaved()
    {
        if (Model is Plane plane)
        {
            plane.StopShooting();
        }
    }
}