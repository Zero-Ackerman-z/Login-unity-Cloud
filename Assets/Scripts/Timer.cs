using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLimit = 30f;
    public TextMeshProUGUI timerText;
    private bool isGameOver = false;

    private void Start()
    {
        UpdateTimerText();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 10 && !isGameOver)
            {
                AudioManager.Instance.SwitchToAlternativeMusic();
                isGameOver = true; // Asegura que no se cambie varias veces
            }
            if (timeLimit <= 0)
            {
                timeLimit = 0;
                // Notifica al GameManager que el tiempo ha terminado
                GameManager.Instance.OnTimeExpired();
            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
      {
        timerText.text = "Time: " + Mathf.RoundToInt(timeLimit).ToString();
    }

    public void AddTime(float extraTime)
    {
        timeLimit += extraTime;
    }

    public float GetRemainingTime()
    {
        return timeLimit;
    }

    public void SetRemainingTime(float time)
    {
        timeLimit = time;
    }

    public void SetGameOver(bool value)
    {
        isGameOver = value;
    }
}
