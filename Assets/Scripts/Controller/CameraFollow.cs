using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the target (player) transform
    public float smoothing = 5f; // Smoothing factor for camera movement
    public float initialZoom = 10f; // Initial zoom level
    public float targetZoom = 5f; // Target zoom level
    public float zoomSpeed = 2f; // Speed of zooming in/out
    public float minZoom = 5f; // Minimum zoom level
    public float maxZoom = 15f; // Maximum zoom level
    public Button runButton; // Reference to the run button

    private Camera cam; // Reference to the Camera component
    private bool isFollowing = false; // Flag to check if the camera should follow the player
    private bool isDragging = false; // Flag to check if the camera is being dragged
    private Vector3 lastMousePosition; // Last mouse position
    private Vector3 lastTargetPosition; // Last position of the target
    private float timeSinceLastMove = 0f; // Time since the target last moved
    private float stopThreshold = 0.1f; // Threshold to consider the target as stopped (in seconds)

    void Start()
    {
        // Get the Camera component
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera component missing on this game object. Please attach a Camera component.");
            return;
        }

        // Add listener to the button
        runButton.onClick.AddListener(OnRunButtonClicked);

        // Set the initial zoom level
        cam.orthographicSize = initialZoom;

        if (target != null)
        {
            lastTargetPosition = target.position; // Initialize the last target position
        }
    }

    void LateUpdate()
    {
        if (isFollowing && target != null)
        {
            // Smoothly interpolate between the camera's current position and the target position
            Vector3 targetCamPos = target.position;
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetCamPos.x, targetCamPos.y, transform.position.z), smoothing * Time.deltaTime);
        }
    }

    void Update()
    {
        if (cam == null)
        {
            return;
        }

        // Handle zooming in and out
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float newZoom = Mathf.Clamp(cam.orthographicSize - scrollInput * zoomSpeed, minZoom, maxZoom);
            cam.orthographicSize = newZoom;
        }

        // Handle camera dragging
        if (!isFollowing)
        {
            HandleCameraDragging();
        }

        // Check if the target has stopped moving
        if (target != null)
        {
            if (target.position != lastTargetPosition)
            {
                lastTargetPosition = target.position;
                timeSinceLastMove = 0f;
            }
            else
            {
                timeSinceLastMove += Time.deltaTime;
                if (timeSinceLastMove > stopThreshold)
                {
                    isFollowing = false; // Stop following the player
                }
            }
        }
    }

    private IEnumerator ZoomInCoroutine(float targetZoom)
    {
        while (cam.orthographicSize > targetZoom)
        {
            cam.orthographicSize -= zoomSpeed * Time.deltaTime;
            if (cam.orthographicSize < targetZoom)
            {
                cam.orthographicSize = targetZoom;
            }
            yield return null;
        }
    }

    private void OnRunButtonClicked()
    {
        if (target == null)
        {
            Debug.LogError("Target is not set. Please set the target in the inspector.");
            return;
        }

        // Start the zoom in coroutine
        StartCoroutine(ZoomInCoroutine(targetZoom));

        // Start following the player
        isFollowing = true;
    }

    private void HandleCameraDragging()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            Vector3 moveDelta = new Vector3(-mouseDelta.x * cam.orthographicSize / cam.pixelHeight, -mouseDelta.y * cam.orthographicSize / cam.pixelHeight, 0);

            transform.position += moveDelta;
            lastMousePosition = Input.mousePosition;
        }
    }
}
