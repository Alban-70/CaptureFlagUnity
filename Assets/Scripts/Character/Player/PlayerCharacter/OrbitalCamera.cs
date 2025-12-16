using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target; // Objet à suivre (le joueur)

    private float distance = 3f;
    private float xSpeed = 200f;
    private float ySpeed = 120f;

    private float yMin = -50f;
    private float yMax = 70f;

    private Vector3 targetOffset = new Vector3(0.3f, 5.1f, 0); // hauteur caméra

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
        Vector3 lateralOffset = target.right * 0.2f; // 0.2 = décalage vers la droite
        Vector3 position = target.position + targetOffset + lateralOffset - rotation * Vector3.forward * distance;


        transform.rotation = rotation;
        transform.position = position;
    }
}
