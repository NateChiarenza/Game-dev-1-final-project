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
    public AudioMixer audio;
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
            SceneManager.LoadScene("Jail_house");
    }
    public void ChangeVolume(float v)
    {
        audio.SetFloat("Volume", v); 
    }
    public void SetQuality(int q)
    {
        QualitySettings.SetQualityLevel(q);
    }
}
