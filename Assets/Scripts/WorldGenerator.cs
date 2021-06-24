using System.Collections;

using UnityEngine;

public enum VoxelType
{
    Grass,
    Stone,
    Water,
    Bedrock,
}

public class WorldGenerator : MonoBehaviour
{
    private readonly int voxelsPerFrame = 5000;

    public int WorldSize;
    public int WorldHeight;

    public GameObject BedrockVoxel;
    public GameObject GrassVoxel;
    public GameObject StoneVoxel;
    public GameObject WaterVoxel;

    public void CreateVoxel(GameObject voxel, Vector3 position)
	{
        Instantiate(voxel, position, Quaternion.identity);
	}

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(GenerateWorld());
    }

    private IEnumerator GenerateWorld()
	{
        var map = new int[WorldSize, WorldSize];
        for (int x = 0; x < WorldSize; ++x) {
            for (int z = 0; z < WorldSize; ++z) {
                map[x, z] = Random.Range(1, WorldHeight);
            }
        }

        for (int i = 0; i < 25; ++i) {
            int x = Random.Range(1, WorldSize);
            int y = Random.Range(1, WorldSize);
            map[x, y] = WorldHeight * 30;
        }

        for (int k = 0; k < 5; ++k) {
            var smoothMap = new int[WorldSize, WorldSize];
            for (int x = 0; x < WorldSize; ++x) {
                for (int z = 0; z < WorldSize; ++z) {
                    var sum = 0;
                    var counter = 0;
                    for (int dx = -1; dx <= 1; ++dx) {
                        for (int dz = -1; dz <= 1; ++dz) {
                            var nx = x + dx;
                            var nz = z + dz;
                            if (nx >= 0 && nx < WorldSize && nz >= 0 && nz < WorldSize) {
                                sum += map[nx, nz];
                                counter++;
                            }
                        }
                    }
                    smoothMap[x, z] = sum / counter;
                }
            }
            for (int x = 0; x < WorldSize; ++x) {
                for (int z = 0; z < WorldSize; ++z) {
                    map[x, z] = smoothMap[x, z];
                }
            }
        }

        var voxelsInFrameCounter = 0;
        for (int x = 0; x < WorldSize; ++x) {
            for (int z = 0; z < WorldSize; ++z) {
                CreateVoxel(BedrockVoxel, new Vector3(x, 0, z));
                var height = map[x, z];
                for (int y = 1; y < height; ++y) {
                    CreateVoxel(GrassVoxel, new Vector3(x, y, z));
                    ++voxelsInFrameCounter;
                    if (voxelsInFrameCounter >= voxelsPerFrame) {
                        voxelsInFrameCounter = 0;
                        yield return new WaitForEndOfFrame();
					}
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
