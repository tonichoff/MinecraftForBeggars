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

    private WaterManager waterManager;

    public void CreateVoxel(GameObject voxel, Vector3 position, bool checkWater = false)
	{
        Instantiate(voxel, position, Quaternion.identity);
        if (checkWater) {
            CheckWater(position);
        }
	}

    public void DestroyVoxel(GameObject voxel)
	{
        switch (voxel.name) {
            case "GrassCube":
            case "StoneCube":
            case "WaterCube":
                Destroy(voxel);
                CheckWater(voxel.transform.position);
                break;
        }
	}

    // Start is called before the first frame update
    private void Start()
    {
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
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
                ++voxelsInFrameCounter;
                var height = map[x, z];
                if (height <= 3) {
                    var firstVoxel = Random.Range(0, 1) == 0 ? BedrockVoxel : StoneVoxel;
                    CreateVoxel(firstVoxel, new Vector3(x, 1, z));
                    CreateVoxel(WaterVoxel, new Vector3(x, 2, z));
                    CreateVoxel(WaterVoxel, new Vector3(x, 3, z));
                    voxelsInFrameCounter += 3;
                    if (voxelsInFrameCounter >= voxelsPerFrame) {
                        voxelsInFrameCounter = 0;
                        yield return new WaitForEndOfFrame();
                    }
                } else {
                    for (int y = 1; y < height; ++y) {
                        CreateVoxel(StoneVoxel, new Vector3(x, y, z));
                        ++voxelsInFrameCounter;
                        if (voxelsInFrameCounter >= voxelsPerFrame) {
                            voxelsInFrameCounter = 0;
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
                CreateVoxel(GrassVoxel, new Vector3(x, height, z));
                ++voxelsInFrameCounter;
            }
        }
    }

    private void Update()
    {
        
    }

    private void CheckWater(Vector3 center)
    {
        var waterCandidates = Physics.OverlapSphere(center, 2);
        foreach (var candidate in waterCandidates) {
            if (candidate.name == "WaterCube") {
                StartCoroutine(waterManager.CheckCoroutine(candidate.gameObject));
            }
        }
    }
}
