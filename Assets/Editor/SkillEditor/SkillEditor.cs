using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : Editor
{
    private int selectedElementIndex = 0;

    public override void OnInspectorGUI()
    {
        Skill skill = (Skill)target;
        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Skill Info", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillName"));

        EditorGUILayout.LabelField("Skill Cost", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillHPCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillMPCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillAPCost"));

        EditorGUILayout.LabelField("Skill Ranges", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillTarget"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillRangeType"));
        if (skill.skillRangeType != SkillRangeType.Weapon)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skillRanges"));
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("usedBySubWeapon"));
        }
            EditorGUILayout.LabelField("Skill Effects", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damageType"));
        if(skill.damageType != DamageType.Weapon)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("accuracy"));
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectBaseValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectModifierStat"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectScaleValue"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillEffectChance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectID"));
        EditorGUILayout.LabelField("Skill Texts", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("FlavourText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillIcon"));
        
        if(skill is not TargetingSingleSkill)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skillAreaEffectedType"));
        }


        if (skill is CustomAOESkill customAOESkill)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Ranges", EditorStyles.boldLabel);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("customRanges"), true);
            if (GUILayout.Button("Add Custom Range"))
            {
                serializedObject.Update();
                Array.Resize(ref customAOESkill.customRanges, customAOESkill.customRanges.Length + 1);
                customAOESkill.customRanges[customAOESkill.customRanges.Length - 1] = new CustomRange();
                selectedElementIndex = customAOESkill.customRanges.Length - 1;
                EditorUtility.SetDirty(customAOESkill);
                serializedObject.ApplyModifiedProperties();
                Repaint();
            }

            if (GUILayout.Button("Edit Custom Ranges"))
            {
                if (customAOESkill.customRanges.Length == 0)
                {
                    Array.Resize(ref customAOESkill.customRanges, customAOESkill.customRanges.Length + 1);
                    customAOESkill.customRanges[customAOESkill.customRanges.Length - 1] = new CustomRange();
                    EditorUtility.SetDirty(customAOESkill);

                    CustomRangeEditorWindow.Init(customAOESkill, customAOESkill.customRanges.Length - 1);
                }
                else
                {
                    CustomRangeEditorWindow.Init(customAOESkill, selectedElementIndex);
                }
            }

            if(GUILayout.Button("Delete Custom Range"))
            {
                serializedObject.Update();
                if (customAOESkill.customRanges.Length > 0)
                {
                    CustomRange[] newCustomRanges = new CustomRange[customAOESkill.customRanges.Length - 1];
                    for (int i = 0, j = 0; i < customAOESkill.customRanges.Length; i++)
                    {
                        if (i != selectedElementIndex)
                        {
                            newCustomRanges[j] = customAOESkill.customRanges[i];
                            j++;
                        }
                    }
                    customAOESkill.customRanges = newCustomRanges;
                    selectedElementIndex = Mathf.Clamp(selectedElementIndex, 0, customAOESkill.customRanges.Length - 1);
                    EditorUtility.SetDirty(customAOESkill);
                    serializedObject.ApplyModifiedProperties();
                    Repaint();
                }
                
            }

            if (customAOESkill.customRanges.Length > 0)
            {
                selectedElementIndex = EditorGUILayout.IntSlider("Select Custom Range Index", selectedElementIndex, 0, customAOESkill.customRanges.Length - 1);
            }
        }

        else if (skill is AOESkill)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Skill Area", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skillAreaRanges"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("skillAreaType"));
        }

        // SerializedObject ������Ʈ
        serializedObject.ApplyModifiedProperties();
    }
}
