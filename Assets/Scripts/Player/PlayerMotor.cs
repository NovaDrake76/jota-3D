using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private bool sprinting = false;
    private bool crouching = false;
    private bool lerpCrouch = false;
    private float crouchTimer = 0;

    [Header("Shooting")]
    public Camera playerCamera;
    public float shootingRange = 100f;
    public float damage = 10f;
    public GameObject muzzleFlashPrefab;
    public GameObject Gun;
    public AudioClip shootingSound;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public Transform gunBarrel;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0;
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8;
        }
        else
        {
            speed = 5;
        }
    }

    public void Shoot()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            // play shooting sound
            if (shootingSound != null)
            {
                audioSource.PlayOneShot(shootingSound);
            }

            // instantiate muzzle flash
            if (muzzleFlashPrefab != null)
            {
                // Use the gun barrel's position and rotation
                GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, gunBarrel.position, gunBarrel.rotation);
                Destroy(muzzleFlash, 0.1f); // remove muzzle flash after a short time
            }

            // play shooting animation by setting RecoilTrigger parameter to true
            Gun.GetComponent<Animator>().SetTrigger("RecoilTrigger");



            // raycast to detect hit
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootingRange))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                    else
                    {
                        Debug.LogWarning("Hit object tagged as Enemy but no Enemy component found in parent.");
                    }
                }
                else
                {
                    Debug.Log("Hit object is not tagged as Enemy.");
                }
            }
        }
    }

}
