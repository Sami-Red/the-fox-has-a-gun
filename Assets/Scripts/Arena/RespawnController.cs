using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Vector3 respawnPoint;
    public float waitToRespawn;
    private GameObject thePlayer;

    void Start()
    {
        thePlayer = PlayerHealthController.instance.gameObject;
        respawnPoint = thePlayer.transform.position;
    }

    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());
    }

    IEnumerator RespawnCo()
    {
        thePlayer.SetActive(false);
        yield return new WaitForSeconds(waitToRespawn);

        thePlayer.transform.position = respawnPoint;
        thePlayer.SetActive(true);

        PlayerHealthController.instance.FillHealth();
    }
}
