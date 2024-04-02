using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    public Transform mainCamera;                        // Referinta catre camera principala
    public CharacterController characterController;     // Referinta catre controller-ul de caractere

    [Header("Physics")]
    [Range(0.0f, 100.0f)]
    public float gravity = 9.807f;                      // Valoarea fortei de gravitatie
    [Range(0.0f, 1.0f)]
    public float decelerationRate = 0.3f;               // Rata de decelerare

    [Header("Walking")]
    public bool canWalk = false;                        // Indicator pentru permisiunea de a merge
    [HideInInspector]
    public bool isWalkingForward = false;               // Indicator pentru starea de mers inainte
    [HideInInspector]
    public bool isWalkingBackward = false;              // Indicator pentru starea de mers inapoi
    [HideInInspector]
    public bool isWalkingLeftward = false;              // Indicator pentru starea de mers la stanga
    [HideInInspector]
    public bool isWalkingRightward = false;             // Indicator pentru starea de mers la dreapta
    [Range(0.0f, 10.0f)]
    public float walkingSpeed = 1.3f;                   // Viteza de mers
    [Range(0.0f, 5.0f)]
    public float timeToWalking = 0.3f;                  // Timpul pentru atingerea vitezei de mers

    [Header("Running")]
    public bool canRun = false;                         // Indicator pentru permisiunea de a alerga
    [HideInInspector]
    public bool isRunning = false;                      // Indicator pentru starea de alergare
    [Range(0.0f, 10.0f)]
    public float runingSpeed = 3.0f;                    // Viteza de alergare
    [Range(0.0f, 5.0f)]
    public float timeToRunning = 1.0f;                  // Timpul pentru atingerea vitezei de alergare

    [Header("Crouching")]
    public bool canCrouch = false;                      // Indicator pentru permisiunea de a se stoarce
    [HideInInspector]
    public bool isCrouching = false;                    // Indicator pentru starea de stoarcere
    [Range(0.0f, 10.0f)]
    public float croughSpeed = 0.6f;                    // Viteza de stoarcere
    [Range(0.0f, 5.0f)]
    public float timeToCrouch = 0.3f;                   // Timpul pentru atingerea vitezei de stoarcere
    [Range(0.0f, 1.0f)]
    public float crouchDownSpeed = 0.6f;                // Viteza de coborare a stoarcerii
    [Range(0.0f, 1.0f)]
    public float crouchUpSpeed = 0.3f;                  // Viteza de ridicare a stoarcerii
    [Range(0.0f, 5.0f)]
    public float playerHeight = 2.0f;                   // Inaltimea jucatorului în pozitie normala
    [Range(0.0f, 5.0f)]
    public float croughHeight = 1.0f;                   // Inaltimea jucatorului cand este stoars

    [Header("Jumping")]
    public bool canJump = false;                        // Indicator pentru permisiunea de a sari
    [HideInInspector]
    public bool isJumping = false;                      // Indicator pentru starea de sarit
    [Range(0.0f, 10.0f)]
    public float jumpForce = 3.0f;                      // Forta saritului

    [Header("Climbing")]
    public bool canClimb = false;                       // Indicator pentru permisiunea de a catara
    [HideInInspector]
    public bool isClimbing = false;                     // Indicator pentru starea de catarare
    [Range(0.0f, 10.0f)]
    public float climbingSpeed = 1.0f;                  // Viteza de catarare

    [Header("Movement")]
    private Vector3 _moveDirection = Vector3.zero;      // Directia de miscare
    private float _currentSpeed;                        // Viteza de miscare curenta

    [Header("Input")]
    private float _vertical;                            // Input-ul vertical
    private float _horizontal;                          // Input-ul orizontal
    private bool _verticalBlock = false;                // Indicator pentru blocarea input-ului vertical
    private bool _horizontalBlock = false;              // Indicator pentru blocarea input-ului orizontal

    private void Update()
    {
        // Verificarea input-ului
        InputSystem();

        // Calcularea directiei de mișcare
        HandleDirection();

        // Gestionarea mișcării player-ului
        HandleMovement();

        // actiune de stoarcere
        HandleCrouching();

        // Actiune de sarit
        HandleJumping();
    }

    private void InputSystem()
    {
        // Verificarea daca player-ul poate merge
        if (canWalk)
        {
            isWalkingForward = Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0;
            isWalkingBackward = Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < 0;

            isWalkingLeftward = Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0;
            isWalkingRightward = Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0;

            _vertical = _verticalBlock ? Mathf.Lerp(_vertical, 0.0f, Time.deltaTime / decelerationRate) : _vertical;
            _verticalBlock = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S);

            _horizontal = _horizontalBlock ? Mathf.Lerp(_horizontal, 0.0f, Time.deltaTime / decelerationRate) : _horizontal;
            _horizontalBlock = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D);
        }

        // Verificarea daca player-ul poate alerga
        if (canRun)
        {
            isRunning = Input.GetKey(KeyCode.LeftShift) && isWalkingForward && !isWalkingBackward && !isCrouching && !isClimbing;
        }

        // Verificarea daca player-ul poate stoarce
        if (canCrouch)
        {
            isCrouching = Input.GetKey(KeyCode.LeftControl) && !isRunning && !isJumping && !isClimbing;
        }

        // Verificarea daca player-ul poate sari
        if (canJump)
        {
            isJumping = Input.GetKey(KeyCode.Space) && !isCrouching && !isClimbing;
        }
    }
    private void HandleDirection()
    {
        // Aplicarea fortei de gravitație
        if (!characterController.isGrounded && !isClimbing)
        {
            _moveDirection.y -= gravity * Time.deltaTime;
        }

        // Obținerea directiilor de baza de miscare
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Calcularea directiei de miscare
        float movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * _vertical) + (right * _horizontal);
        _moveDirection.y = movementDirectionY;

        // Aplicarea miscarei player-ului
        characterController.Move(_moveDirection * Time.deltaTime);
    }
    private void HandleMovement()
    {
        // Verificarea daca player-ul este in starea de mers
        if (isWalkingForward || isWalkingBackward || isWalkingLeftward || isWalkingRightward)
        {
            // Verificarea daca player-ul este in starea de alergare
            if (isRunning)
            {
                // Interpolare liniara intre viteza curenta si viteza de alergare
                _currentSpeed = Mathf.Lerp(_currentSpeed, runingSpeed, Time.deltaTime / timeToRunning);
            }
            else if (isCrouching)
            {
                // Interpolare liniară intre viteza curenta si viteza de stoarcere
                _currentSpeed = Mathf.Lerp(_currentSpeed, croughSpeed, Time.deltaTime / timeToCrouch);
            }
            else
            {
                // Interpolare liniara intre viteza curenta si viteza de mers
                _currentSpeed = Mathf.Lerp(_currentSpeed, walkingSpeed, Time.deltaTime / timeToWalking);
            }

            // Calcularea miscarii pe axele verticala și orizontala
            _vertical = !_verticalBlock ? _currentSpeed * Input.GetAxis("Vertical") : _vertical;
            _horizontal = !_horizontalBlock ? _currentSpeed * Input.GetAxis("Horizontal") : _horizontal;
        }
        else
        {
            // Interpolare liniara pentru reducerea input-ul vertical si orizontal
            _vertical = Mathf.Lerp(_vertical, 0.0f, Time.deltaTime / decelerationRate);
            _horizontal = Mathf.Lerp(_horizontal, 0.0f, Time.deltaTime / decelerationRate);
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0.0f, Time.deltaTime / decelerationRate);
        }
    }
    private void HandleCrouching()
    {
        if (Input.GetKey(KeyCode.LeftControl) && isCrouching)
        {
            // Calcularea inaltimii  pentru a se stoarce
            isCrouching = true;
            float Height = Mathf.Lerp(characterController.height, croughHeight, Time.deltaTime / crouchDownSpeed);
            characterController.height = Height;

        }
        else if (!Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.up), out RaycastHit CroughCheck, 0.8f, 1))
        {
            if (characterController.height != playerHeight)
            {
                // Calcularea inaltimii pentru a se ridica
                isCrouching = false;
                float Height = Mathf.Lerp(characterController.height, playerHeight, Time.deltaTime / crouchUpSpeed);
                characterController.height = Height;
            }
        }
    }
    private void HandleJumping()
    {
        // Verificarea daca player-ul poate sari si daca corespunde altor stari
        if (Input.GetKey(KeyCode.Space) && characterController.isGrounded && isJumping && !isCrouching && !isClimbing)
        {
            // Aplicarea fortei de săritura pe axa Y a directiei de miscare
            _moveDirection.y = jumpForce;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificarea dacă player-ul a intrat în coliziune cu scara
        if (other.CompareTag("Ladder") && canClimb)
        {
            isClimbing = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // Verificarea daca se afla in coliziune cu scara
        if (other.CompareTag("Ladder") && canClimb)
        {
            _moveDirection = new Vector3(0, Input.GetAxis("Vertical") * climbingSpeed * (-mainCamera.localRotation.x / 1.7f), 0);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Verificarea daca a iesit din coliziune cu scara
        if (other.CompareTag("Ladder") && canClimb)
        {
            isClimbing = false;
        }
    }
}
