using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target; // Objet à suivre (le joueur)

    public float distance = 3f;
    public float xSpeed = 200f;
    public float ySpeed = 120f;

    public float yMin = -50f;
    public float yMax = 60f;

    public Vector3 targetOffset = new Vector3(0, 1.7f, 0); // hauteur caméra

    private float x; // horizontal camera angle
    private float y; // vertical camera angle

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (!target) return;

        // --------- 1. Récupérer le mouvement souris -----------
        float mouseX = Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

        // --------- 2. Rotation caméra horizontale -----------
        x += mouseX;

        // --------- 3. Rotation verticale caméra -----------
        y -= mouseY;
        y = Mathf.Clamp(y, yMin, yMax);

        // --------- 4. Faire tourner uniquement le joueur sur Y -----------
        target.rotation = Quaternion.Euler(0f, x, 0f);

        // --------- 5. Calculer la rotation de la caméra -----------
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // --------- 6. Placer la caméra autour du joueur -----------
        Vector3 position = target.position + targetOffset - rotation * Vector3.forward * distance;

        transform.rotation = rotation;
        transform.position = position;
    }
}
