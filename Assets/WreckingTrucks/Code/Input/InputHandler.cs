using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode _interactableButton = KeyCode.Mouse0;
    [SerializeField] private bool _isOneClick = true;

    private bool _isInitialized = false;

    public event Action InteractableButtonPressed;

    public void Initialize()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"{nameof(InputHandler)} has been initialized.");
        }

        _isInitialized = true;
    }

    private void Awake()
    {
        Initialize();        
    }

    private void Update()
    {
        HandleInteractableButton();
    }

    private void HandleInteractableButton()
    {
        if (_isOneClick)
        {
            if (Input.GetKeyDown(_interactableButton))
            {
                OnInteractableButtonPressed();
            }
        }
        else
        {
            if (Input.GetKey(_interactableButton))
            {
                OnInteractableButtonPressed();
            }
        }
    }

    private void OnInteractableButtonPressed()
    {
        InteractableButtonPressed?.Invoke();
    }
}