using UnityEngine;
using UnityEngine.UI;

public class ShowZone : MonoBehaviour
{
    // colors[0] = cyan
    // colors[1] = orange
    // colors[2] = green
    [SerializeField] private Color[] colors;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Image imageFill;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject pandoraBox;


    #region PandoraBox
    private Animator pandoraAnim;
    private Transform pandoraTransform;
    private GameObject boxNormal;
    private GameObject boxDestroyed;
    #endregion


    private LineRenderer lr;
    private float timer = 0f;
    private float captureDuration = 10f; // Pour 2 minutes
    private int segments = 64;
    private bool isCapturing = false;
    private bool isCaptured = false;

    void Awake()
    {
        pandoraAnim = pandoraBox.gameObject.GetComponent<Animator>();
        pandoraTransform = pandoraBox.transform;

        boxNormal = pandoraTransform.GetChild(1).gameObject;
        boxDestroyed = pandoraTransform.GetChild(2).gameObject;
        boxDestroyed.SetActive(false);
        
        backgroundImage.gameObject.SetActive(false);

        SphereCollider sphere = GetComponent<SphereCollider>();
        float radius = sphere.radius;

        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.loop = true;
        lr.positionCount = segments;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        if (lr.material == null)
            lr.material = new Material(Shader.Find("Sprites/Default"));

        Vector3 sphereWorldCenter = transform.TransformPoint(sphere.center);
        Vector3 bottomWorldPos = sphereWorldCenter;

        for (int i = 0; i < segments; i++)
        {
            float angle = ((float)i / segments) * 2 * Mathf.PI;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 pointWorld = bottomWorldPos + new Vector3(x, 0f, z);
            lr.SetPosition(i, pointWorld);
        }

        if (imageFill != null)
            imageFill.fillAmount = 0f;
    }

    void Update()
    {
        if (colors != null && colors.Length >= 3)
        {
            if (isCaptured)
                lr.material.color = colors[2];
            else if (isCapturing)
                lr.material.color = colors[1];
            else
                lr.material.color = colors[0];
        }

        if (isCapturing && !isCaptured)
        {
            backgroundImage.gameObject.SetActive(true);
            timer += Time.deltaTime;

            if (imageFill != null)
                imageFill.fillAmount = Mathf.Clamp01(timer / captureDuration);

            // zone capturée
            if (timer >= captureDuration)
            {
                isCaptured = true;
                isCapturing = false;
                timer = captureDuration;

                boxNormal.SetActive(false);
                Destroy(boxNormal);
                boxDestroyed.SetActive(true);
                pandoraAnim.SetTrigger("zoneCaptured");
            }
        }
        else if (!isCapturing && !isCaptured)
        {
            timer -= Time.deltaTime / 2f;
            // timer = Mathf.Max(timer, 0f);

            if (imageFill != null)
                imageFill.fillAmount = Mathf.Clamp01(timer / captureDuration);
            
            if (timer <= 0f)
                backgroundImage.gameObject.SetActive(false);
        }
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isCapturing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isCapturing = false;
        }
    }


    public void DestroyPandoraBox()
    {
        
        Destroy(pandoraBox);
    }
}
