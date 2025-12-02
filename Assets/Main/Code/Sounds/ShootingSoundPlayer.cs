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

    private ModelProduction _modelProduction;
    //private List<Gun> _guns;

    private bool _isInitialized = false;
    private bool _isSubscribed = false;

    public void Initialize(ModelProduction modelProduction)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException("Initialized");
        }

        _modelProduction = modelProduction ?? throw new ArgumentNullException(nameof(modelProduction));
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
        if (_isSubscribed == false && _isInitialized)
        {
            //foreach (Gun gun in _guns)
            //{
            //    SubscribeToGun(gun);
            //}

            _modelProduction.ModelCreated += OnModelCreated;

            _isSubscribed = true;
        }
    }

    private void Unsubscribe()
    {
        if (_isSubscribed && _isInitialized)
        {
            _modelProduction.ModelCreated -= OnModelCreated;

            //foreach (Gun gun in _guns)
            //{
            //    UnsubscribeFromGun(gun);
            //}

            _isSubscribed = false;
        }
    }

    private void SubscribeToGun(Gun gun)
    {
        gun.ShotFired += OnShotFired;
        gun.Destroyed += OnDestroyed;
    }

    private void UnsubscribeFromGun(Gun gun)
    {
        gun.Destroyed -= OnDestroyed;
        gun.ShotFired -= OnShotFired;
    }

    private void OnModelCreated(Model model)
    {
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

    private void OnDestroyed(Model model)
    {
        if (model is Gun gun)
        {
            UnsubscribeFromGun(gun);
            //_guns.Remove(gun);
        }
    }
}