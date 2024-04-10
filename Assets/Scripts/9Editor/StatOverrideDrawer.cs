using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatManagerOverride))]
public class StatOverrideDrawer : PropertyDrawer
{   
    // 변수가 그려지는 모양을 마음껏 바꾸기 위한 Attribute
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // FindPropertyRelative로 StatOverride 객체가 가진 Stat 변수 정보를 찾아온다
        var statProperty = property.FindPropertyRelative("stat");

        // 앞서 찾아온 Stat 정보에서 objectReferenceValue, 즉 실제 변수값이며 변수가
        // null이 아니라면 Stat_을 빼고 이름을 가져온다
        // null이라면 인자로 받은 기본 label.text를 가져온다
        var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        string labelName = statProperty.objectReferenceValue?.name.Replace("STAT_", "") ?? label.text;

        property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, labelName);
        // Serialized의 fold out 여부를 저장하기 위한 변수
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

            // isUseoverride가 false이면 GUI가 그려지지 않는다
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
            // 확장되어 있지 않다면 Foldout만 그려질테니 높이는 한줄이다
            return EditorGUIUtility.singleLineHeight;
        else
        {
            // 확장되어 있다면 Override 사용 여부 가져오고
            // 확장시 그려야 할 인자는 총 3,4개(Foldout, Stat, isUseOverride(bool 체크), overrideDefaultValue) 
            // propertyline 만큼 여유 확보 + 하단 공간 여유 확보
            bool isUseOverride = property.FindPropertyRelative("isUseOverride").boolValue;
            int propertyLine = isUseOverride ? 4 : 3;
            return (EditorGUIUtility.singleLineHeight * propertyLine) + propertyLine;
        }
    }
}

