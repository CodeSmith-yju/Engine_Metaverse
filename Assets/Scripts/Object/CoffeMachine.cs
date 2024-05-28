using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoffeMachine : MonoBehaviour
{
    public List<GameObject> machines = new();

    public GameObject timer_Screen;
    public TextMeshProUGUI textTimer;
    public Image time_Img;

    public float currentTime = 0f;
    public bool isTimerRunning = false;
    public float maxTime = 0f;

    private void Awake()
    {
        timer_Screen.SetActive(false);
        machines[0].SetActive(false);
    }
    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                TimerFinished();
            }
            UpdateTimerDisplay();
        }
    }

    public void UpdateTimerDisplay()
    {
        /*if (currentTime < 10)
        {
            textTimer.text = "0:0" + $"{currentTime:F0}"; // �Ҽ��� 0�ڸ����� ǥ��
        }
        else
        textTimer.text = "0:" + $"{currentTime:F0}"; // �Ҽ��� 0�ڸ����� ǥ��*/
        textTimer.text = $"{currentTime:F0}"; // �Ҽ��� 0�ڸ����� ǥ��
        time_Img.fillAmount = currentTime / maxTime;
    }

    public void TimerFinished()
    {
        // Ÿ�̸Ӱ� ������ �� ������ �ڵ�
        timer_Screen.SetActive(false);
        machines[0].SetActive(true);
        Debug.Log("Timer finished!");
    }
    public void StartTimer(float _time)
    {
        currentTime = _time;
        maxTime = _time;
        isTimerRunning = true;
        timer_Screen.SetActive(true);
        UpdateTimerDisplay();
    }

}