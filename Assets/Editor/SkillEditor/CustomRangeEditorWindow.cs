using System;
using UnityEditor;
using UnityEngine;

public class CustomRangeEditorWindow : EditorWindow
{
    private Skill skill;
    private Vector2Int selectedTile;
    private Vector2Int gridSize = new Vector2Int(5, 5); // 예시: 5x5 격자
    
    private float tileSize = 50f; // 타일 크기

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

        // 격자 그리기
        DrawTileGrid();

        // 완료 버튼
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
        // 격자 배경 그리기
        EditorGUI.DrawRect(gridRect, Color.gray);

        // 타일 그리기
        for (int x = -gridSize.x; x <= gridSize.x; x++)
        {
            for (int y = -gridSize.y; y <= gridSize.y; y++)
            {
                Rect tileRect = new Rect(gridOffset.x + x * tileSize, gridOffset.y + y * tileSize, tileSize, tileSize);
                if (x == 0 && y == 0)
                {
                    if (customRange.tilePositions.Contains(new Vector2Int(x, y)))
                    {
                        // 중심 타일이 선택된 경우 보라색으로 표시
                        EditorGUI.DrawRect(tileRect, Color.magenta);
                    }
                    else
                    {
                        // 중심 타일을 푸른색으로 표시
                        EditorGUI.DrawRect(tileRect, Color.blue);
                    }
                }
                else if (customRange.tilePositions.Contains(new Vector2Int(x, y)))
                {
                    // 선택된 타일을 붉은 색으로 표시
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

                // 타일 클릭 시 선택 처리
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
