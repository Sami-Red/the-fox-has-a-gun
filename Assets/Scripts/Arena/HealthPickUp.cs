using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.HealPlayer(healthAmount);
            Destroy(gameObject);
        }
    }
}
