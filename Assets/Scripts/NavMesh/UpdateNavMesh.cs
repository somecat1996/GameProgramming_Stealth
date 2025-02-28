using UnityEngine;
using Unity.AI.Navigation;

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

    public void UpdateNavMeshSurface()
    {
        navMeshSurface.BuildNavMesh();
    }
}

