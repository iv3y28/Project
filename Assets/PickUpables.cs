using System;
using UnityEditor.Rendering;
using UnityEngine;

public class PickUpables : MonoBehaviour
{
    
    public bool isActive = false;
    public bool isHolding = false;
    private Rigidbody2D rb;
    [SerializeField]
    private CircleCollider2D trigger;
    
    [SerializeField]
    private float radius = 2.5f;

    private Hold hold;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trigger = GetComponentInChildren<CircleCollider2D>();
        trigger.radius = radius;

        hold = GetComponent<Hold>();
        hold.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hold.enabled)
        {
            rb.position = hold.getPathPoint();
        }
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = Tools.fallingGravity;
        }
        else
        {
            rb.gravityScale = Tools.gravity;
        }
    }

    public void pickUp()
    {
        isHolding = true;
        trigger.enabled = false;
        toggleHold(true);
    }
    
    public void drop()
    {
        isHolding = false;
        trigger.enabled = true;
        toggleHold(false);
    }

    private void toggleHold(bool state)
    {
        hold.enabled = state;
    }

    private void instantiateHoldRadius()
    {
        hold.radius=radius;
    }
}
