using System.Collections.Generic;
using UnityEngine;

public class PathPresenter : MonoBehaviour
{
    [SerializeField] private List<Transform> _transforms;

    private List<Vector3> _positions;

    public void Initialize()
    {
        _positions = new List<Vector3>();

        ConvertToListVector3();
    }

    public IReadOnlyList<Vector3> Positions => _positions;

    private void ConvertToListVector3()
    {
        for (int i = 0; i < _transforms.Count; i++)
        {
            _positions.Add(_transforms[i].position);
        }
    }
}