using UnityEngine;
using Unity.AI.Navigation;

/// <summary>
/// Update NavMesh at runtime
/// </summary>
public class UpdateNavMesh : MonoBehaviour
{
    public static UpdateNavMesh instance;
    private NavMeshSurface navMeshSurface;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
    }

    /// <summary>
    /// Update NavMesh
    /// </summary>
    public void UpdateNavMeshSurface()
    {
        navMeshSurface.BuildNavMesh();
    }
}

