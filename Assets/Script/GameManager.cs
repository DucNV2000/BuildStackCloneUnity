using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CubeSpawner[] _cubeSpawners;
    private CubeSpawner currentSpawner;
    private int spawnerIndex;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (MovingCube.CurrentCube != null) MovingCube.CurrentCube.Stop();
            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            currentSpawner = _cubeSpawners[spawnerIndex];
            currentSpawner.SpawnCube();
        }
    }
}