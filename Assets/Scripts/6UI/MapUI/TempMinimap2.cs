using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempMinimap2 : MonoBehaviour
{
    [SerializeField]
    private Transform m_left;
    [SerializeField]
    private Transform m_right;
    [SerializeField]
    private Transform m_top;
    [SerializeField]
    private Transform m_bottom;

    [SerializeField]
    private Image m_minimapImage;
    [SerializeField]
    private Image m_minimapPlayerImage;
    [SerializeField]
    private Transform m_targetPlayer;

    // Update is called once per frame
    void Update()
    {
        if(m_targetPlayer != null)
        {
            Vector2 mapArea = new Vector2(Vector3.Distance(m_left.position, m_right.position), Vector3.Distance(m_bottom.position, m_top.position));
            Vector2 charPos = new Vector2(Vector3.Distance(m_left.position, new Vector3(m_targetPlayer.transform.position.x, 0f, 0f)),
                Vector3.Distance(m_bottom.position, new Vector3(0f,0f, m_targetPlayer.transform.position.z)));
            Vector2 normalPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);

            m_minimapPlayerImage.rectTransform.anchoredPosition = new Vector2(m_minimapImage.rectTransform.sizeDelta.x * normalPos.x, m_minimapImage.rectTransform.sizeDelta.y * normalPos.y);
        }
    }
}
