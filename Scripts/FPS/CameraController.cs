using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Virtual Camera")]
    public CinemachineVirtualCamera virtualCamera;  // Referintă catre camera Cinemachine

    [Header("Player")]
    public Transform Player;                        // Referinta catre Transformul jucatorului

    [Header("Camera Settings")]
    [Range(0.0f, 10.0f)]
    public float sensitivity = 1.8f;                // Sensibilitatea miscării camerei
    [Range(0.0f, 100.0f)]
    public float smoothing = 50.0f;                 // Gradul de netezire al mișcării camerei

    [Header("Camera Angle")]
    [Range(0.0f, 90.0f)]
    public float maxVerticalAngle = 90.0f;          // Unghiul maxim vertical al camerei

    [Header("Default Camera FOV")]
    [Range(0.0f, 180.0f)]
    public float defaultFOV = 60.0f;                // Campul vizual implicit al camerei

    [Header("Running Camera FOV")]
    public bool canRun = false;                     // Indicator pentru permisiunea de a alerga
    [Range(0.0f, 180.0f)]
    public float runningFOV = 70.0f;                // Campul vizual pentru starea de alergare
    [Range(0.0f, 10.0f)]
    public float changeSpeedFOV = 3.6f;             // Viteza de schimbare a fov-ului pentru starea de alergare

    [Header("Mouse Settings")]
    private Vector2 _velocity;                      // Viteza actuala a miscării camerei
    private Vector2 _frameVelocity;                 // Viteza curenta a cadrelor

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        virtualCamera.m_Lens.FieldOfView = defaultFOV;
    }
    private void Update()
    {
        // Gestionarea controlului
        HandleCamera();

        HandleRunningFOV();
    }

    private void HandleCamera()
    {
        // Obtine miscării relative
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);

        // Interpolarea la viteza totala
        _frameVelocity = Vector2.Lerp(_frameVelocity, rawFrameVelocity, 1 / smoothing);
        _velocity += _frameVelocity;

        // Limitarea miscarii verticale si aplicarea rotatiei camerei si jucătorului
        _velocity.y = Mathf.Clamp(_velocity.y, -maxVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.AngleAxis(-_velocity.y, Vector3.right);
        Player.localRotation = Quaternion.AngleAxis(_velocity.x, Vector3.up);
    }
    private void HandleRunningFOV()
    {
        // Verificarea dacă player-ul poate alerga
        if (canRun)
        {
            bool _isWalkingForward = Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0;

            bool _verticalBlock = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S);

            bool _isRunning = Input.GetKey(KeyCode.LeftShift) && _isWalkingForward && !_verticalBlock && !Input.GetKey(KeyCode.LeftControl);

            // Modificarea treptata a valorii fov-ului pentru starea de alergare
            if (_isRunning)
            {
                virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, runningFOV, Time.deltaTime / changeSpeedFOV);
            }
            else
            {
                virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, defaultFOV, Time.deltaTime / changeSpeedFOV);
            }
        }
    }
}
