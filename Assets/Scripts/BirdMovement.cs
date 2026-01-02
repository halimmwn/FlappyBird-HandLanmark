using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    [Header("Pergerakan")]
    public GameObject birdObject;
    public float smoothing = 15f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Animasi Sprite")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] birdSprites;
    public float animationSpeed = 0.15f;

    private int currentFrame;
    private float animationTimer;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Harus Kinematic agar bisa mengikuti mouse tapi tetap mendeteksi tabrakan
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0;
        }

        if (birdObject == null) birdObject = gameObject;
    }

    void Update()
    {
        if (birdObject == null) return;

        Vector3 mouseInput = Input.mousePosition;
        mouseInput.z = -Camera.main.transform.position.z;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseInput);

        float clampedY = Mathf.Clamp(mousePos.y, minY, maxY);

        Vector3 targetPos = new Vector3(birdObject.transform.position.x, clampedY, 0);
        birdObject.transform.position = Vector3.Lerp(birdObject.transform.position, targetPos, Time.deltaTime * smoothing);

        float tilt = (clampedY - birdObject.transform.position.y) * 10f;
        birdObject.transform.rotation = Quaternion.Euler(0, 0, tilt);

        HandleAnimation();
    }

    void HandleAnimation()
    {
        if (birdSprites == null || birdSprites.Length == 0 || spriteRenderer == null) return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % birdSprites.Length;
            spriteRenderer.sprite = birdSprites[currentFrame];
        }
    }

    // --- BAGIAN BARU: DETEKSI TABRAKAN ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mendeteksi jika menabrak pipa atau tanah
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
        // Mendeteksi jika melewati celah pipa (Zona Skor)
        else if (other.CompareTag("Scoring"))
        {
            GameManager.Instance.IncreaseScore();
        }
    }
}