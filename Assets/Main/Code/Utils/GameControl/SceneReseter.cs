using System;
using UnityEngine;

public class SceneReseter
{
    private readonly KeyCode _resetSceneButton;

    public SceneReseter(KeyCode resetSceneButton)
    {
        _resetSceneButton = resetSceneButton;
    }

    public event Action ResetSceneButtonPressed;

    public void Update()
    {
        if (Input.GetKeyDown(_resetSceneButton))
        {
            ResetSceneButtonPressed?.Invoke();
        }
    }
}