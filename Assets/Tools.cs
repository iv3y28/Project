using UnityEngine;

public class Tools : MonoBehaviour
{
    public static float fallingGravity = 3.5f;
    public static float gravity = 3f;
    public static GameObject player;
    public static PlayerController playerControl;
    
    
    /*
     Gravity stuff
     
    if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = Tools.fallingGravity;
        }
        else
        {
            rb.gravityScale = Tools.gravity;
        }
     */
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
