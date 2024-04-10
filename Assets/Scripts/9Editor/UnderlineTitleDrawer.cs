using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnderlineTitleAttribute))]
public class UnderlineTitleDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        var attributeAsUnderlineTitle = attribute as UnderlineTitleAttribute;

        position=EditorGUI.IndentedRect(position);
        position.height= EditorGUIUtility.singleLineHeight;
        position.y += attributeAsUnderlineTitle.Space;

        // position에 title을 Bold Style로 그린다
        GUI.Label(position,attributeAsUnderlineTitle.Title,EditorStyles.boldLabel);

        // 한줄 이동
        position.y += EditorGUIUtility.singleLineHeight;
        // 두께 1
        position.height = 1f;
        // 회색 선 그림
        EditorGUI.DrawRect(position, Color.gray);
    }

    public override float GetHeight()
    {
        var attributeAsUnderlineTitle = attribute as UnderlineTitleAttribute;
        // 기본 GUI 높이 + 설정된 Attribute Space + 기본 GUI 간격 * 2
        return attributeAsUnderlineTitle.Space + EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2);
    }
}
