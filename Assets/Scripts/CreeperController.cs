using UnityEngine;

public class CreeperController : MonoBehaviour
{
    private const float jumpSpeed = 8;
    private const float moveSpeed = 4;
    private const float gravity = 20;

    private GameObject player;
    private Rigidbody body;

    private Vector3 moveDirection = Vector3.zero;
    private float distanceToPlayer;

    private void Start()
    {
        player = GameObject.Find("Player").transform.Find("Body").gameObject;
        body = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        UpdateDistanceToPlayer();
        UpdatePosition();
    }

    private void UpdateDistanceToPlayer()
	{
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
    }

    private void UpdatePosition()
	{
        var characterController = GetComponent<CharacterController>();

        if (characterController.isGrounded) {
            moveDirection = (player.transform.position - transform.position);
            moveDirection.Normalize();
            moveDirection *= moveSpeed;

            var rayOrigin = transform.position;
            var rayDirection = transform.position + moveDirection;
            rayDirection.y = rayOrigin.y;
            var ray = new Ray(rayOrigin, rayDirection);
            if (Physics.Raycast(ray, out var hit, 1f)) {
                if (hit.collider.name.Contains("Cube")) {
                    moveDirection.y = jumpSpeed;
                }
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void Explode()
	{

	}
}
