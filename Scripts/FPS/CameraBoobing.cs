using UnityEngine;
using Cinemachine;

public class CameraBoobing : MonoBehaviour
{
    [Header("Virtual Camera")]
    public CinemachineVirtualCamera virtualCamera;  // Referinta la camera Cinemachine
    private Vector3 _originalPosition;              // Stocarea pozitiei originale a camerei virtuale

    [Header("Walking Camera Boobing")]
    public bool canWalk = false;                    // Indicator pentru permisiunea de a merge
    [HideInInspector]
    public bool isWalkingForward = false;           // Indicator pentru starea de mers înainte
    [HideInInspector]
    public bool isWalkingBackward = false;          // Indicator pentru starea de mers înapoi
    [HideInInspector]
    public bool isWalkingLeftward = false;          // Indicator pentru starea de mers la stânga
    [HideInInspector]
    public bool isWalkingRightward = false;         // Indicator pentru starea de mers la dreapta
    [Range(0.001f, 0.1f)]
    public float walkingAmplitude = 0.03f;          // Amplitudinea pentru starea de mers
    [Range(1f, 25)]
    public float walkingFrequency = 7.0f;           // Frecventa pentru starea de mers
    [Range(10f, 50)]
    public float walkingSmooth = 10.0f;             // Gradul de netezire pentru starea de mers

    [Header("Running Camera Boobing")]
    public bool canRun = false;                     // Indicator pentru permisiunea de a alerga
    [HideInInspector]
    public bool isRunning = false;                  // Indicator pentru starea de alergare
    [Range(0.001f, 0.1f)]
    public float runningAmplitude = 0.04f;          // Amplitudinea pentru starea de alergare
    [Range(1f, 25)]
    public float runningFrequency = 12.0f;          // Frecventa pentru starea de alergare
    [Range(10f, 50)]
    public float runningSmooth = 10.0f;             // Gradul de netezire pentru starea de alergare

    [Header("Crouching Camera Boobing")]
    public bool canCrouch = false;                  // Indicator pentru permisiunea de a se stoarce
    [HideInInspector]
    public bool isCrouching = false;                // Indicator pentru starea de stoarcere
    [Range(0.001f, 0.1f)]
    public float crouchingAmplitude = 0.04f;        // Amplitudinea pentru starea de stoarcere
    [Range(1f, 25)]
    public float crouchingFrequency = 7.0f;         // Frecventa pentru starea de stoarcere
    [Range(10f, 50)]
    public float crouchingSmooth = 10.0f;           // Gradul de netezire pentru starea de stoarcere

    [Header("Boobing Change Speed")]
    public float changeSpeedAmplitude = 0.1f;       // Viteza de schimbare a amplitudinii 
    public float changeSpeedFrequency = 0.0f;       // Viteza de schimbare a frecventei
    public float changeSpeedSmooth = 0.1f;          // Viteza de schimbare a gradului de netezire

    [Header("Boobing Current Speed")]
    private float _amplitude;                        // Amplitudinea curentă
    private float _frequency;                        // Frecventa curentă
    private float _smooth;                           // Gradul de netezire curent a

    [Header("Boobing Setting")]
    private float _amplitudeGain;                   // Amplitudinea de schimbare 
    private float _frequencyGain;                   // Frecventa de schimbare 
    private float _smoothGain;                      // Gradul de netezire de schimbare

    [Header("Input")]
    private bool _vertical = false;                 // Input-ul vertical
    private bool _horizontal = false;               // Input-ul orizontal
    private bool _verticalBlock = false;            // Indicator pentru blocarea input-ului vertical
    private bool _horizontalBlock = false;          // Indicator pentru blocarea input-ului orizontal

