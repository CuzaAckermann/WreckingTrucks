using System.Collections.Generic;
using UnityEngine;

public class ShootingSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _shootingSound;
    [SerializeField] private AudioSource _shootingSoundSource;
    [SerializeField] private float _soundOfVolume;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    private List<Gun> _guns;

    public void Initialize()
    {
        _guns = new List<Gun>();
        _shootingSoundSource.clip = _shootingSound;
    }

    private void OnEnable()
    {
        if (_guns != null)
        {
            SubscribeToGuns();
        }
    }

    private void OnDisable()
    {
        if (_guns != null)
        {
            UnsubscribeFromGuns();
        }
    }

    public void AddGun(Gun gun)
    {
        SubscribeToGun(gun);
        _guns.Add(gun);
    }

    public void Remove(Gun gun)
    {
        UnsubscribeFromGun(gun);
        _guns.Remove(gun);
    }

    public void SubscribeToGuns()
    {
        foreach (Gun gun in _guns)
        {
            SubscribeToGun(gun);
        }
    }

    public void UnsubscribeFromGuns()
    {
        foreach (Gun gun in _guns)
        {
            UnsubscribeFromGun(gun);
        }
    }

    private void SubscribeToGun(Gun gun)
    {
        gun.ShotFired += OnShotFired;
    }

    private void UnsubscribeFromGun(Gun gun)
    {
        gun.ShotFired -= OnShotFired;
    }

    private void OnShotFired(Bullet bullet)
    {
        float pitch = Random.Range(_minPitch, _maxPitch);

        _shootingSoundSource.pitch = pitch;
        _shootingSoundSource.PlayOneShot(_shootingSound, _soundOfVolume);
    }
}