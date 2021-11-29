using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody player;
    public int speed = 2;
    private float H = 0.0f;
    private float V = 0.0f;
    public float camHSpeed = 2.0f;
    public float camVSpeed = 2.0f;
    private float cH = 90.0f;
    private float cV = 0.0f;
    public static bool lookable = true;
    Camera cam;
   
  
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.lookable = true;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
        moveP();
    }
    void moveP()
    {
        H = Input.GetAxisRaw("Horizontal");
        V = Input.GetAxisRaw("Vertical");
        if (H != 0 || V!=0)
        {
            GetComponent<Player>().walking = true;
        }
        else
        {
            GetComponent<Player>().walking = false;
        }
        if (lookable)
        {
            cH += camHSpeed * Input.GetAxis("Mouse X");
            cV -= camVSpeed * Input.GetAxis("Mouse Y");
        }
        
        cV = Mathf.Clamp(cV, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(cV, 0, 0);
        player.transform.rotation = Quaternion.Euler(0, cH, 0);
        player.velocity = new Vector3(H, 0, V).normalized * speed + new Vector3(0, player.velocity.y, 0);
        player.velocity = transform.TransformDirection(player.velocity);
        
    }
   
}
