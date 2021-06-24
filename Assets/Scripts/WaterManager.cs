using System.Collections;

using UnityEngine;

public class WaterManager : MonoBehaviour
{
    private readonly float spawnWaterTimer = 1f;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public IEnumerator CheckCoroutine(GameObject waterCube)
	{
        var center = waterCube.transform.position;
        float radius = 2f;
        var hitColliders = Physics.OverlapSphere(center, radius);

        var positionUnder = center + new Vector3(0, -1, 0);
        var positionNorth = center + new Vector3(1, 0, 0);
        var positionEast = center + new Vector3(0, 0, 1);
        var waterPositionNorth = center + new Vector3(2, 0, 0);
        var waterPositionEast = center + new Vector3(0, 0, 2);

        bool hasVoxelUnder = false;
        bool hasVoxelNorth = false;
        bool hasVoxelEast = false;
        bool hasWaterVoxelNorth = false;
        bool hasWaterVoxelEast = false;

        yield return new WaitForSeconds(spawnWaterTimer);

        if (waterCube == null) {
            yield break;
		}

        foreach (var hitCollider in hitColliders) {
            if (hitCollider == null || hitCollider == gameObject) {
                continue;
            }
            if (hitCollider.transform.position == positionUnder) {
                hasVoxelUnder = true;
            }
            if (hitCollider.transform.position == positionNorth) {
                hasVoxelNorth = true;
            }
            if (hitCollider.transform.position == positionEast) {
                hasVoxelEast = true;
            }
            if (hitCollider.transform.position == waterPositionNorth && hitCollider.name == "WaterCube") {
                hasWaterVoxelNorth = true;
            }
            if (hitCollider.transform.position == waterPositionEast && hitCollider.name == "WaterCube") {
                hasWaterVoxelEast = true;
            }
        }
        bool needSpawnWaterNorth = !hasVoxelNorth && hasWaterVoxelNorth;
        bool needSpawnWaterEast = !hasVoxelEast && hasWaterVoxelEast;

        var worldGenerator = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();
        if (!hasVoxelUnder) {
            worldGenerator?.CreateVoxel(worldGenerator.WaterVoxel, positionUnder, checkWater: true);
        }
        if (needSpawnWaterNorth) {
            worldGenerator?.CreateVoxel(worldGenerator.WaterVoxel, positionNorth, checkWater: true);
        }
        if (needSpawnWaterEast) {
            worldGenerator?.CreateVoxel(worldGenerator.WaterVoxel, positionEast, checkWater: true);
        }
    }
}
