using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject GameOver;
    [SerializeField] GameObject Control;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthHeartManager.health <= 0)
        {
            GameOver.SetActive(true);
            Control.SetActive(false);
            Time.timeScale = 0f;
        }
    }
}
