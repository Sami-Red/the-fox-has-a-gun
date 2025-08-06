using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    public Transform exitPoint;
    public Transform player;
    public float spawnYOffset = 0.5f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.FillHealth();

            RespawnController.instance.Respawn();
        }
    }
}
