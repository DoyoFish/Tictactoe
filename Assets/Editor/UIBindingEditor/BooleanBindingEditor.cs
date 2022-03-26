using System.Collections;
using System.Collections.Generic;
using UIBinding;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BooleanBinding))]
public class BooleanBindingEditor : Editor
{
    BooleanBinding _instance;

    private void OnEnable()
    {
        _instance = target as BooleanBinding;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        switch (_instance.ActiveConditionType)
        {
            case ActiveConditionType.EqualToReference:
            case ActiveConditionType.GreaterThanReference:
            case ActiveConditionType.LessThanReference:
                _instance.IntReference = EditorGUILayout.IntField("IntReference", _instance.IntReference);
                break;
            case ActiveConditionType.EqualToString:
                _instance.StringReference = EditorGUILayout.TextField("StringReference", _instance.StringReference);
                break;
            case ActiveConditionType.Between:
                _instance.MinReference = EditorGUILayout.IntField("MinReference", _instance.MinReference);
                _instance.MaxReference = EditorGUILayout.IntField("MaxReference", _instance.MaxReference);
                break;
        }
    }
}
