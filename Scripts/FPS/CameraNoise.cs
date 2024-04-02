using Cinemachine;
using UnityEngine;

public class CameraNoise : MonoBehaviour
{
    [Header("Virtual Camera")]
    public CinemachineVirtualCamera virtualCamera;  // Referinta catre camera Cinemachine

    [Header("Idle Camera Noise")]
    [Range(1.0f, 10.0f)]
    public float idleAmplitude = 2.0f;              // Amplitudinea pentru starea de repaus
    [Range(1.0f, 10.0f)]
    public float idleFrequency = 1.0f;              // Frecventa pentru starea de repaus

    [Header("Walking Camera Noise")]
    public bool canWalk = false;                    // Indicator pentru permisiunea de a merge
    [HideInInspector]
    public bool isWalkingForward = false;           // Indicator pentru starea de mers inainte
    [HideInInspector]
    public bool isWalkingBackward = false;          // Indicator pentru starea de mers inapoi
    [HideInInspector]
    public bool isWalkingLeftward = false;          // Indicator pentru starea de mers la stanga
    [HideInInspector]
    public bool isWalkingRightward = false;         // Indicator pentru starea de mers la dreapta
    [Range(1.0f, 10.0f)]
    public float walkingAmplitude = 4.0f;           // Amplitudinea pentru mers
    [Range(1.0f, 10.0f)]
    public float walkingFrequency = 1.5f;           // Frecvența pentru mers

    [Header("Running Camera Noise")]
    public bool canRun = false;                     // Indicator pentru permisiunea de a alerga
    [HideInInspector]
    public bool isRunning = false;                  // Indicator pentru starea de alergare
    [Range(1.0f, 10.0f)]
    public float runningAmplitude = 6.0f;           // Amplitudinea pentru alergare
    [Range(1.0f, 10.0f)]
    public float runningFrequency = 2.0f;           // Frecventa pentru alergare

    [Header("Crouching Camera Noise")]
    public bool canCrouch = false;                  // Indicator pentru permisiunea de a se stoarce
    [HideInInspector]
    public bool isCrouching = false;                // Indicator pentru starea de stoarcere
    [Range(1.0f, 10.0f)]
    public float crouchAmplitude = 4.0f;            // Amplitudinea pentru stoarcere
    [Range(1.0f, 10.0f)]
    public float crouchFrequency = 1.5f;            // Frecvența pentru stoarcere

    [Header("Jumping Camera Noise")]
    public bool canJump = false;                    // Indicator pentru permisiunea de a sari
    [HideInInspector]
    public bool isJumping = false;                  // Indicator pentru starea de sarit
    [Range(1.0f, 10.0f)]
    public float jumpAmplitude = 8.0f;              // Amplitudinea pentru sarit
    [Range(1.0f, 10.0f)]
    public float jumpFrequency = 2.0f;              // Frecventa pentru sarit

    [Header("Climbing Camera Noise")]
    public bool canClimb = false;                   // Indicator pentru permisiunea de a catara
    [HideInInspector]
    public bool isClimbing = false;                 // Indicator pentru starea de catarare
    [Range(1.0f, 10.0f)]
    public float climbAmplitude = 6.0f;             // Amplitudinea pentru catarare
    [Range(1.0f, 10.0f)]
    public float climbFrequency = 1.5f;             // Frecventa pentru catarare

    [Header("Noise Change Speed")]
    public float changeSpeedAmplitude = 0.4f;       // Viteza de schimbare a amplitudinii zgomotului
    public float changeSpeedFrequency = 0.8f;       // Viteza de schimbare a frecventei zgomotului

    [Header("Noise Current Speed")]
    private float _amplitude;                       // Amplitudinea curenta a zgomotului
    private float _frequency;                       // Frecventa curentă a zgomotului

    [Header("Noise Setting")]
    private float _amplitudeGain;                   // Amplitudinea de schimbare a zgomotului
    private float _frequencyGain;                   // Frecventa de schimbare a zgomotului

