using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class WorldGeneator : MonoBehaviour
{
    public int WorldSize;
    public int WorldHeight;

    public GameObject BedrockVoxel;
    public GameObject GrassVoxel;
    public GameObject StoneVoxel;
    public GameObject WaterVoxel;

    private void CreateVoxel(GameObject voxel, Vector3 position)
	{
        Instantiate(voxel, position, Quaternion.identity);
	}

    // Start is called before the first frame update
    private void Start()
    {
        for (int x = 0; x < WorldSize; ++x) {
            for (int z = 0; z < WorldSize; ++z) {
                var height = Random.Range(1, WorldHeight);
                for (int y = 0; y < height; ++y) {
                    CreateVoxel(GrassVoxel, new Vector3(x, y, z));
				}
			}
		}
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
