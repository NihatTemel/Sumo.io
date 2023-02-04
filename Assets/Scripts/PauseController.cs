using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject PauseButton;
    
    public void PauseGame() 
    {
        
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void ResumeButton() 
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }


}
