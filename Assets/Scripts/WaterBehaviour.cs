using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    private readonly float spawnWaterTimer = 1f;

    private float timerToSpawnWaterUnder;
    private bool waitToSpawnWaterUnder;

    private float timerToSpawnWaterNorth;
    private bool waitToSpawnWaterNorth;

    private float timerToSpawnWaterEast;
    private bool waitToSpawnWaterEast;

    private void Start()
    {
        
    }

    private void Update()
    {
        var center = gameObject.transform.position;
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

        foreach (var hitCollider in hitColliders) {
            if (hitCollider == gameObject) {
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
        if (hasVoxelUnder && waitToSpawnWaterUnder) {
            waitToSpawnWaterUnder = false;
            timerToSpawnWaterUnder = 0f;
		}
        if (!needSpawnWaterNorth && waitToSpawnWaterNorth) {
            waitToSpawnWaterNorth = false;
            timerToSpawnWaterNorth = 0f;
		}
        if (!needSpawnWaterEast && waitToSpawnWaterEast) {
            waitToSpawnWaterEast = false;
            timerToSpawnWaterEast = 0f;
        }
        if (!hasVoxelUnder && !waitToSpawnWaterUnder) {
            waitToSpawnWaterUnder = true;
            timerToSpawnWaterUnder = 0f;
        }
        if (needSpawnWaterNorth && !waitToSpawnWaterNorth) {
            waitToSpawnWaterNorth = true;
            timerToSpawnWaterNorth = 0f;
        }
        if (needSpawnWaterEast && !waitToSpawnWaterEast) {
            waitToSpawnWaterEast = true;
            timerToSpawnWaterEast = 0f;
        }

        var worldGenerator = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();
        if (waitToSpawnWaterUnder) {
            timerToSpawnWaterUnder += Time.deltaTime;
            if (timerToSpawnWaterUnder >= spawnWaterTimer) {
                worldGenerator?.CreateVoxel(worldGenerator.WaterVoxel, positionUnder);
                timerToSpawnWaterUnder = 0f;
                waitToSpawnWaterUnder = false;
            }
		}
        if (waitToSpawnWaterNorth) {
            timerToSpawnWaterNorth += Time.deltaTime;
            if (timerToSpawnWaterNorth >= spawnWaterTimer) {
                worldGenerator?.CreateVoxel(worldGenerator.WaterVoxel, positionNorth);
                timerToSpawnWaterNorth = 0f;
                waitToSpawnWaterNorth = false;
            }
        }
        if (waitToSpawnWaterEast) {
            timerToSpawnWaterEast += Time.deltaTime;
            if (timerToSpawnWaterEast >= spawnWaterTimer) {
                worldGenerator?.CreateVoxel(worldGenerator.WaterVoxel, positionEast);
                timerToSpawnWaterEast = 0f;
                waitToSpawnWaterEast = false;
            }
        }
    }
}
