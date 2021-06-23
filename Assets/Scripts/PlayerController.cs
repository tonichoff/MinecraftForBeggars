using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float jumpPower = 300;
    private const float speed = 10;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

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
            rigidbody.AddForce(Vector3.up * jumpPower);
        }

        var transformVector = rigidbody.transform.localToWorldMatrix.MultiplyVector(new Vector3(x, 0, z));
        var deltaPosition = transformVector * speed * Time.deltaTime;
        rigidbody.transform.position += deltaPosition;
    }

    private void UpdateDestroyingVoxel()
	{
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out var hit)) {
            if (hit.collider.name == "Cube") {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void UpdateCreatingVoxel()
	{
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out var hit)) {
            if (hit.collider.name == "Cube") {
                var wg = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>();
                wg.CreateVoxel(wg.GrassVoxel, hit.transform.position + hit.normal);
            }
        }
    }
}
