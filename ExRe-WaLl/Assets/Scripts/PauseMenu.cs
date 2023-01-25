using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
//     [SerializeField] GameObject heathbarMenu;
    public string HomesceneName;
    
    	public void Pause(){
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;    
	    }
        
    	public void Resume(){
                // count down 3 seconds
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;    
	    }
        
    	public void Restart(){
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex/*game scene*/);
	}

    	public void Home(){
            Time.timeScale = 1f;
            SceneManager.LoadScene(HomesceneName);
	    }
}
