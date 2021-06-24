using System.Collections;

using UnityEngine;

public class CreeperController : MonoBehaviour
{
    private const float jumpSpeed = 8;
    private const float moveSpeed = 4;
    private const float gravity = 20;

    private const float timeToExplode = 3;
    private const float distanceToExplode = 3;
    private const float explosionRadius = 5;

    private GameObject player;
    private Renderer creeperRenderer;

    private Vector3 moveDirection = Vector3.zero;
    private float distanceToPlayer;
    private float timer;
    private bool waitToExplode;
    private IEnumerator blinkingCoroutine;

    private void Start()
    {
        player = GameObject.Find("Player").transform.Find("Body").gameObject;
        creeperRenderer = GetComponent<Renderer>();
    }


    private void Update()
    {
        UpdateDistanceToPlayer();
        UpdatePosition();
        UpdateExploding();
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

    private void UpdateExploding()
	{
        if (distanceToPlayer <= distanceToExplode) {
            if (!waitToExplode) {
                timer = 0;
                waitToExplode = true;
                blinkingCoroutine = Blinking();
                StartCoroutine(blinkingCoroutine);
            } else {
                timer += Time.deltaTime;
                if (timer >= timeToExplode) {
                    Explode();
				}
			}
        } else {
            if (waitToExplode) {
                timer = 0;
                waitToExplode = false;
                if (blinkingCoroutine != null) {
                    StopCoroutine(blinkingCoroutine);
                    creeperRenderer.material.color = Color.white;
                }
            }
		}
    }

    private void Explode()
	{
        var center = gameObject.transform.position;
        var hitColliders = Physics.OverlapSphere(center, explosionRadius);
        foreach (var collide in hitColliders) {
            switch (collide.name) {
                case "GrassCube":
                case "StoneCube":
                case "WaterCube":
                    Destroy(collide.gameObject);
                    break;
            }
        }
        Destroy(gameObject);
    }

    private IEnumerator Blinking()
	{
        while (true) {
            creeperRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            creeperRenderer.material.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
