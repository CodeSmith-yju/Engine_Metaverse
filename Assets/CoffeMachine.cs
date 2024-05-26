using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoffeMachine : MonoBehaviour
{
    public List<GameObject> machines = new();

    public GameObject timer_Screen;
    public TextMeshProUGUI textTimer;

    public float currentTime = 0f;
    public bool isTimerRunning = false;

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
        if (currentTime < 10)
        {
            textTimer.text = "0:0" + $"{currentTime:F0}"; // �Ҽ��� 0�ڸ����� ǥ��
        }
        else
        textTimer.text = "0:" + $"{currentTime:F0}"; // �Ҽ��� 0�ڸ����� ǥ��

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
        isTimerRunning = true;
        timer_Screen.SetActive(true);
        UpdateTimerDisplay();
    }

}
