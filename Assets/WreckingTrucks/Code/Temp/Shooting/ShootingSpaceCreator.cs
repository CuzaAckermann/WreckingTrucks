using UnityEngine;

public class ShootingSpaceCreator : MonoBehaviour
{
    [Header("Gun Rotator Settings")]
    [SerializeField, Min(1)] private int _capacityRotatables = 5;
    [SerializeField, Min(1)] private int _rotationSpeed = 720;
    [SerializeField, Min(0.01f)] private int _minAngle = 1;

    [Header("Bullet Mover Settings")]
    [SerializeField, Min(1)] private int _capacityMovables = 10;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 35;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPosition = 0.001f;

    [Header("Bullet Presenter Factory")]
    [SerializeField] private BulletPresenterFactory _bulletPresenterFactory;

    public void Initialize()
    {
        _bulletPresenterFactory.Initialize();
    }

    public ShootingSpace CreateShootingSpace()
    {
        BulletSimulation bulletSimulation = CreateBulletSimulation();
        Rotator rotator = CreateRotator(bulletSimulation);
        Mover mover = CreateMover(bulletSimulation);
        PresentersProduction<Bullet> bulletProduction = CreateBulletPresenterProduction();
        ModelPresenterBinder modelPresenterBinder = CreateModelPresenterBinder(bulletSimulation, bulletProduction);

        return new ShootingSpace(bulletSimulation,
                                 rotator,
                                 mover,
                                 modelPresenterBinder);
    }

    private BulletSimulation CreateBulletSimulation()
    {
        return new BulletSimulation();
    }

    private Mover CreateMover(BulletSimulation bulletSimulation)
    {
        return new Mover(bulletSimulation,
                         _capacityMovables,
                         _movementSpeed,
                         _minSqrDistanceToTargetPosition);
    }

    private Rotator CreateRotator(BulletSimulation bulletSimulation)
    {
        return new Rotator(bulletSimulation,
                           _rotationSpeed,
                           _minAngle,
                           _capacityRotatables);
    }

    private PresentersProduction<Bullet> CreateBulletPresenterProduction()
    {
        PresentersProduction<Bullet> bulletProduction = new PresentersProduction<Bullet>();
        bulletProduction.AddFactory<Bullet>(_bulletPresenterFactory);

        return bulletProduction;
    }

    private ModelPresenterBinder CreateModelPresenterBinder(IModelAddedNotifier modelAddedNotifier,
                                                            IModelPresenterCreator modelPresenterCreator)
    {
        return new ModelPresenterBinder(modelAddedNotifier, modelPresenterCreator);
    }
}