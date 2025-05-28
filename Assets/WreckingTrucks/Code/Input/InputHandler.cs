using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode _interactableButton = KeyCode.Mouse0;
    [SerializeField] private float _inputCooldown = 0.1f;

    private Stopwatch _stopwatch;
    private bool _isInitialized = false;
    private bool _canHandle;

    public event Action InteractableButtonPressed;

    public void Initialize()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"{nameof(InputHandler)} has been initialized.");
        }

        _stopwatch = new Stopwatch(_inputCooldown);
        _canHandle = false;
        SubscribeToStopwatch();
        _stopwatch.Start();
        _isInitialized = true;
    }

    private void Awake()
    {
        Initialize();        
    }

    private void OnEnable()
    {
        SubscribeToStopwatch();
    }

    private void Update()
    {
        HandleInteractableButton();
        _stopwatch.Tick(Time.deltaTime);
    }

    private void OnDisable()
    {
        UnsubscribeFromStopwatch();
    }

    private void HandleInteractableButton()
    {
        if (_canHandle == false)
        {
            return;
        }

        if (Input.GetKeyDown(_interactableButton))
        {
            OnInteractableButtonPressed();
            _canHandle = false;
            _stopwatch.Start();
        }
    }

    private void OnInteractableButtonPressed()
    {
        InteractableButtonPressed?.Invoke();
    }

    private void OnIntervalPassed()
    {
        _canHandle = true;
        _stopwatch.Stop();
    }

    private void SubscribeToStopwatch()
    {
        if (_stopwatch != null)
        {
            _stopwatch.IntervalPassed += OnIntervalPassed;
        }
    }

    private void UnsubscribeFromStopwatch()
    {
        if (_stopwatch != null)
        {
            _stopwatch.IntervalPassed -= OnIntervalPassed;
        }
    }
}