using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mixer : MonoBehaviour
{
    public GameObject timer_Screen;
    public TextMeshProUGUI textTimer;
    public GameObject bg;
    public Image time_Img;

    float currentTime = 0f;
    bool isTimerRunning = false;
    float maxTime = 0f;
    public bool mix_Done = false;
    public bool mix_Ing = false;

    private void Awake()
    {
        timer_Screen.SetActive(true);
        bg.SetActive(false);
    }
    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                isTimerRunning = false;
                currentTime = 0;
                TimerFinished();
            }
            else
            {
                UpdateTimerDisplay();
            }
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
        textTimer.text = "�Ϸ�";
        Debug.Log("Timer finished!");
    }
    public void StartTimer(float _time)
    {
        currentTime = _time;
        maxTime = _time;
        isTimerRunning = true;
        bg.SetActive(true);
        UpdateTimerDisplay();
    }


    public IEnumerator MixerRoutine(Mixer mixer)
    {
        float time = 15f;

        yield return StartCoroutine(Mix(mixer, time));
      
    }


    public IEnumerator Mix(Mixer mixer, float time)
    {
        mixer.StartTimer(time);
        mix_Ing = true;
        yield return new WaitForSeconds(time);
        mix_Ing = false;
        mix_Done = true;
        Debug.Log("��ᰡ �� ���Ƚ��ϴ�.");
    }
}
