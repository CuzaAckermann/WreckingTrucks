using UnityEngine;
using Random = UnityEngine.Random;

public class SoundInformer : MonoBehaviourSubscriber
{
    [SerializeField] private AudioClip _shootingSound;
    [SerializeField] private AudioSource _shootingSoundSource;
    [SerializeField] private float _soundOfVolume;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    private EventBus _eventBus;

    //private List<Gun> _guns;

    public void Init(EventBus eventBus)
    {
        Validator.ValidateNotNull(eventBus);

        _eventBus = eventBus;
        //_guns = new List<Gun>();

        _shootingSoundSource.clip = _shootingSound;

        Init();
    }

    protected override void Subscribe()
    {
        _eventBus.Subscribe<CreatedSignal<IDestroyable>>(OnModelCreated);

        //foreach (Gun gun in _guns)
        //{
        //    SubscribeToGun(gun);
        //}
    }

    protected override void Unsubscribe()
    {
        _eventBus.Subscribe<CreatedSignal<IDestroyable>>(OnModelCreated);

        //foreach (Gun gun in _guns)
        //{
        //    UnsubscribeFromGun(gun);
        //}
    }

    private void SubscribeToGun(Gun gun)
    {
        gun.ShotFired += OnShotFired;
        gun.Destroyed += OnDestroyed;
    }

    private void UnsubscribeFromGun(Gun gun)
    {
        gun.ShotFired -= OnShotFired;
        gun.Destroyed -= OnDestroyed;
    }

    private void OnModelCreated(CreatedSignal<IDestroyable> createdSignal)
    {
        IDestroyable destroyable = createdSignal.Creatable;

        if (destroyable is not Gun gun)
        {
            return;
        }

        SubscribeToGun(gun);

        //if (_guns.Contains(gun) == false)
        //{
        //    SubscribeToGun(gun);
        //    _guns.Add(gun);
        //}
    }

    private void OnShotFired(Bullet _)
    {
        float pitch = Random.Range(_minPitch, _maxPitch);

        _shootingSoundSource.pitch = pitch;
        _shootingSoundSource.PlayOneShot(_shootingSound, _soundOfVolume);
    }

    private void OnDestroyed(IDestroyable destroyable)
    {
        if (Validator.IsRequiredType(destroyable, out Gun gun) == false)
        {
            return;
        }

        UnsubscribeFromGun(gun);
        //_guns.Remove(gun);
    }
}