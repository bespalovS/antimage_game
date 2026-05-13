using System.Collections;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshSurfaceManagement : MonoBehaviour
{
    public static NavMeshSurfaceManagement Instance { get; private set; }

    private NavMeshSurface navMeshSurface;

    private bool rebakeQueued;

    private void Awake()
    {
        Instance = this;
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.hideEditorLogs = true;
    }

    public void RebakeNavMeshSurface()
    {
        navMeshSurface.BuildNavMesh();
    }

    public void RequestRebake()
    {
        if (rebakeQueued) return;

        rebakeQueued = true;
        StartCoroutine(RebakeRoutine());
    }

    private IEnumerator RebakeRoutine()
    {
        yield return null;

        RebakeNavMeshSurface();

        rebakeQueued = false;
    }

}
