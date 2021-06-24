using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float jumpSpeed = 8;
    private const float moveSpeed = 6;
    private const float gravity = 20;
    private const float sensitvity = 4;

    private Vector3 moveDirection = Vector3.zero;
    private float yCameraRotation;

    private Camera playerCamera;
    private VoxelType selectedVoxel;
    private UIController uiController;

    private void Start()
    {
        playerCamera = Camera.main;
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
        float y = -Input.GetAxis("Mouse Y") * sensitvity;
        float x = Input.GetAxis("Mouse X") * sensitvity;

        yCameraRotation += y;
        yCameraRotation = Mathf.Clamp(yCameraRotation, -80, 80);

        if (x != 0) {
            transform.eulerAngles += new Vector3(0, x, 0);
        }
        if (y != 0) {
            playerCamera.transform.eulerAngles = new Vector3(yCameraRotation, transform.eulerAngles.y, 0);
        }
    }

    private void UpdatePosition()
	{
        var characterController = GetComponent<CharacterController>();

        if (characterController.isGrounded) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            moveDirection = transform.TransformDirection(new Vector3(x, 0, z));
            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space)) {
                moveDirection.y = jumpSpeed;
			}
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
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
