using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool playerExiting;
    public Transform exitPoint; 
    public float spawnYOffset = 0.5f; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerExiting)
        {
            StartCoroutine(UseDoorCo(other.transform));
        }
    }

    IEnumerator UseDoorCo(Transform player)
    {
        playerExiting = true;
        UIController.instance?.StartFadeFromBlack();
        yield return new WaitForSeconds(0.1f);
        player.position = new Vector3(exitPoint.position.x, exitPoint.position.y + spawnYOffset, exitPoint.position.z);
        playerExiting = false;
    }
}
