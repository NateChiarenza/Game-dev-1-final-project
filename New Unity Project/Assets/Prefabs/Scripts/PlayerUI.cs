using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public Text health;
    public Text arrows;
    public Text Timer;
    public Text detected;
    public Text Interact;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        health.text = player.GetComponent<Player>().hp.ToString();
        arrows.text = Player.arrowsLeft.ToString();
        Interact.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        health.text = player.GetComponent<Player>().hp.ToString();
        arrows.text = Player.arrowsLeft.ToString();
        Timer.text = System.TimeSpan.FromSeconds((int)Time.fixedTime).ToString();
        if (player.GetComponent<Player>().detected)
        {
            detected.text = "DETECTED";
        }
        else
        {
            detected.text = "";
        }
        
    }
}
