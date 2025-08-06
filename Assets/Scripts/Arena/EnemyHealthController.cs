using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int health;
    public GameObject deathEffect;
    public TextMeshProUGUI display;
    public int botID;


    public void DamageEnemy(int damageAmount)
    {
        health -= damageAmount;
        if (display != null)
        {
            display.text = "ID: " + botID + "\nHP: " + health;
        }
        if (health <= 0)
        {
            if (display != null)
            {
                display.transform.SetParent(null);
                Destroy(display.gameObject, 1f);

                if (deathEffect != null)
                {
                    Instantiate(deathEffect, transform.position, transform.rotation);
                }

                Destroy(gameObject);
            }
        }
    }
}
