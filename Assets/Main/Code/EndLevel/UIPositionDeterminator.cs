using UnityEngine;

public class UIPositionDeterminator : MonoBehaviour, ITargetPositionDeterminator
{
    [SerializeField] private Camera _camera;

    // сделай нормальный метод с определением точки где находится отображение денег игрока
    public Vector3 GetTargetPosition()
    {
        return _camera.ScreenToWorldPoint(new Vector3(0, 1080, 10));
    }
}