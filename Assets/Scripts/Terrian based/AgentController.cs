using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject linePrefab;
    private GameObject pathLineObject;
    private LineRenderer lineRenderer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pathLineObject = Instantiate(linePrefab);
        lineRenderer = pathLineObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                VisualizePath(hit.point);
            }
        }
    }
    #region shows path to player 
    void VisualizePath(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            Vector3[] adjustedCorners = AdjustPathToTerrain(path.corners);
            lineRenderer.positionCount = adjustedCorners.Length;
            lineRenderer.SetPositions(adjustedCorners);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
    #endregion
    #region adjusts heights of paths to navmesh
    Vector3[] AdjustPathToTerrain(Vector3[] pathCorners)
    {
        Vector3[] adjustedCorners = new Vector3[pathCorners.Length];

        for (int i = 0; i < pathCorners.Length; i++)
        {
            Vector3 corner = pathCorners[i];
            RaycastHit terrainHit;

            if (Physics.Raycast(new Vector3(corner.x, corner.y + 10f, corner.z), Vector3.down, out terrainHit, 20f))
            {
                adjustedCorners[i] = new Vector3(corner.x, terrainHit.point.y, corner.z);
            }
            else
            {
                adjustedCorners[i] = corner; 
            }
        }

        return adjustedCorners;
    }
    #endregion
}
