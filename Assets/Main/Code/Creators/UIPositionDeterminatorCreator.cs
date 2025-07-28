using System;
using UnityEngine;

public class UIPositionDeterminatorCreator
{
    private readonly Camera _camera;

    public UIPositionDeterminatorCreator(Camera camera)
    {
        _camera = camera ? camera : throw new ArgumentNullException(nameof(camera));
    }

    public UIPositionDeterminator Create()
    {
        return new UIPositionDeterminator(_camera);
    }
}