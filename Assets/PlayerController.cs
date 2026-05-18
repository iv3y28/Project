using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 5f;
    private float jumpForce = 12.5f;
    private float jumpCutOff = .5f;
    private bool isGrounded = true;
    private float readMoveValue;
    private readonly ContactPoint2D[] contacts = new ContactPoint2D[8];
    
    public float lastDirection=-1f;

    private GameObject activeObject = null;
    private GameObject anObject = null;
    private PickUpables activeObjectScript = null;
    private bool objectActive = false;
    private bool pickedUp = false;
    private bool readyToPickUp = true;

    public Vector2 aim = new Vector2();

    public bool isGamepad = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
    }
    
    // Receive inputs from Update() and apply to FixedUpdate()

    // Update is called once per frame
    // Use this for inputs and then visuals and stuff like that
    void Update()
    {
        
    }
    
    // Use this for physics
    void FixedUpdate()
    {
        // Moving
        rb.linearVelocityX = readMoveValue * speed;
        
        //Checks if ground
        CheckGrounded();
        
        // Increase gravity while falling
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = Tools.fallingGravity;
        }
        else
        {
            rb.gravityScale = Tools.gravity;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = false;

        int count = rb.GetContacts(contacts);

        for (int i = 0; i < count; i++)
        {
            if (contacts[i].normal.y > 0.9f)
            {
                isGrounded = true;
                return;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("pickUpable"))
        {
            anObject = other.gameObject;
            objectActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("pickUpable"))
        {
            anObject = null;
            objectActive = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context) 
    {
        readMoveValue=context.ReadValue<float>();
        if (readMoveValue != 0)
        {
            if (readMoveValue < 0)
            {
                lastDirection = -1f;
            }else if (readMoveValue > 0)
            {
                lastDirection = 1f;
            }
        }
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.linearVelocityY = jumpForce;
        }

        if (context.canceled && rb.linearVelocityY > 0)
        {
            rb.linearVelocityY *= jumpCutOff;
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.canceled && objectActive && readyToPickUp)
        {
            fillObjectVariables();
            pickedUp = true;
            readyToPickUp = false;
        }

        if (context.canceled && !readyToPickUp && !pickedUp)
        {
            readyToPickUp = true;
        }

        if (pickedUp)
        {
            if (!activeObjectScript.isHolding)
            {
                activeObjectScript.pickUp();
            }
            else
            {
                activeObjectScript.drop();
                pickedUp = false;
                activeObject = null;
                activeObjectScript = null;
            }
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValue<Vector2>();
        var device=context.control.device;
        if (device is Mouse)
        {
            isGamepad = false;
        }
        else if(device is Gamepad)
        {
            isGamepad = true;
        }
    }

    public void OnAction1(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            rb.MovePosition(new Vector2(-4.39f, 0));
            rb.linearVelocity = new Vector2(0, 0);
        }
    }

    private void fillObjectVariables()
    {
        if (activeObject == null)
        {
            activeObject = anObject;
            activeObjectScript = activeObject.GetComponentInParent<PickUpables>();
        }
    }

}
