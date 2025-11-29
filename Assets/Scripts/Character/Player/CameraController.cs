using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speedRotation = 100f;
    private float cameraPitch;

    void Update()
    {
        float y = Input.GetAxis("Mouse X");
        float x = Input.GetAxis("Mouse Y");
        Vector3 rotateValue = new Vector3(x, y * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * speedRotation * Time.deltaTime, 0);
    }
}

