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
            textTimer.text = "0:0" + $"{currentTime:F0}"; // 소수점 0자리까지 표시
        }
        else
        textTimer.text = "0:" + $"{currentTime:F0}"; // 소수점 0자리까지 표시

    }

    public void TimerFinished()
    {
        // 타이머가 끝났을 때 실행할 코드
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
