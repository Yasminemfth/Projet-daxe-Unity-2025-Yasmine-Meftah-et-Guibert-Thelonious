using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player Tracking")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 1, -10);
    public float followSmoothSpeed = 0.125f;

    [Header("Zoom Control")]
    public float idleZoom = 7f;
    public float activeZoom = 5f;
    public float zoomSmoothSpeed = 2f;
    public float idleTimeThreshold = 2f;

    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    private Vector3 lastPlayerPos;
    private float idleTimer = 0f;

    void Start()
    {
        cam = Camera.main;
        lastPlayerPos = player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // suivi smooth
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, followSmoothSpeed);

        //  Zoom/Dezoom Dynamique 
        if (Vector3.Distance(player.position, lastPlayerPos) < 0.01f)
            idleTimer += Time.deltaTime;
        else
            idleTimer = 0f;

        float targetZoom = (idleTimer >= idleTimeThreshold) ? idleZoom : activeZoom;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSmoothSpeed);

        lastPlayerPos = player.position;
    }
}