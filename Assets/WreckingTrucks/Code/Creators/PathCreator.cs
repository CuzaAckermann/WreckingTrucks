using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [SerializeField] private List<Transform> _transform;

    public Path CreatePath()
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < _transform.Count; i++)
        {
            positions.Add(_transform[i].position);
        }

        return new Path(positions);
    }
}