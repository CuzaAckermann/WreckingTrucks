using UnityEngine;
using UnityEditor;

// ��������� �������� ��� ���������� AdvancedBezierCurve
[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{
    private BezierCurve _curve; // ������ �� ������������� ������
    private int _selectedNodeIndex = -1; // ������ ���������� ���� (-1 - ������ �� �������)
    private int _selectedTangentIndex = -1; // ������ ���������� ������������ ������
    private bool _isTangentIn; // ����, ����������� �� ��, ����� ����� ������ (��������/���������)

    // ���������� ��� ��������� ���������
    private void OnEnable()
    {
        // �������� ������ �� ������� ���������
        _curve = (BezierCurve)target;
    }

    // ����� ��� ��������� � �����
    private void OnSceneGUI()
    {
        // ���� ��� ����� - �������
        if (_curve.Nodes == null || _curve.Nodes.Count == 0)
        {
            return;
        }

        // ��������� ����������� �������� ��� ��������� �������
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        // �������� ������� �������
        Event currentEvent = Event.current;

        // �������� �� ���� ����� ������
        for (int i = 0; i < _curve.Nodes.Count; i++)
        {
            BezierNode node = _curve.Nodes[i];

            // ���������� ���� ����� �� ������
            if (node.Point == null)
            {
                continue;
            }

            // ������������� ���� ��� �����
            Handles.color = _curve.NodesColor;
            // ��������� ������ ������� ���� (������� �� ���������� �� ������)
            float handleSize = HandleUtility.GetHandleSize(node.Point.position) * 0.2f;

            // ������� ������-������ ��� ����
            if (Handles.Button(node.Point.position, Quaternion.identity, handleSize, handleSize, Handles.SphereHandleCap))
            {
                // ��� ����� ���������� ��������� ����
                _selectedNodeIndex = i;
                _selectedTangentIndex = -1;
                Repaint(); // ��������� ���������
            }

            // ���� ���� ������ - ������������ ��� �����������
            if (_selectedNodeIndex == i)
            {
                EditorGUI.BeginChangeCheck();
                // ������� ����� ��� �����������
                Vector3 newPos = Handles.PositionHandle(node.Point.position, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    // ���������� ��������� ��� Undo
                    Undo.RecordObject(_curve, "Move Node");
                    // ��������� �������
                    node.Point.position = newPos;
                    // �������������� �����������
                    _curve.SyncAllTangents();
                }
            }

            // ��������� ���������� ������ (tangentOut)
            if (node.TangentOut != null)
            {
                Handles.color = _curve.TangentsColor;
                float tangentSize = HandleUtility.GetHandleSize(node.TangentOut.position) * 0.15f;

                // ������-������ ��� ���������� ������
                if (Handles.Button(node.TangentOut.position, Quaternion.identity, tangentSize, tangentSize, Handles.SphereHandleCap))
                {
                    _selectedNodeIndex = i;
                    _selectedTangentIndex = i;
                    _isTangentIn = false;
                    Repaint();
                }

                // ����������� ���������� ������
                if (_selectedTangentIndex == i && _isTangentIn == false)
                {
                    EditorGUI.BeginChangeCheck();
                    Vector3 newPos = Handles.PositionHandle(node.TangentOut.position, Quaternion.identity);
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_curve, "Move Tangent");
                        node.TangentOut.position = newPos;
                        node.SyncTangents();
                        _curve.CalculateCurve();
                    }
                }

                // ������ ����� �� ���� � ������
                Handles.DrawLine(node.Point.position, node.TangentOut.position);
            }

            // ��������� ��������� ������ (tangentIn) - ���������� ����������
            if (node.TangentIn != null)
            {
                Handles.color = _curve.TangentsColor;
                float tangentSize = HandleUtility.GetHandleSize(node.TangentIn.position) * 0.15f;

                if (Handles.Button(node.TangentIn.position, Quaternion.identity, tangentSize, tangentSize, Handles.SphereHandleCap))
                {
                    _selectedNodeIndex = i;
                    _selectedTangentIndex = i;
                    _isTangentIn = true;
                    Repaint();
                }

                if (_selectedTangentIndex == i && _isTangentIn)
                {
                    EditorGUI.BeginChangeCheck();
                    Vector3 newPos = Handles.PositionHandle(node.TangentIn.position, Quaternion.identity);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_curve, "Move Tangent");
                        node.TangentIn.position = newPos;
                        node.SyncTangents();
                        _curve.CalculateCurve();
                    }
                }

                Handles.DrawLine(node.Point.position, node.TangentIn.position);
            }
        }

        // ��������� �������� ���� �� ������� Delete
        if (currentEvent.type == EventType.KeyDown &&
            currentEvent.keyCode == KeyCode.Delete &&
            _selectedNodeIndex != -1)
        {
            Undo.RecordObject(_curve, "Delete Node");
            _curve.RemoveNode(_selectedNodeIndex);
            _selectedNodeIndex = -1;
            _selectedTangentIndex = -1;
            currentEvent.Use(); // �������� ������� ��� ������������
        }

        // ��������� ���������� ���� �� Ctrl+A
        if (currentEvent.type == EventType.KeyDown &&
            currentEvent.keyCode == KeyCode.A &&
            currentEvent.control)
        {
            // ������� ������ ���� - ������ �� ���������� ��� � ������ �������
            Vector3 newPos = _curve.Nodes.Count > 0 ?
                             _curve.Nodes[_curve.Nodes.Count - 1].Point.position + Vector3.right * 2f :
                             _curve.transform.position;

            // ���� ���� ����������� � ����� - ���������� ����� �����������
            Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                newPos = hit.point;
            }

            Undo.RecordObject(_curve, "Add Node");
            _curve.AddNodeAtPosition(newPos);
            _selectedNodeIndex = _curve.Nodes.Count - 1;
            _selectedTangentIndex = -1;
            currentEvent.Use();
        }
    }

    // ��������� ���������� � ����������
    public override void OnInspectorGUI()
    {
        // ��������� ������������ ����������
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Editor Controls", EditorStyles.boldLabel);
        // ��������� �� ����������
        EditorGUILayout.HelpBox("Click on nodes or tangents to select them\n" +
                               "Drag to move selected items\n" +
                               "Ctrl+A to add new node at mouse position\n" +
                               "Delete to remove selected node", MessageType.Info);

        // ���� ���� ��������� ���� - ���������� ������ ��������
        if (_selectedNodeIndex != -1)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Selected Node: {_selectedNodeIndex}", EditorStyles.boldLabel);

            if (GUILayout.Button("Delete Selected Node"))
            {
                Undo.RecordObject(_curve, "Delete Node");
                _curve.RemoveNode(_selectedNodeIndex);
                _selectedNodeIndex = -1;
                _selectedTangentIndex = -1;
            }
        }
    }
}