    [Header("Input")]
    private bool _vertical = false;                 // Input-ul vertical
    private bool _horizontal = false;               // Input-ul orizontal
    private bool _verticalBlock = false;            // Indicator pentru blocarea input-ului vertical
    private bool _horizontalBlock = false;          // Indicator pentru blocarea input-ului orizontal

    private void Update()
    {
        // Verificarea input-ului
        InputSystem();

        // Amplificarea zgomotului
        AmplifyCameraNoise();
    }

    private void InputSystem()
    {
        // Verificarea dacă player-ul poate merge
        if (canWalk)
        {
            isWalkingForward = Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0;
            isWalkingBackward = Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < 0;

            isWalkingLeftward = Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0;
            isWalkingRightward = Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0;

            _vertical = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
            _verticalBlock = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S);

            _horizontal = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
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
            isCrouching = Input.GetKey(KeyCode.LeftControl) && isWalkingForward && !isRunning && !isJumping && !isClimbing;
        }

        // Verificarea daca player-ul poate sări
        if (canJump)
        {
            isJumping = Input.GetKey(KeyCode.Space) && !isCrouching && !isClimbing;
        }
    }
    private void AmplifyCameraNoise()
    {
        if (isWalkingForward || isWalkingBackward || isWalkingLeftward || isWalkingRightward)
        {
            // Gain-ul amplitudinii si frecventei
            _amplitudeGain = walkingAmplitude;
            _frequencyGain = walkingFrequency;

            if (_verticalBlock || _horizontalBlock)
            {
                _amplitudeGain = idleAmplitude;
                _frequencyGain = idleFrequency;
            }

            if (_verticalBlock && _horizontal || _horizontalBlock && _vertical)
            {
                _amplitudeGain = walkingAmplitude;
                _frequencyGain = walkingFrequency;
            }

            if (_verticalBlock && _vertical && _horizontalBlock && _horizontal)
            {
                _amplitudeGain = idleAmplitude;
                _frequencyGain = idleFrequency;
            }
        }
        else
        {
            _amplitudeGain = idleAmplitude;
            _frequencyGain = idleFrequency;
        }

        if (isRunning)
        {
            _amplitudeGain = runningAmplitude;
            _frequencyGain = runningFrequency;
        }

        if (isCrouching)
        {
            _amplitudeGain = crouchAmplitude;
            _frequencyGain = crouchFrequency;

            if (_verticalBlock || _horizontalBlock)
            {
                _amplitudeGain = idleAmplitude;
                _frequencyGain = idleFrequency;
            }

            if (_verticalBlock && _horizontal || _horizontalBlock && _vertical)
            {
                _amplitudeGain = crouchAmplitude;
                _frequencyGain = crouchFrequency;
            }

            if (_verticalBlock && _vertical && _horizontalBlock && _horizontal)
            {
                _amplitudeGain = idleAmplitude;
                _frequencyGain = idleFrequency;
            }
        }

        if (isJumping)
        {
            _amplitudeGain = jumpAmplitude;
            _frequencyGain = jumpFrequency;
        }

        if (isClimbing)
        {
            _amplitudeGain = climbAmplitude;
            _frequencyGain = climbFrequency;
        }

        // Amplificarea a gain-ului pentru amplitudine
        _amplitude = Mathf.MoveTowards(virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, _amplitudeGain, Time.deltaTime / changeSpeedAmplitude);
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _amplitude;

        // Amplificarea a gain-ului pentru frecventă
        _frequency = Mathf.MoveTowards(virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain, _frequencyGain, Time.deltaTime / changeSpeedFrequency);
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = _frequency;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificarea daca player-ul a intrat in coliziune cu scara
        if (other.CompareTag("Ladder") && canClimb)
        {
            isClimbing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Verificarea dacă player-ul a iesit din coliziune cu scara
        if (other.CompareTag("Ladder") && canClimb)
        {
            isClimbing = false;
        }
    }
}
