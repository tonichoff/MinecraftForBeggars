using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private readonly Vector3 selectedIconScale = new Vector3(0.65f, 0.65f, 1);
    private readonly Vector3 unselectedIconScale = new Vector3(0.5f, 0.5f, 1);

    private Image grassIco;
    private Image stoneIco;
    private Image waterIco;

    private VoxelType selectedVoxel;

    public void ChangeVoxelIco(VoxelType voxelType)
	{
        var unselectedIco = GetIcoByVoxelType(selectedVoxel);
        if (unselectedIco != null) {
            unselectedIco.transform.localScale = unselectedIconScale;
        }
        var selectedIco = GetIcoByVoxelType(voxelType);
        if (selectedIco != null) {
            selectedIco.transform.localScale = selectedIconScale;
        }
        selectedVoxel = voxelType;
    }

    private Image GetIcoByVoxelType(VoxelType voxelType)
	{
        switch (voxelType) {
            case VoxelType.Grass:
                return grassIco;
            case VoxelType.Stone:
                return stoneIco;
            case VoxelType.Water:
                return waterIco;
        }
        return null;
    }

    // Start is called before the first frame update
    private void Start()
    {
        grassIco = GameObject.Find("GrassIco").GetComponent<Image>();
        stoneIco = GameObject.Find("StoneIco").GetComponent<Image>();
        waterIco = GameObject.Find("WaterIco").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
