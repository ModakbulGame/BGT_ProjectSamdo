using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatManagerOverride))]
public class StatOverrideDrawer : PropertyDrawer
{   
    // ������ �׷����� ����� ������ �ٲٱ� ���� Attribute
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // FindPropertyRelative�� StatOverride ��ü�� ���� Stat ���� ������ ã�ƿ´�
        var statProperty = property.FindPropertyRelative("stat");

        // �ռ� ã�ƿ� Stat �������� objectReferenceValue, �� ���� �������̸� ������
        // null�� �ƴ϶�� Stat_�� ���� �̸��� �����´�
        // null�̶�� ���ڷ� ���� �⺻ label.text�� �����´�
        var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        string labelName = statProperty.objectReferenceValue?.name.Replace("STAT_", "") ?? label.text;

        property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, labelName);
        // Serialized�� fold out ���θ� �����ϱ� ���� ����
        if (property.isExpanded)
        {
            var boxRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width,
                GetPropertyHeight(property, label) - EditorGUIUtility.singleLineHeight);
            EditorGUI.HelpBox(boxRect, "", MessageType.None);

            var propertyRect = new Rect(boxRect.x + 4f, boxRect.y + 2f, boxRect.width - 8f, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property.FindPropertyRelative("stat"));

            propertyRect.y += EditorGUIUtility.singleLineHeight;
            var isUseOverrideProperty = property.FindPropertyRelative("isUseOverride");
            EditorGUI.PropertyField(propertyRect, isUseOverrideProperty);

            // isUseoverride�� false�̸� GUI�� �׷����� �ʴ´�
            if(isUseOverrideProperty.boolValue)
            {
                propertyRect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(propertyRect, property.FindPropertyRelative("overrideDefaultValue"));
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            // Ȯ��Ǿ� ���� �ʴٸ� Foldout�� �׷����״� ���̴� �����̴�
            return EditorGUIUtility.singleLineHeight;
        else
        {
            // Ȯ��Ǿ� �ִٸ� Override ��� ���� ��������
            // Ȯ��� �׷��� �� ���ڴ� �� 3,4��(Foldout, Stat, isUseOverride(bool üũ), overrideDefaultValue) 
            // propertyline ��ŭ ���� Ȯ�� + �ϴ� ���� ���� Ȯ��
            bool isUseOverride = property.FindPropertyRelative("isUseOverride").boolValue;
            int propertyLine = isUseOverride ? 4 : 3;
            return (EditorGUIUtility.singleLineHeight * propertyLine) + propertyLine;
        }
    }
}

