using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public CubeSpawner.MoveDirection MoveDirection { get; set; }

    private void Update()
    {
        if (MoveDirection == CubeSpawner.MoveDirection.z)
            transform.position += -transform.forward * Time.deltaTime * moveSpeed;
        else
            transform.position += transform.right * Time.deltaTime * moveSpeed;
    }

    private void OnEnable()
    {
        if (LastCube == null) LastCube = GameObject.Find("Start").GetComponent<MovingCube>();

        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y,
            LastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f),
            Random.Range(0, 1f));
    }

    private float GetHangOver()
    {
        if (MoveDirection == CubeSpawner.MoveDirection.z)
            return transform.position.z - LastCube.transform.position.z;
        return transform.position.x - LastCube.transform.position.x;
    }

    public void Stop()
    {
        moveSpeed = 0;
        var hangover = GetHangOver();
        var max = MoveDirection == CubeSpawner.MoveDirection.z
            ? LastCube.transform.localScale.z
            : LastCube.transform.localScale.x;
        if (Mathf.Abs(hangover) >= max)
        {
            CurrentCube = null;
            LastCube = null;
            SceneManager.LoadScene(0);
        }

        var direction = hangover > 0 ? 1f : -1f;
        if (MoveDirection == CubeSpawner.MoveDirection.z)
            SplitCubeOnZ(hangover, direction);
        else
            SplitCubeOnX(hangover, direction);
        LastCube = this;
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        var newSizeX = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        var fallingBlockSize = transform.localScale.x - newSizeX;
        var newPositionZ = LastCube.transform.position.x + hangover / 2;
        transform.localScale = new Vector3(newSizeX, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newPositionZ, transform.position.y, transform.position.z);

        var cubeEdge = transform.position.x + newSizeX / 2 * direction;
        var fallingBlockPositionX = cubeEdge + fallingBlockSize / 2 * direction;
        SpawnDropCube(fallingBlockPositionX, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        var newSizeZ = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        var fallingBlockSize = transform.localScale.z - newSizeZ;
        var newPositionZ = LastCube.transform.position.z + hangover / 2;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newSizeZ);
        transform.position = new Vector3(transform.position.x, transform.position.y, newPositionZ);

        var cubeEdge = transform.position.z + newSizeZ / 2 * direction;
        var fallingBlockPositionZ = cubeEdge + fallingBlockSize / 2 * direction;
        SpawnDropCube(fallingBlockPositionZ, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (MoveDirection == CubeSpawner.MoveDirection.z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z);
        }

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1f);
    }
}