    private void Start()
    {
        // Salvarea pozitiei originale a camerei virtuale
        _originalPosition = virtualCamera.transform.localPosition;
    }
    private void Update()
    {
        // Verificarea input-ului player-ului si actualizarea starii acestuia
        InputSystem();

        // Aplicarea efectului "Boobing" daca player-ul se misca
        CheckForHeadbobTrigger();

        // Amplificarea impulsului "Boobing" in dependenta de starea player-ului
        AmplifyHeadBob();

        // Oprirea efectului "Boobing" daca player-ul nu se misca
        StopHeadBob();
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

            _vertical = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
            _verticalBlock = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S);

            _horizontal = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
            _horizontalBlock = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D);
        }

        if (canRun)
        {
            isRunning = Input.GetKey(KeyCode.LeftShift) && isWalkingForward && !isWalkingBackward && !isCrouching;
        }

        if (canCrouch)
        {
            isCrouching = Input.GetKey(KeyCode.LeftControl) && !isRunning;
        }
    }
    private void CheckForHeadbobTrigger()
    {
        // Verificarea daca player-ul se misca pentru a incepe efectul "Boobing"
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        // Aplicarea efectulului "Boobing" la mișcare
        if (inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }
    private void StartHeadBob()
    {
        // Calcularea efectului "Boobing" in functie de timp si parametrii specificati
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * _frequency) * _amplitude * 1.4f, Time.deltaTime * _smooth);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * _frequency / 2f) * _amplitude * 1.6f, Time.deltaTime * _smooth);

        // Aplicarea efectului "Boobing" la pozitia camerei virtuale
        virtualCamera.transform.localPosition += pos;
    }
    private void AmplifyHeadBob()
    {
        if (isWalkingForward || isWalkingBackward || isWalkingLeftward || isWalkingRightward)
        {
            _amplitudeGain = walkingAmplitude;
            _frequencyGain = walkingFrequency;
            _smoothGain = walkingSmooth;

            if (_verticalBlock || _horizontalBlock)
            {
                _amplitudeGain = 0.0f;
                _frequencyGain = 0.0f;
                _smoothGain = 0.0f;
            }

            if (_verticalBlock && _horizontal || _horizontalBlock && _vertical)
            {
                _amplitudeGain = walkingAmplitude;
                _frequencyGain = walkingFrequency;
                _smoothGain = walkingSmooth;
            }

            if (_verticalBlock && _vertical && _horizontalBlock && _horizontal)
            {
                _amplitudeGain = 0.0f;
                _frequencyGain = 0.0f;
                _smoothGain = 0.0f;
            }
        }

        if (isRunning)
        {
            _amplitudeGain = runningAmplitude;
            _frequencyGain = runningFrequency;
        }

        if (isCrouching)
        {
            _amplitudeGain = crouchingAmplitude;
            _frequencyGain = crouchingFrequency;
            _smoothGain = crouchingSmooth;

            if (_verticalBlock || _horizontalBlock)
            {
                _amplitudeGain = 0.0f;
                _frequencyGain = 0.0f;
                _smoothGain = 0.0f;
            }

            if (_verticalBlock && _horizontal || _horizontalBlock && _vertical)
            {
                _amplitudeGain = walkingAmplitude;
                _frequencyGain = walkingFrequency;
                _smoothGain = walkingSmooth;
            }

            if (_verticalBlock && _vertical && _horizontalBlock && _horizontal)
            {
                _amplitudeGain = 0.0f;
                _frequencyGain = 0.0f;
                _smoothGain = 0.0f;
            }
        }

        _amplitude = Mathf.MoveTowards(_amplitude, _amplitudeGain, Time.deltaTime / changeSpeedAmplitude);
        _frequency = Mathf.MoveTowards(_frequency, _frequencyGain, Time.deltaTime / changeSpeedFrequency);
        _smooth = Mathf.MoveTowards(_smooth, _smoothGain, Time.deltaTime / changeSpeedSmooth);
    }
    private void StopHeadBob()
    {
        // Restabilirea treptate a pozitiei originale a camerei
        virtualCamera.transform.localPosition = Vector3.Lerp(virtualCamera.transform.localPosition, _originalPosition, 1 * Time.deltaTime);
    }
}
