using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private MovingCube cubePrefabs;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefabs.transform.localScale);
    }

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefabs);
        if (MovingCube.LastCube != null &&
            MovingCube.LastCube != GameObject.Find("Start"))
            cube.transform.position = new Vector3(transform.position.x,
                MovingCube.LastCube.transform.position.y + MovingCube.LastCube.transform.localScale.y / 2 +
                cubePrefabs.transform.localScale.y / 2, transform.position.z);
        else
            cube.transform.position = transform.position;
    }
}