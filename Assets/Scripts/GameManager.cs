using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Dibutuhkan untuk Coroutine (Animasi)

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private BirdMovement player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text scoreText; // Skor saat main
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;

    [Header("New UI Elements")]
    [SerializeField] private GameObject gameTitle;
    [SerializeField] private GameObject getReadyText;
    [SerializeField] private Text finalScoreText; // Text untuk skor akhir di layar Game Over

    public int score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // Munculkan Judul & Get Ready hanya saat pertama kali buka game
        gameTitle.SetActive(true);
        getReadyText.SetActive(true);
        finalScoreText.gameObject.SetActive(false); // Sembunyikan skor akhir dulu

        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);

        // Sembunyikan semua UI pembantu saat mulai main
        gameTitle.SetActive(false);
        getReadyText.SetActive(false);
        finalScoreText.gameObject.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void GameOver()
    {
        playButton.SetActive(true);
        gameOver.SetActive(true);

        // Tampilkan skor akhir dan mulai animasi count
        finalScoreText.gameObject.SetActive(true);
        StartCoroutine(CountScoreAnim());

        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    // --- FITUR ANIMASI SKOR ---
    private IEnumerator CountScoreAnim()
    {
        int currentCount = 0;
        float duration = 0.5f; // Durasi animasi (detik)
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Gunakan unscaled karena Time.timeScale sedang 0
            currentCount = (int)Mathf.Lerp(0, score, elapsed / duration);
            finalScoreText.text = "Score: " + currentCount.ToString();
            yield return null;
        }

        finalScoreText.text = "Score: " + score.ToString(); // Pastikan angka terakhir pas
    }

    public void StartByHand()
    {
        if (Time.timeScale == 0f)
        {
            Play();
        }
    }
}