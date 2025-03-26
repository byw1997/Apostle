using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Skill))]
public class SkillEditor : Editor
{
    private int selectedElementIndex = 0;

    public override void OnInspectorGUI()
    {
        Skill skill = (Skill)target;
        // �⺻���� ��ų ���� ǥ��
        // �׻� ǥ�õǴ� ��ҵ� (skillAreaType�� �������)
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Skill Info", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillMPCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillAPCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillTarget"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillTargetType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damageType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectBaseValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectScaleValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectChance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("FlavourText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillIcon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillAreaType"));
        
        // skillAreaType�� Custom�� ��� customRanges�� ǥ��
        if (skill.skillAreaType == SkillAreaType.Custom)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Ranges", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("customRanges"), true);
            if (GUILayout.Button("Edit Custom Ranges"))
            {
                if (skill.customRanges.Length == 0)
                {
                    // customRanges �迭�� ���ο� ���� �߰�
                    Array.Resize(ref skill.customRanges, skill.customRanges.Length + 1);
                    skill.customRanges[skill.customRanges.Length - 1] = new CustomRange();
                    EditorUtility.SetDirty(skill);

                    CustomRangeEditorWindow.Init(skill, skill.customRanges.Length - 1);
                }
                else
                {
                    CustomRangeEditorWindow.Init(skill, selectedElementIndex);
                }
            }

            if (skill.customRanges.Length > 0)
            {
                selectedElementIndex = EditorGUILayout.IntSlider("Select Custom Range Index", selectedElementIndex, 0, skill.customRanges.Length - 1);
            }

            if (GUILayout.Button("Add Custom Range"))
            {
                serializedObject.Update();
                Array.Resize(ref skill.customRanges, skill.customRanges.Length + 1);
                skill.customRanges[skill.customRanges.Length - 1] = new CustomRange();
                selectedElementIndex = skill.customRanges.Length - 1;
                EditorUtility.SetDirty(skill);
                serializedObject.ApplyModifiedProperties();
                Repaint();
            }
        }
        else
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Skill Ranges", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skillRanges"), true);
        }

        // SerializedObject ������Ʈ
        serializedObject.ApplyModifiedProperties();
    }
}
