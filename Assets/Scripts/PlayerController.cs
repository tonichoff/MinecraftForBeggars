using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float jumpPower = 300;
    private const float speed = 10;

    private Rigidbody body;
    private VoxelType selectedVoxel;
    private UIController uiController;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        selectedVoxel = VoxelType.Grass;
        uiController = GameObject.Find("HUD").GetComponent<UIController>();
        uiController.ChangeVoxelIco(selectedVoxel);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        UpdateLook();
        UpdatePosition();
        if (Input.GetMouseButtonDown(0)) {
            UpdateDestroyingVoxel();
        }
        if (Input.GetMouseButtonDown(1)) {
            UpdateCreatingVoxel();
        }
        UpdateSelectedVoxel();
    }

    private void UpdateLook()
	{
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        transform.eulerAngles += new Vector3(0, x, 0);

        var head = transform.Find("Head");
        if (head != null) {
            head.transform.eulerAngles += new Vector3(-y, 0, 0);
        }
    }

    private void UpdatePosition()
	{
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space)) {
            body.AddForce(Vector3.up * jumpPower);
        }

        var transformVector = body.transform.localToWorldMatrix.MultiplyVector(new Vector3(x, 0, z));
        var deltaPosition = transformVector * speed * Time.deltaTime;
        body.MovePosition(body.transform.position + deltaPosition);
    }

    private void UpdateDestroyingVoxel()
	{
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out var hit)) {
            switch (hit.collider.name) {
                case "GrassCube":
                case "StoneCube":
                case "WaterCube":
                    Destroy(hit.collider.gameObject);
                    break;
            }
        }
    }

    private void UpdateCreatingVoxel()
	{
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out var hit)) {
            if (hit.collider.name.Contains("Cube")) {
                var worldGenerator = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();
                GameObject voxel = selectedVoxel switch {
					VoxelType.Grass => worldGenerator.GrassVoxel,
					VoxelType.Stone => worldGenerator.StoneVoxel,
					VoxelType.Water => worldGenerator.WaterVoxel,
					_ => worldGenerator.GrassVoxel,
				};
				worldGenerator.CreateVoxel(voxel, hit.transform.position + hit.normal);
            }
        }
    }

    private void UpdateSelectedVoxel()
	{
        var selectedVoxel = this.selectedVoxel;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedVoxel = VoxelType.Grass;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedVoxel = VoxelType.Stone;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedVoxel = VoxelType.Water;
        }
        if (selectedVoxel != this.selectedVoxel) {
            this.selectedVoxel = selectedVoxel;
            uiController.ChangeVoxelIco(selectedVoxel);
		}
    }
}
