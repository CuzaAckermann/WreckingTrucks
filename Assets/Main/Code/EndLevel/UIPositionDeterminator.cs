using System;
using UnityEngine;

public class UIPositionDeterminator : ITargetPositionDeterminator
{
    private readonly Camera _camera;

    public UIPositionDeterminator(Camera camera)
    {
        _camera = camera ? camera : throw new ArgumentNullException(nameof(camera));
    }

    // ������ ���������� ����� � ������������ ����� ��� ��������� ����������� ����� ������
    public Vector3 GetTargetPosition()
    {
        return _camera.ScreenToWorldPoint(new Vector3(0, 1080, 10));
    }
}