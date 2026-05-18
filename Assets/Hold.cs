using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Hold : MonoBehaviour
{
    private Vector2 transform=Tools.player.GetComponent<Rigidbody2D>().position;
    public float radius=2.5f;

    private Vector2 pathPoint;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Tools.playerControl.isGamepad)
        {
            GetStickCirclePoint(transform, radius, Tools.playerControl.aim,new Vector2(Tools.playerControl.lastDirection*radius,0));
        }else if (!Tools.playerControl.isGamepad)
        {
            GetMouseCirclePoint(transform, radius);
        }
    }
    
    private void GetMouseCirclePoint(Vector2 center, float radius)
    {
        Vector2 mouseWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = mouseWorld - center;

        if (dir == Vector2.zero)
            dir = Vector2.right; // fallback

        pathPoint = center + dir.normalized * radius;
    }
    
    private void GetStickCirclePoint(
        Vector2 center,
        float radius,
        Vector2 stickInput,
        Vector2 defaultDir
    )
    {
        Vector2 dir =
            stickInput.sqrMagnitude > 0.01f
                ? stickInput
                : defaultDir;

        pathPoint = center + dir.normalized * radius;
    }

    public Vector2 getPathPoint()
    {
        return pathPoint;
    }
}
