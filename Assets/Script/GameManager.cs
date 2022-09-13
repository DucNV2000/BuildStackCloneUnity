using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (MovingCube.CurrentCube != null) MovingCube.CurrentCube.Stop();

            _cubeSpawner.SpawnCube();
        }
    }
}