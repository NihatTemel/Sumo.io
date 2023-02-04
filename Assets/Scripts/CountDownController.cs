using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CountDownController : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] int Timer;
    [SerializeField] GameObject FailPanel;
    void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()                     // Countdown for game. when it 0 lose game and open Fail panel
    {

        for (int i = 0; i < Timer; i++)
        {
            timerText.text = "Time : " + (Timer - i);
            yield return new WaitForSeconds(1);
        }
        timerText.text = "Time : 0";
        FailPanel.SetActive(true);
        Time.timeScale = 0;
    }
    
}
