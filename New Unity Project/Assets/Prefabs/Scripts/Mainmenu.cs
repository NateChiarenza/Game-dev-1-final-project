using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class Mainmenu : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    public AudioMixer volume;
    public static bool gamePaused = false;
    public GameObject pauseScreen;
    public GameObject FailScreen;
    public bool main = false;
    private void Start()
    {
        if (main)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    private void Update()
    {
        if (!main)
        {
            if (GameObject.Find("Player").GetComponent<Player>().dead)
            {
                FailScreen.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                PlayerController.lookable = false;
                Time.timeScale = 0f;
            }
        }
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }
    public void setPannel(int p)
    {
        switch (p)
        {
            case 1:
                p1.SetActive(true);
                p2.SetActive(false);
                break;
            case 2:
                p1.SetActive(false);
                p2.SetActive(true);
                break;
            
        }
    }
    public void nextLevel()
    {
            SceneManager.LoadScene(1);
    }
    public void ChangeVolume(float v)
    {
        volume.SetFloat("Volume", v); 
    }
    public void SetQuality(int q)
    {
        QualitySettings.SetQualityLevel(q);
    }
    public void pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        PlayerController.lookable = false;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }
    public void resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.SetActive(false);
        p1.SetActive(false);
        p2.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        PlayerController.lookable = true;
    }
    public void restart()
    {
        Scene retry = SceneManager.GetActiveScene();
        SceneManager.LoadScene(retry.buildIndex);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
