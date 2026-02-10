using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShootingSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _shootingSound;
    [SerializeField] private AudioSource _shootingSoundSource;
    [SerializeField] private float _soundOfVolume;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    private EventBus _eventBus;

    //private List<Gun> _guns;

    private bool _isInitialized = false;

    public void Initialize(EventBus eventBus)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException("Initialized");
        }

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        //_guns = new List<Gun>();

        _shootingSoundSource.clip = _shootingSound;

        _isInitialized = true;

        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelCreated);

        //foreach (Gun gun in _guns)
        //{
        //    SubscribeToGun(gun);
        //}
    }

    private void Unsubscribe()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelCreated);

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

    private void OnModelCreated(CreatedSignal<Model> modelSignal)
    {
        Model model = modelSignal.Creatable;

        if (model is Gun gun)
        {
            SubscribeToGun(gun);

            //if (_guns.Contains(gun) == false)
            //{
            //    SubscribeToGun(gun);
            //    _guns.Add(gun);
            //}
        }
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