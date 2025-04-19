using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Effect), true)]
public class EffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Effect effect = (Effect)target;
        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Effect Info", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectType"));
        switch(effect.effectType)
        {
            case EffectType.Buff:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("buffType"));
                switch (effect.buffType)
                {
                    case BuffType.Stat:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("statType"));
                        break;
                    case BuffType.Actionpoint:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionpointEffectType"));
                        break;
                    case BuffType.SkillCost:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillCostEffectType"));
                        break;
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("value"));
                break;
            case EffectType.Debuff:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("debuffType"));
                switch (effect.debuffType)
                {
                    case DebuffType.Stat:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("statType"));
                        break;
                    case DebuffType.Actionpoint:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionpointEffectType"));
                        break;
                    case DebuffType.SkillCost:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillCostEffectType"));
                        break;
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("value"));
                break;
            case EffectType.CrowdControl:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("crowdControlType"));
                break;
            case EffectType.DamageOverTime:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dotType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("value"));
                break;
            case EffectType.HealOverTime:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("value"));
                break;
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
        serializedObject.ApplyModifiedProperties();
    }
}
