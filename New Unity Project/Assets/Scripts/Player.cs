using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody player;
    public int speed = 2;
    private float H = 0.0f;
    private float V = 0.0f;
    public float camHSpeed = 2.0f;
    public float camVSpeed = 2.0f;
    private float cH = 0.0f;
    private float cV = 0.0f;
    public GameObject rock;
    public int rockspeed = 500;
    private bool throwable = true;
    Camera cam;
    public GameObject hand;
  
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Fire1") == 1 && throwable)
        {
            throwable = false;
            fire();
            StartCoroutine(timer());
        }
        moveP();
    }
    void moveP()
    {
        H = Input.GetAxisRaw("Horizontal");
        V = Input.GetAxisRaw("Vertical");
        cH += camHSpeed * Input.GetAxis("Mouse X");
        cV -= camVSpeed * Input.GetAxis("Mouse Y");
        cV = Mathf.Clamp(cV, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(cV, 0, 0);
        player.transform.rotation = Quaternion.Euler(0, cH, 0);
        player.velocity = new Vector3(H, 0, V).normalized * speed + new Vector3(0, player.velocity.y, 0);
        player.velocity = transform.TransformDirection(player.velocity);
        
    }
    void fire()
    {
        GameObject bullet = Instantiate(rock, hand.transform.position + cam.transform.forward, cam.transform.rotation);
        //bullet.GetComponent<Rigidbody>().AddForce(player.transform.position * rockspeed);
    }
    private IEnumerator timer()
    {
        
        yield return new WaitForSeconds(2.0f);
        throwable = true;
    }
}
