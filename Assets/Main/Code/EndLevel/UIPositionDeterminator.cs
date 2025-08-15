using UnityEngine;

public class UIPositionDeterminator : MonoBehaviour, ITargetPositionDeterminator
{
    [SerializeField] private Camera _camera;

    // ������ ���������� ����� � ������������ ����� ��� ��������� ����������� ����� ������
    public Vector3 GetTargetPosition()
    {
        return _camera.ScreenToWorldPoint(new Vector3(0, 1080, 10));
    }
}