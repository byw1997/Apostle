using System;
using UnityEditor;
using UnityEngine;

public class CustomRangeEditorWindow : EditorWindow
{
    private Skill skill;
    private Vector2Int selectedTile;
    private Vector2Int gridSize = new Vector2Int(5, 5); // ����: 5x5 ����
    
    private float tileSize = 50f; // Ÿ�� ũ��

    private CustomRange customRange;
    private int selectedElementIndex;

    public static void Init(Skill skill, int elementIndex)
    {
        CustomRangeEditorWindow window = GetWindow<CustomRangeEditorWindow>("Custom Range Editor");
        window.skill = skill;
        window.selectedElementIndex = elementIndex;
        if (elementIndex >= 0 && elementIndex < skill.customRanges.Length)
        {
            window.customRange = skill.customRanges[elementIndex];
        }
        else
        {
            window.customRange = new CustomRange();
            Array.Resize(ref skill.customRanges, skill.customRanges.Length + 1);
            skill.customRanges[skill.customRanges.Length - 1] = window.customRange;
            window.selectedElementIndex = skill.customRanges.Length - 1;
        }
        window.Show();
    }

    private void OnGUI()
    {
        if (skill == null)
            return;

        GUILayout.Label("Tile Grid", EditorStyles.boldLabel);

        // ���� �׸���
        DrawTileGrid();

        // �Ϸ� ��ư
        if (GUILayout.Button("Save Custom Range"))
        {
            if (skill.customRanges.Length == 0)
            {
                Array.Resize(ref skill.customRanges, 1);
            }
            skill.customRanges[selectedElementIndex] = customRange;
            EditorUtility.SetDirty(skill);
            Close();
        }
    }

    private void DrawTileGrid()
    {
        Rect gridRect = new Rect(10, 40, (2 * gridSize.x + 1) * tileSize, (2 * gridSize.y + 1) * tileSize);
        Vector2 gridOffset = new Vector2(gridSize.x * tileSize + 10, gridSize.x * tileSize + 40);
        // ���� ��� �׸���
        EditorGUI.DrawRect(gridRect, Color.gray);

        // Ÿ�� �׸���
        for (int x = -gridSize.x; x <= gridSize.x; x++)
        {
            for (int y = -gridSize.y; y <= gridSize.y; y++)
            {
                Rect tileRect = new Rect(gridOffset.x + x * tileSize, gridOffset.y + y * tileSize, tileSize, tileSize);
                if (x == 0 && y == 0)
                {
                    if (customRange.tilePositions.Contains(new Vector2Int(x, y)))
                    {
                        // �߽� Ÿ���� ���õ� ��� ��������� ǥ��
                        EditorGUI.DrawRect(tileRect, Color.magenta);
                    }
                    else
                    {
                        // �߽� Ÿ���� Ǫ�������� ǥ��
                        EditorGUI.DrawRect(tileRect, Color.blue);
                    }
                }
                else if (customRange.tilePositions.Contains(new Vector2Int(x, y)))
                {
                    // ���õ� Ÿ���� ���� ������ ǥ��
                    EditorGUI.DrawRect(tileRect, Color.red);
                }
                else
                {
                    GUI.Box(tileRect, "");
                }

                Handles.color = Color.black;
                Handles.DrawPolyLine(
                    new Vector3[] {
                        tileRect.min,
                        new Vector3(tileRect.xMax, tileRect.yMin),
                        tileRect.max,
                        new Vector3(tileRect.xMin, tileRect.yMax),
                        tileRect.min
                    });

                // Ÿ�� Ŭ�� �� ���� ó��
                if (tileRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    selectedTile = new Vector2Int(x , y );
                    if (customRange.tilePositions.Contains(selectedTile))
                    {
                        customRange.tilePositions.Remove(selectedTile);
                    }
                    else
                    {
                        customRange.tilePositions.Add(selectedTile);
                    }
                    EditorUtility.SetDirty(skill);

                    Event.current.Use();
                    
                }
            }
        }
    }
}
