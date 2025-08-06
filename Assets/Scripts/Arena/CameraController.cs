using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController player;
    public BoxCollider2D boundBox;

    private float halfHeight, halfWidth;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfWidth * Camera.main.aspect;
    }
    void Update()
    {
        if(player != null)
        {
            transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, boundBox.bounds.min.x + halfWidth, boundBox.bounds.max.x - halfWidth), 
                Mathf.Clamp(player.transform.position.y, boundBox.bounds.min.y + halfHeight, boundBox.bounds.max.y + halfHeight), transform.position.z);
        }
    }
}
        