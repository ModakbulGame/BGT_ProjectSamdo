using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class TimerScript : MonoBehaviour
{
    private TextMeshProUGUI[] m_timerTxt;
    private TextMeshProUGUI m_gameOverTxt;  // ���� �� �Ϸ�, ���� �� ���� �ؽ�Ʈ

    private float time = 60;   // �ӽ÷� 1��
    private int min, sec;

    void Start()
    {
        m_timerTxt[0].text = "01";
        m_timerTxt[1].text = "00";
    }

    void Update()
    {
        time -= Time.deltaTime;

        min = (int)time / 60;
        sec = ((int)time - min * 60) % 60;

        if (min <= 0 && sec <= 0)
        {
            m_timerTxt[0].text = 00.ToString();
            m_timerTxt[1].text = 00.ToString();
        }

        else
        {
            if (sec >= 60)
            {
                min += 1;
                sec -= 60;
            }
            else
            {
                m_timerTxt[0].text = min.ToString();
                m_timerTxt[1].text = sec.ToString();
            }
        }
    }
}
