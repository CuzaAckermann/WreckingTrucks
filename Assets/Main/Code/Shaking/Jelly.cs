using System;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    [SerializeField] private float _intensity = 1f;
    [SerializeField] private float _mass = 1f;
    [SerializeField] private float _stiffness = 1f;
    [SerializeField] private float _damping = 0.75f;

    private Transform _transform;
    private Mesh _originalMesh;
    private Mesh _meshClone;
    private MeshRenderer _meshRenderer;
    private JellyVertex[] _jellyVertex;
    private Vector3[] _vertexArray;

    public event Action<Jelly> HesitationFinished;

    public void Initialize()
    {
        _transform = transform;
        _originalMesh = GetComponent<MeshFilter>().sharedMesh;
        _meshClone = Instantiate(_originalMesh);
        GetComponent<MeshFilter>().sharedMesh = _meshClone;
        _meshRenderer = GetComponent<MeshRenderer>();
        _jellyVertex = new JellyVertex[_meshClone.vertices.Length];

        for (int i = 0; i < _meshClone.vertices.Length; i++)
        {
            _jellyVertex[i] = new JellyVertex(i, _transform.TransformPoint(_meshClone.vertices[i]));
        }
    }

    public void Shake()
    {
        bool isShaked = false;
        _vertexArray = _originalMesh.vertices;

        for (int i = 0; i < _jellyVertex.Length; i++)
        {
            Vector3 target = _transform.TransformPoint(_vertexArray[_jellyVertex[i].ID]);
            float intensity = (1 - (_meshRenderer.bounds.max.y - target.y) / _meshRenderer.bounds.size.y) * _intensity;
            
            if (_jellyVertex[i].TryShake(target, _mass, _stiffness, _damping))
            {
                isShaked = true;
            }

            target = _transform.InverseTransformPoint(_jellyVertex[i].Position);
            _vertexArray[_jellyVertex[i].ID] = Vector3.Lerp(_vertexArray[_jellyVertex[i].ID], target, intensity);
        }

        _meshClone.vertices = _vertexArray;

        if (isShaked == false)
        {
            HesitationFinished?.Invoke(this);
        }
    }

    public void Settle()
    {
        _vertexArray = _originalMesh.vertices;

        for (int i = 0; i < _jellyVertex.Length; i++)
        {
            Vector3 target = transform.TransformPoint(_vertexArray[_jellyVertex[i].ID]);
            _jellyVertex[i].Settle(target);
        }
    }
}