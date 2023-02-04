using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [Header("Start Timer")]
    [SerializeField] GameObject StartTimerText;
    [SerializeField] float StartSpeed;

    [Header("Player Info")]
    [SerializeField] GameObject Player;
    PlayerController pc;
    [SerializeField] TMP_Text PlayerScoreText;

   
    int score;
    private Vector3 initialScale;
    void Start()
    {

        initialScale = PlayerScoreText.GetComponent<RectTransform>().localScale;
        pc = Player.GetComponent<PlayerController>();

        Time.timeScale = 1;
        StartCoroutine(TextCountDown());
        StartTimerText.transform.DOScale(Vector3.zero, StartSpeed).SetEase(Ease.InBack);
    }

    

    IEnumerator TextCountDown() 
    {
        Vector3 TimerStartSize = StartTimerText.transform.localScale;
        int CountDown = 3;
        for (int i = 0; i < 3; i++)                                                             // Countdown settings
        {
            StartTimerText.transform.DOScale(Vector3.zero, StartSpeed).SetEase(Ease.InBack);        // Change countdown timer scale in every StartSpeed time.
            yield return new WaitForSeconds(StartSpeed);
            StartTimerText.transform.localScale = TimerStartSize;
            CountDown--;
            StartTimerText.GetComponent<TMP_Text>().text = "" + CountDown;

        }
        StartTimerText.GetComponent<TMP_Text>().text = "GO";                                    
        StartTimerText.transform.DOLocalMoveY(5000, 0.7f).SetEase(Ease.InOutQuad);          // when the countdown is over the game should start and text is change to GO and disappear from view
        yield return new WaitForSeconds(0.4f);

        GameStart();    

    }
    

    void GameStart() 
    {
        pc.enabled = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().enabled = true;
        }
        
    }
    
    void PlayerScore() 
    {
        if(score !=  Player.GetComponent<PlayerController>().pushPower * 100)           // Check the Player's score by using its pushPower value.
        {
            score = Player.GetComponent<PlayerController>().pushPower * 100;
            PlayerScoreText.text = "" + score;

           
            PlayerScoreText.rectTransform.DOScale(initialScale * 1.5f, 0.5f)                // Score change animation.
            .OnComplete(() => PlayerScoreText.rectTransform.DOScale(initialScale, 0.5f));

        }
        
        
    }

    private void FixedUpdate()
    {
        PlayerScore();
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(0);
    }

}
