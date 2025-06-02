using System.Collections.Generic;
using UnityEngine;

public class PathPresenter : MonoBehaviour
{
    [SerializeField] private List<Transform> _transform;

    public List<Vector3> Positions { get; private set; }

    public void Initialize()
    {
        Positions = ConvertToListVector3();
    }

    public List<Vector3> ConvertToListVector3()
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < _transform.Count; i++)
        {
            positions.Add(_transform[i].position);
        }

        return positions;
    }
}