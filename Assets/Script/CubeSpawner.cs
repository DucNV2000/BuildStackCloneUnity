using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public enum MoveDirection
    {
        x,
        z
    }

    [SerializeField] private MovingCube cubePrefabs;
    [SerializeField] private MoveDirection moveDirection;

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
        {
            var x = moveDirection == MoveDirection.x
                ? transform.position.x
                : MovingCube.LastCube.transform.position.x;
            var z = moveDirection == MoveDirection.z
                ? transform.position.z
                : MovingCube.LastCube.transform.position.z;

            cube.transform.position = new Vector3(x,
                MovingCube.LastCube.transform.position.y + MovingCube.LastCube.transform.localScale.y / 2 +
                cubePrefabs.transform.localScale.y / 2, z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.MoveDirection = moveDirection;
    }
}