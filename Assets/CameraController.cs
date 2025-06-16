using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Referência à empilhadeira

    [Header("Settings")]
    public float distance = 10f;
    public float mouseSensitivity = 2f;
    public float heightOffset = 3f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;

        Cursor.lockState = CursorLockMode.Locked; // Esconde e trava o cursor no centro
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Movimento do mouse
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, -20f, 60f); // Limita a rotação vertical

        // Calcula a nova posição e rotação da câmera
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position + Vector3.up * heightOffset;

        transform.rotation = rotation;
        transform.position = position;
    }
}
