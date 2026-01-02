using UnityEngine;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;

public class BirdHandController : MonoBehaviour
{
    [Header("MediaPipe Setup")]
    public HandLandmarkerRunner handLandmarkerRunner;
    public GameObject bird;

    [Header("Settings")]
    public float movementRange = 10f;
    [Range(1f, 20f)] public float smoothingSpeed = 10f;
    public MonoBehaviour mouseControlScript;

    private bool _isGameRunning = false;

    void Update()
    {
        // 1. Pastikan runner dan bird tidak null
        if (handLandmarkerRunner == null || bird == null) return;

        var result = handLandmarkerRunner.LatestResult;

        // 2. Pengecekan ekstra aman untuk data MediaPipe
        if (result.handLandmarks != null && result.handLandmarks.Count > 0)
        {
            var hand = result.handLandmarks[0];

            // Pastikan list landmarks di dalam hand tidak null dan isinya cukup (MediaPipe butuh 21 titik)
            if (hand.landmarks != null && hand.landmarks.Count >= 21)
            {
                if (!_isGameRunning)
                {
                    if (CheckIfFist(hand))
                    {
                        StartGameAction();
                    }
                }
                else
                {
                    // Gunakan landmark indeks 9 (Pangkal jari tengah) sebagai titik pusat kontrol
                    float handY = hand.landmarks[9].y;

                    // Konversi koordinat 0-1 MediaPipe ke World Space Unity
                    // 1-handY karena koordinat Y MediaPipe terbalik (0 di atas, 1 di bawah)
                    float targetY = (1 - handY) * movementRange - (movementRange / 2);

                    Vector3 currentPos = bird.transform.position;
                    float smoothedY = Mathf.Lerp(currentPos.y, targetY, Time.deltaTime * smoothingSpeed);

                    bird.transform.position = new Vector3(currentPos.x, smoothedY, currentPos.z);
                }
            }
        }
    }

    bool CheckIfFist(NormalizedLandmarks hand)
    {
        // Cek apakah data landmark lengkap sebelum akses indeks
        if (hand.landmarks == null || hand.landmarks.Count < 21) return false;

        // Logika kepalan tangan: ujung jari (8, 12) berada di bawah pangkalnya (5, 9)
        // Catatan: Di MediaPipe, angka Y makin besar berarti makin ke BAWAH
        return hand.landmarks[8].y > hand.landmarks[5].y &&
               hand.landmarks[12].y > hand.landmarks[9].y;
    }

    void StartGameAction()
    {
        _isGameRunning = true;
        Debug.Log("Game Dimulai dengan Tangan!");

        if (mouseControlScript != null)
        {
            mouseControlScript.enabled = false;
        }

        Rigidbody2D rb = bird.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            // Memberikan sedikit dorongan awal agar tidak langsung jatuh
            rb.velocity = Vector2.up * 2f;
        }
    }
}