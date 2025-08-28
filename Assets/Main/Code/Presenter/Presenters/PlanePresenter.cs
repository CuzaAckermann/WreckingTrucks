using UnityEngine;

public class PlanePresenter : Presenter
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
        if (_isSubscribed == false)
        {
            //_gunPresenter.ShootingEnded += OnLeaved;

            _shootingTriggerDetector.Detected += OnDetected;
            _shootingTriggerDetector.Leaved += OnLeaved;
            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromElements()
    {
        if (_isSubscribed)
        {
            //_gunPresenter.ShootingEnded -= OnLeaved;

            _shootingTriggerDetector.Detected -= OnDetected;
            _shootingTriggerDetector.Leaved -= OnLeaved;
            _isSubscribed = false;
        }
    }

    private void OnDetected()
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
            Logger.Log("Prok");

            //plane.FinishShooting();
            plane.ContinueShiftBlocks();
        }
    }
}