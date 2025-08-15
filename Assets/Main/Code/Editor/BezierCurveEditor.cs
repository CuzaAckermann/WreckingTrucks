using UnityEngine;
using UnityEditor;

// Кастомный редактор для компонента AdvancedBezierCurve
[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{
    private BezierCurve _curve; // Ссылка на редактируемую кривую
    private int _selectedNodeIndex = -1; // Индекс выбранного узла (-1 - ничего не выбрано)
    private int _selectedTangentIndex = -1; // Индекс выбранного касательного рычага
    private bool _isTangentIn; // Флаг, указывающий на то, какой рычаг выбран (входящий/исходящий)

    // Вызывается при активации редактора
    private void OnEnable()
    {
        // Получаем ссылку на целевой компонент
        _curve = (BezierCurve)target;
    }

    // Метод для отрисовки в сцене
    private void OnSceneGUI()
    {
        // Если нет узлов - выходим
        if (_curve.Nodes == null || _curve.Nodes.Count == 0)
        {
            return;
        }

        // Добавляем стандартный контроль для обработки событий
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        // Получаем текущее событие
        Event currentEvent = Event.current;

        // Проходим по всем узлам кривой
        for (int i = 0; i < _curve.Nodes.Count; i++)
        {
            BezierNode node = _curve.Nodes[i];

            // Пропускаем если точка не задана
            if (node.Point == null)
            {
                continue;
            }

            // Устанавливаем цвет для узлов
            Handles.color = _curve.NodesColor;
            // Вычисляем размер маркера узла (зависит от расстояния до камеры)
            float handleSize = HandleUtility.GetHandleSize(node.Point.position) * 0.2f;

            // Создаем кнопку-маркер для узла
            if (Handles.Button(node.Point.position, Quaternion.identity, handleSize, handleSize, Handles.SphereHandleCap))
            {
                // При клике запоминаем выбранный узел
                _selectedNodeIndex = i;
                _selectedTangentIndex = -1;
                Repaint(); // Обновляем инспектор
            }

            // Если узел выбран - обрабатываем его перемещение
            if (_selectedNodeIndex == i)
            {
                EditorGUI.BeginChangeCheck();
                // Создаем хендл для перемещения
                Vector3 newPos = Handles.PositionHandle(node.Point.position, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    // Записываем изменение для Undo
                    Undo.RecordObject(_curve, "Move Node");
                    // Обновляем позицию
                    node.Point.position = newPos;
                    // Синхронизируем касательные
                    _curve.SyncAllTangents();
                }
            }

            // Обработка исходящего рычага (tangentOut)
            if (node.TangentOut != null)
            {
                Handles.color = _curve.TangentsColor;
                float tangentSize = HandleUtility.GetHandleSize(node.TangentOut.position) * 0.15f;

                // Кнопка-маркер для исходящего рычага
                if (Handles.Button(node.TangentOut.position, Quaternion.identity, tangentSize, tangentSize, Handles.SphereHandleCap))
                {
                    _selectedNodeIndex = i;
                    _selectedTangentIndex = i;
                    _isTangentIn = false;
                    Repaint();
                }

                // Перемещение исходящего рычага
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

                // Рисуем линию от узла к рычагу
                Handles.DrawLine(node.Point.position, node.TangentOut.position);
            }

            // Обработка входящего рычага (tangentIn) - аналогично исходящему
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

        // Обработка удаления узла по клавише Delete
        if (currentEvent.type == EventType.KeyDown &&
            currentEvent.keyCode == KeyCode.Delete &&
            _selectedNodeIndex != -1)
        {
            Undo.RecordObject(_curve, "Delete Node");
            _curve.RemoveNode(_selectedNodeIndex);
            _selectedNodeIndex = -1;
            _selectedTangentIndex = -1;
            currentEvent.Use(); // Помечаем событие как обработанное
        }

        // Обработка добавления узла по Ctrl+A
        if (currentEvent.type == EventType.KeyDown &&
            currentEvent.keyCode == KeyCode.A &&
            currentEvent.control)
        {
            // Позиция нового узла - справа от последнего или у начала объекта
            Vector3 newPos = _curve.Nodes.Count > 0 ?
                             _curve.Nodes[_curve.Nodes.Count - 1].Point.position + Vector3.right * 2f :
                             _curve.transform.position;

            // Если есть пересечение с мышью - используем точку пересечения
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

    // Отрисовка интерфейса в инспекторе
    public override void OnInspectorGUI()
    {
        // Отрисовка стандартного инспектора
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Editor Controls", EditorStyles.boldLabel);
        // Подсказки по управлению
        EditorGUILayout.HelpBox("Click on nodes or tangents to select them\n" +
                               "Drag to move selected items\n" +
                               "Ctrl+A to add new node at mouse position\n" +
                               "Delete to remove selected node", MessageType.Info);

        // Если есть выбранный узел - показываем кнопку удаления
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