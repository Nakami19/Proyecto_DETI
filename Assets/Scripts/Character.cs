using System.Collections;
using Unity.Cinemachine;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Variables

    Camera mainCamera;
    [SerializeField] private CinemachineCamera freeLookCam;
    //public GameManager gameManager;


    [Header("Variables de movimiento")]
    public float speed = 5f;
    public float currentSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    Vector3 currentMoveVelocity = Vector3.zero;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float bounceHeight = 15f;


    [Header("Variables para checkear wallrun")]
    public CollisionDetectorRaycast leftCollider;
    public CollisionDetectorRaycast rightCollider;

    [Header("Variable para checkear piso")]
    public CollisionDetectorRaycast downCollider;
    public bool grounded;

    [Header("Variables del Wallrun")]
    [SerializeField] private float wallJumpCooldown = 2f;
    private float wallJumpCooldownTimer = 0f;
    Vector3 wallRunDirection;
    public bool fallWhileWallrunning = true;
    public float wallRunJumpHeight = 3f;
    public float wallRunSpeedTreshold = 3.5f;
    public float wallRunGravity = -20f;
    public float maxFallSpeed = -2f;
    public float wallJumpForceMultiplier = 1.5f;
    bool isWallRunning = false;
    bool isTouchingWallLeft;
    bool isTouchingWallRight;
    bool isInAir;
    bool onRightWall;

    [Header("Variables del Disparo")]
    //public GameObject bullet;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;

    [Header("Efectos de sonido")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip shootSound;

    [Header("Daño por caída")]
    [SerializeField] private float fallDamageThreshold = -10f;
    [SerializeField] private int fallDamageAmount = 1;
    private bool wasGroundedLastFrame = true;
    private float lastVerticalVelocity = 0f;


    //Variables de plataformas
    Transform currentPlatform = null;
    PlatformMovementDetector movingPlatform = null;


    CharacterController controller;

    PlayerControls playerControls;
    Vector2 moveInput;

    float verticalVelocity;
    bool wantJump;
    bool wantWallJump;

    #endregion


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); //Lee el movimiento
        playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero; //Captura fin del movimiento
        playerControls.Player.Jump.performed += ctx =>
        {
            if (controller.isGrounded || grounded)
                wantJump = true;
            else if (isWallRunning)
                wantWallJump = true;
        }; //Lee el salto
    }

    void OnEnable()
    {
        playerControls.Enable(); //Habilita el Input Action
    }

    void OnDisable()
    {
        playerControls.Disable(); //Deshabilita el Input Action
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Bloquea el cursor en el centro de la pantalla
        Cursor.visible = false; //Oculta el cursor
        mainCamera = Camera.main; //Obtiene la camara principal

        //gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Guardamos la velocidad vertical antes de actualizarla
        lastVerticalVelocity = verticalVelocity;

        grounded = downCollider.isColliding;
        //Debug.Log("Esta en el piso:" + grounded);

        // Detectamos aterrizaje
        if (!wasGroundedLastFrame && grounded)
        {
            // Comprobamos si la velocidad vertical fue suficientemente rápida hacia abajo
            if (lastVerticalVelocity <= fallDamageThreshold)
            {
                TakeFallDamage();
            }
        }

        WallRun();
        if (wallJumpCooldownTimer > 0f)
        {
            wallJumpCooldownTimer -= Time.deltaTime;
        }

        wasGroundedLastFrame = grounded || controller.isGrounded;

        Jump();
        PlayerMove();

        if (grounded && currentPlatform != null && movingPlatform != null)
        {
            // Apply platform movement
            controller.Move(movingPlatform.DeltaPosition);

            // Apply platform rotation around platform's pivot
            Vector3 relativePos = transform.position - currentPlatform.position;
            relativePos = movingPlatform.DeltaRotation * relativePos;
            Vector3 newWorldPos = currentPlatform.position + relativePos;

            controller.Move(newWorldPos - transform.position);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            GameManager.Instance.takeDamage(1);
            AudioManager.Instance.playSound(hurtSound); //Reproduce el sonido de da�o
            //Object.FindFirstObjectByType<GameManager>().takeDamage(1); //Prueba de da�o al jugador
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void TakeFallDamage()
    {
        Debug.Log("Daño por caída aplicado");

        //Llamo funcion para tomar daño
        GameManager.Instance.takeDamage(fallDamageAmount);

        // Reproducir efecto de daño
        AudioManager.Instance.playSound(hurtSound);
        UIManager.Instance.PlayerIconDamage();
    }

    void PlayerMove()
    {
        if (isWallRunning) return;

        //Movimiento plano XZ relativo a la camara

        Vector3 camForward = Camera.main.transform.forward; camForward.y = 0f; camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right; camRight.y = 0f; camRight.Normalize();

        Vector3 targetMove = (camRight * moveInput.x + camForward * moveInput.y) * currentSpeed;
        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, targetMove, acceleration * Time.deltaTime);


        //Aplica movimiento y gravedad
        Vector3 velocity = currentMoveVelocity + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime); //Aplica el movimiento al CharacterController
    }

    void Jump()
    {
        if (controller.isGrounded || grounded)
        {
            if (wantJump)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity); //Salto
                wantJump = false;
            }
            else
            {
                // Solo si no estamos rebotando o saltando, mantenemos en el suelo.
                if (verticalVelocity < 0.1f)
                {
                    verticalVelocity = -1f; // Mantiene al jugador en el suelo cuando no hay otra fuerza vertical
                }
            }
           
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; //Aplica gravedad
        }
    }

    #region WallRun
    void WallRun()
    {

        if (wallJumpCooldownTimer > 0f)
        {
            isWallRunning = false;
            return;
        }

        float planarSpeed = new Vector2(currentMoveVelocity.x, currentMoveVelocity.z).magnitude;
        bool isSpeedEnough = planarSpeed >= wallRunSpeedTreshold;
        bool isMovingForward = moveInput.y > 0.1f;

        isTouchingWallLeft = leftCollider.isColliding;
        isTouchingWallRight = rightCollider.isColliding;
        isInAir = !grounded;

        if ((isTouchingWallLeft || isTouchingWallRight) && isInAir && isSpeedEnough && isMovingForward)
        {
            isWallRunning = true;
            onRightWall = isTouchingWallRight;

            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            wallRunDirection = camForward;

            if (wantWallJump)
            {
                wantWallJump = false;
                isWallRunning = false;
                wallJumpCooldownTimer = wallJumpCooldown;

                Vector3 awayFromWall = onRightWall ? -Camera.main.transform.right : Camera.main.transform.right;
                awayFromWall.y = 0f;
                awayFromWall.Normalize();

                Vector3 jumpDirection = (awayFromWall + Vector3.up).normalized;

                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                currentMoveVelocity = jumpDirection * currentSpeed * wallJumpForceMultiplier;

                return;
            }

            if (fallWhileWallrunning)
            {
                verticalVelocity = Mathf.Max(verticalVelocity + wallRunGravity * Time.deltaTime, maxFallSpeed);
            }
            else
            {
                verticalVelocity = 0f;
            }

            Vector3 velocity = wallRunDirection * currentSpeed + Vector3.up * verticalVelocity;
            controller.Move(velocity * Time.deltaTime);

        }
        else
        {
            isWallRunning = false;
        }
    }

    #endregion

    // Detectar colision
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Detectar trampolin
        if (hit.collider.CompareTag("Trampoline"))
        {

            //Llamo al salto
            Bounce(bounceHeight);
        }

        if (controller.isGrounded || grounded)
        {
            PlatformMovementDetector mp = hit.collider.GetComponent<PlatformMovementDetector>();
            if (mp != null)
            {
                currentPlatform = mp.transform;
                movingPlatform = mp;
            }
            else
            {
                currentPlatform = null;
                movingPlatform = null;
            }
        }
    }

    //Funcion para impulsar salto
    public void Bounce(float force)
    {
        verticalVelocity = Mathf.Sqrt(force * -2f * gravity);
    }

    public void Shoot()
    {
        GameObject bullet = BulletPool.Instance.requestBullet();

        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        AudioManager.Instance.playSound(shootSound); //Reproduce el sonido de disparo

        //Debug.Log("Bullet shot from " + bulletSpawnPoint.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            this.OnDisable();
            freeLookCam.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Dano");
            GameManager.Instance.takeDamage(1);
        }
    }

}
