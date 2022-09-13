using UnityEngine;

public class MovingCube : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }

    private void Update()
    {
        transform.position += -transform.forward * Time.deltaTime * moveSpeed;
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

    public void Stop()
    {
        moveSpeed = 0;
        var hangover = transform.position.z - LastCube.transform.position.z;
        if (Mathf.Abs(hangover) >= LastCube.transform.localScale.z)
        {
            CurrentCube = null;
            LastCube = null;
        }

        var direction = hangover > 0 ? 1f : -1f;
        SplitCubeOnZ(hangover, direction);
        LastCube = this;
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

    private void SpawnDropCube(float fallingBlockPositionZ, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
        cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPositionZ);
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1f);
    }
}