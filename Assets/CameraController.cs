using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;            // The target for the camera to follow (the car)
    public float followSpeed = 0.125f;  // How smoothly the camera follows the target
    public Vector3 offset;              // Offset from the target to maintain the desired view
    public float baseZoom = 30f;        // Base camera zoom size
    public float maxZoomOut = 60f;      // Maximum zoom out size
    public float zoomSpeedFactor = 0.1f;// How much the camera zooms out based on the car's speed

    private Camera cam;                 // Reference to the camera component
    private Vector3 velocity = Vector3.zero; // Used for smooth damping

    void Start()
    {
        cam = Camera.main;              // Automatically assign the main camera

        if (cam != null)
        {
            cam.orthographicSize = baseZoom; // Set the initial camera zoom
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            FollowTarget();
            AdjustZoom();
        }
    }

    void FollowTarget()
    {
        // Target position with offset, maintaining the original Z position
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);
    }

    void AdjustZoom()
    {
        CarController carController = target.GetComponent<CarController>();

        if (carController != null && cam != null)
        {
            // Calculate the zoom level based on the car's speed
            float targetZoom = baseZoom + (Mathf.Abs(carController.currentSpeed) * zoomSpeedFactor);
            targetZoom = Mathf.Clamp(targetZoom, baseZoom, maxZoomOut);

            // Smoothly adjust the camera's zoom level (orthographic size)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * followSpeed);
        }
    }
}
