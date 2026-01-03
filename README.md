📌 Deskripsi Project

Project ini merupakan pengembangan game 2D **Flappy Bird** yang dikombinasikan dengan teknologi **hand landmark detection** menggunakan **MediaPipe**. Berbeda dengan versi klasik yang menggunakan keyboard atau mouse, pada project ini **pergerakan burung dikontrol secara langsung oleh gerakan tangan pemain** melalui webcam.

Gerakan tangan ke atas dan ke bawah akan diikuti oleh burung secara real-time, sehingga menciptakan pengalaman bermain yang lebih interaktif, intuitif, dan nirkontak (*touchless control*).

---

🎮 Fitur Utama

* Kontrol burung menggunakan **gerakan tangan (hand tracking)**
* Deteksi **hand landmark** secara real-time menggunakan MediaPipe
* Gameplay Flappy Bird 2D berbasis Unity
* Sistem rintangan (pipa) dengan posisi acak
* Sistem skor dan *game over*
* Kontrol yang responsif dan halus

--

## 🛠️ Teknologi yang Digunakan

* **Unity Engine (2D)**
* **C#**
* **MediaPipe Unity Plugin**
* **Webcam** sebagai input kamera
* **Computer Vision & Hand Landmark Detection**

---

## ⚙️ Cara Kerja Sistem

1. Webcam menangkap pergerakan tangan pemain secara real-time.
2. MediaPipe memproses input kamera dan mendeteksi **21 titik landmark tangan**.
3. Data koordinat landmark (terutama sumbu Y) dikirim ke Unity melalui skrip C#.
4. Posisi tangan dipetakan ke pergerakan burung pada sumbu vertikal.
5. ketika game dimulai kepalkan tangan agar men trigger kamera nya
6. Burung akan bergerak naik atau turun mengikuti gerakan tangan pemain.

---

## 🧠 Mekanisme Kontrol

* Gerakan tangan **ke atas** → burung bergerak naik
* Gerakan tangan **ke bawah** → burung bergerak turun


## ▶️ Cara Menjalankan Project

1. Clone repository ini:

   ```bash
   git clone https://github.com/username/flappybird-handtracking.git
   ```
2. Buka project menggunakan **Unity Hub**
3. Pastikan webcam terdeteksi dengan baik
4. Jalankan scene utama (*Play*)
5. Gerakkan tangan di depan kamera untuk mengontrol burung

---

⚠️ Catatan

* Pastikan pencahayaan ruangan cukup agar deteksi tangan lebih akurat
* Jarak tangan ke kamera mempengaruhi sensitivitas kontrol
* Disarankan menggunakan kamera dengan resolusi yang baik

tofolio / magang**,
* atau menambahkan **GIF demo & screenshot section** 🔥
