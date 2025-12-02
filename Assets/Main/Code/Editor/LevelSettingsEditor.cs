using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelSettings))]
public class LevelSettingsEditor : Editor
{
    private SerializedProperty _blockFieldSettings;
    //private SerializedProperty _truckFieldSettings;
    private SerializedProperty _cartrigeBoxSettings;

    private bool _showBlockSettings = true;
    //private bool _showTruckSettings = true;
    private bool _showCartridgeSettings = true;

    private bool[] _layerFoldouts;
    private bool[][] _rowFoldouts;
    private bool[][] _showSequences;

    private bool _showAllEditing = true;

    private void OnEnable()
    {
        _blockFieldSettings = serializedObject.FindProperty("_blockFieldSettings");
        //_truckFieldSettings = serializedObject.FindProperty("_truckFieldSettings");
        _cartrigeBoxSettings = serializedObject.FindProperty("_amountCartrigeBoxes");

        UpdateFoldouts();
    }

    private void UpdateFoldouts()
    {
        if (_blockFieldSettings != null)
        {
            SerializedProperty layers = _blockFieldSettings.FindPropertyRelative("_layers");
            
            // Инициализация массивов для слоёв
            if (_layerFoldouts == null || _layerFoldouts.Length != layers.arraySize)
            {
                _layerFoldouts = new bool[layers.arraySize];
                _rowFoldouts = new bool[layers.arraySize][];
                _showSequences = new bool[layers.arraySize][];
                
                for (int l = 0; l < layers.arraySize; l++)
                {
                    _layerFoldouts[l] = true;
                    SerializedProperty layer = layers.GetArrayElementAtIndex(l);
                    SerializedProperty rows = layer.FindPropertyRelative("_rows");
                    
                    _rowFoldouts[l] = new bool[rows.arraySize];
                    _showSequences[l] = new bool[rows.arraySize];
                    
                    for (int r = 0; r < rows.arraySize; r++)
                    {
                        _rowFoldouts[l][r] = true;
                        _showSequences[l][r] = true;
                    }
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        _showBlockSettings = EditorGUILayout.Foldout(_showBlockSettings, "Block Field Settings", true);

        if (_showBlockSettings)
        {
            EditorGUI.indentLevel++;
            DrawBlockFieldContent();
            EditorGUI.indentLevel--;
        }

        //_showTruckSettings = EditorGUILayout.Foldout(_showTruckSettings, "Truck Field Settings", true);

        //if (_showTruckSettings)
        //{
        //    EditorGUI.indentLevel++;
        //    DrawDefaultInspectorSection(_truckFieldSettings);
        //    EditorGUI.indentLevel--;
        //}

        _showCartridgeSettings = EditorGUILayout.Foldout(_showCartridgeSettings, "Cartridge Box Settings", true);

        if (_showCartridgeSettings)
        {
            EditorGUI.indentLevel++;
            DrawCartrigeBoxSettings();
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBlockFieldContent()
    {
        SerializedProperty fieldSize = _blockFieldSettings.FindPropertyRelative("_fieldSize");
        int columns = fieldSize.FindPropertyRelative("_amountColumns").intValue;

        EditorGUILayout.PropertyField(fieldSize.FindPropertyRelative("_amountLayers"), new GUIContent("Layers"));
        EditorGUILayout.PropertyField(fieldSize.FindPropertyRelative("_amountRows"), new GUIContent("Rows"));
        EditorGUILayout.PropertyField(fieldSize.FindPropertyRelative("_amountColumns"), new GUIContent("Columns"));

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(_showAllEditing ? "Hide All Editing" : "Show All Editing", GUILayout.Width(150)))
            {
                _showAllEditing = !_showAllEditing;
                for (int l = 0; l < _showSequences.Length; l++)
                {
                    for (int r = 0; r < _showSequences[l].Length; r++)
                    {
                        _showSequences[l][r] = _showAllEditing;
                    }
                }
            }
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        SerializedProperty layers = _blockFieldSettings.FindPropertyRelative("_layers");
        UpdateFoldouts();

        // Отрисовываем слои
        for (int l = 0; l < layers.arraySize; l++)
        {
            SerializedProperty layer = layers.GetArrayElementAtIndex(l);
            SerializedProperty rows = layer.FindPropertyRelative("_rows");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                // Заголовок слоя
                _layerFoldouts[l] = EditorGUILayout.Foldout(_layerFoldouts[l], $"Layer {l + 1}", true);
                
                if (_layerFoldouts[l])
                {
                    EditorGUI.indentLevel++;
                    
                    // Отрисовываем ряды для этого слоя (снизу вверх)
                    for (int r = rows.arraySize - 1; r >= 0; r--)
                    {
                        SerializedProperty row = rows.GetArrayElementAtIndex(r);
                        SerializedProperty sequences = row.FindPropertyRelative("_sequences");
                        int usedWidth = CalculateUsedWidth(sequences);
                        int remainingSpace = columns - usedWidth;

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                _rowFoldouts[l][r] = EditorGUILayout.Foldout(_rowFoldouts[l][r], $"Row {r + 1} (Used: {usedWidth}/{columns})", true);
                                
                                if (GUILayout.Button(_showSequences[l][r] ? "Hide Editing" : "Show Editing", GUILayout.Width(100)))
                                {
                                    _showSequences[l][r] = !_showSequences[l][r];
                                }

                                if (remainingSpace > 0 && GUILayout.Button("+ Add Sequence", GUILayout.Width(120)))
                                {
                                    sequences.InsertArrayElementAtIndex(sequences.arraySize);
                                    SerializedProperty newSequence = sequences.GetArrayElementAtIndex(sequences.arraySize - 1);
                                    newSequence.FindPropertyRelative("_amount").intValue = 1;
                                    newSequence.FindPropertyRelative("_colorType").enumValueIndex = 0;
                                }
                            }
                            EditorGUILayout.EndHorizontal();

                            if (_rowFoldouts[l][r])
                            {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    GUILayout.Space(5);
                                    int blockCounter = 0;

                                    for (int s = 0; s < sequences.arraySize; s++)
                                    {
                                        SerializedProperty sequence = sequences.GetArrayElementAtIndex(s);
                                        SerializedProperty colorType = sequence.FindPropertyRelative("_colorType");
                                        SerializedProperty amount = sequence.FindPropertyRelative("_amount");

                                        Color blockColor = GetColorForType((ColorType)colorType.enumValueIndex);
                                        int seqLength = amount.intValue;

                                        for (int b = 0; b < seqLength; b++)
                                        {
                                            DrawColorBlock(blockColor, blockCounter + b + 1);
                                            GUILayout.Space(2);
                                        }

                                        blockCounter += seqLength;
                                    }
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndHorizontal();

                                if (_showAllEditing && _showSequences[l][r])
                                {
                                    EditorGUILayout.Space();

                                    for (int s = 0; s < sequences.arraySize; s++)
                                    {
                                        EditorGUILayout.BeginVertical(EditorStyles.textArea);
                                        {
                                            SerializedProperty sequence = sequences.GetArrayElementAtIndex(s);
                                            SerializedProperty colorType = sequence.FindPropertyRelative("_colorType");
                                            SerializedProperty amount = sequence.FindPropertyRelative("_amount");

                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.PropertyField(colorType, GUIContent.none, GUILayout.Width(120));

                                                int currentAmount = amount.intValue;
                                                int maxAmount = currentAmount + remainingSpace;

                                                EditorGUI.BeginChangeCheck();
                                                int newAmount = EditorGUILayout.IntSlider(currentAmount, 1, maxAmount);

                                                if (EditorGUI.EndChangeCheck())
                                                {
                                                    int delta = newAmount - currentAmount;
                                                    amount.intValue = newAmount;
                                                    remainingSpace -= delta;
                                                }

                                                if (GUILayout.Button("×", GUILayout.Width(20)))
                                                {
                                                    sequences.DeleteArrayElementAtIndex(s);
                                                    break;
                                                }
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();
                                    }
                                }
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void DrawColorBlock(Color color, int positionNumber)
    {
        Rect rect = GUILayoutUtility.GetRect(25, 25, GUILayout.Width(25), GUILayout.Height(25));
        EditorGUI.DrawRect(rect, color);

        Handles.DrawAAPolyLine(2, new Vector3(rect.x, rect.y), new Vector3(rect.x + rect.width, rect.y));
        Handles.DrawAAPolyLine(2, new Vector3(rect.x + rect.width, rect.y), new Vector3(rect.x + rect.width, rect.y + rect.height));
        Handles.DrawAAPolyLine(2, new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x, rect.y + rect.height));
        Handles.DrawAAPolyLine(2, new Vector3(rect.x, rect.y + rect.height), new Vector3(rect.x, rect.y));

        GUIStyle style = new GUIStyle(EditorStyles.whiteMiniLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 9
        };

        EditorGUI.LabelField(rect, positionNumber.ToString(), style);
    }

    private Color GetColorForType(ColorType type)
    {
        switch (type)
        {
            case ColorType.Green: return new Color(0.2f, 0.8f, 0.2f);
            case ColorType.Blue: return new Color(0.2f, 0.5f, 0.9f);
            case ColorType.Yellow: return new Color(0.9f, 0.9f, 0.2f);
            case ColorType.Purple: return new Color(0.7f, 0.2f, 0.7f);
            case ColorType.Orange: return new Color(0.9f, 0.5f, 0.1f);
            default: return Color.gray;
        }
    }

    private int CalculateUsedWidth(SerializedProperty sequences)
    {
        int total = 0;

        for (int i = 0; i < sequences.arraySize; i++)
        {
            total += sequences.GetArrayElementAtIndex(i).FindPropertyRelative("_amount").intValue;
        }

        return total;
    }

    //private void DrawDefaultInspectorSection(SerializedProperty property)
    //{
    //    SerializedProperty iterator = property.Copy();
    //    SerializedProperty end = property.GetEndProperty();
    //    bool enterChildren = true;

    //    while (iterator.NextVisible(enterChildren) && SerializedProperty.EqualContents(iterator, end) == false)
    //    {
    //        EditorGUILayout.PropertyField(iterator, true);
    //        enterChildren = false;
    //    }
    //}

    private void DrawCartrigeBoxSettings()
    {
        EditorGUILayout.PropertyField(_cartrigeBoxSettings, new GUIContent("Amount"));
    }
}
#endif