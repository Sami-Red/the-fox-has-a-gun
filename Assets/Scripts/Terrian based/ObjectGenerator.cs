using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class ObjectGenerator : MonoBehaviour
{
    public TerrainGenerator terrainGenerator; // gets access to terraingen script
    public GameObject playerPrefab;
    public GameObject[] objects;
    public int objectsPerType;  

    private GameObject player;

    void Start()
    {
        if (!terrainGenerator.isTerrainReady) return;

        SpawnPlayer();

        foreach (GameObject prefab in objects)
        {
            GenerateObjectType(prefab, objectsPerType, "random");
        }
    }
    #region Spawns player in random area within 10 attempts
    private void SpawnPlayer()
    {
        Vector3 playerPos;
        int attempts = 0;

        do
        {
            playerPos = RandomPosition("random");

            if (IsPosAccess(playerPos, skipPlayerCheck: true))
            {
                player = Instantiate(playerPrefab, playerPos, Quaternion.identity);
                return;
            }

            attempts++;
        } while (attempts < 10);
    }
    #endregion
    #region Spawns objects based on isposacc method
    private void GenerateObjectType(GameObject prefab, int count, string placementRule)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos;
            int attempts = 0;

            do
            {
                pos = RandomPosition(placementRule);

                if (IsPosAccess(pos))
                {
                    GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
                    break;
                }

                attempts++;
            } while (attempts < 10);
        }
    }
    #endregion
    #region Checks if target pos is accessable on the navmesh then calculates path & confirms if path is complete
    // advance terrain section part A
    private bool IsPosAccess(Vector3 targetPos, bool skipPlayerCheck = false)
    {
        GameObject tempAgentObj = new GameObject("TempAgent");
        NavMeshAgent tempAgent = tempAgentObj.AddComponent<NavMeshAgent>();

        if (!skipPlayerCheck && player != null)
        {
            tempAgent.transform.position = player.transform.position;
        }
        else
        {
            tempAgent.transform.position = targetPos;
        }

        if (!NavMesh.SamplePosition(tempAgent.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            Destroy(tempAgentObj);
            return false;
        }

        tempAgent.transform.position = hit.position;

        NavMeshPath path = new NavMeshPath();
        bool pathExists = tempAgent.CalculatePath(targetPos, path) && path.status == NavMeshPathStatus.PathComplete;

        Destroy(tempAgentObj);

        return pathExists;
    }
    #endregion
    #region Random position within bounds and the height of terrain
    private Vector3 RandomPosition(string placementRule)
    {
        float x = Random.Range(0, terrainGenerator.terrainWidth);
        float z = Random.Range(0, terrainGenerator.terrainLength);
        float y = terrainGenerator.CurrentPositionHeight(x, z);

        return new Vector3(x, y, z);
    }
    #endregion
}
