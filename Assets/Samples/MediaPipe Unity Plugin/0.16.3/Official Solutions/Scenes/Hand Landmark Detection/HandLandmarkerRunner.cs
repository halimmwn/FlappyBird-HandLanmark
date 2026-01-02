using System.Collections;
using Mediapipe.Tasks.Vision.HandLandmarker;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mediapipe.Unity.Sample.HandLandmarkDetection
{
    public class HandLandmarkerRunner : VisionTaskApiRunner<HandLandmarker>
    {
        [SerializeField] private HandLandmarkerResultAnnotationController _handLandmarkerResultAnnotationController;
        private Experimental.TextureFramePool _textureFramePool;
        public readonly HandLandmarkDetectionConfig config = new HandLandmarkDetectionConfig();

        // --- BARIS PENTING: Gunakan nama 'LatestResult' agar tidak tabrakan ---
        public HandLandmarkerResult LatestResult { get; private set; }

        public override void Stop()
        {
            base.Stop();
            _textureFramePool?.Dispose();
            _textureFramePool = null;
        }

        protected override IEnumerator Run()
        {
            yield return AssetLoader.PrepareAssetAsync(config.ModelPath);
            var options = config.GetHandLandmarkerOptions(config.RunningMode == Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnHandLandmarkDetectionOutput : null);
            taskApi = HandLandmarker.CreateFromOptions(options, GpuManager.GpuResources);
            var imageSource = ImageSourceProvider.ImageSource;
            yield return imageSource.Play();

            _textureFramePool = new Experimental.TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);
            screen.Initialize(imageSource);
            SetupAnnotationController(_handLandmarkerResultAnnotationController, imageSource);

            var transformationOptions = imageSource.GetTransformationOptions();
            var imageProcessingOptions = new Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: (int)transformationOptions.rotationAngle);

            AsyncGPUReadbackRequest req = default;
            var waitUntilReqDone = new WaitUntil(() => req.done);
            var waitForEndOfFrame = new WaitForEndOfFrame();

            // Gunakan nama lengkap agar tidak error Alloc
            var result = Mediapipe.Tasks.Vision.HandLandmarker.HandLandmarkerResult.Alloc(options.numHands);

            while (true)
            {
                if (isPaused) yield return new WaitWhile(() => isPaused);
                if (!_textureFramePool.TryGetTextureFrame(out var textureFrame)) { yield return waitForEndOfFrame; continue; }

                Image image;
                req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), transformationOptions.flipHorizontally, transformationOptions.flipVertically);
                yield return waitUntilReqDone;
                image = textureFrame.BuildCPUImage();
                textureFrame.Release();

                switch (taskApi.runningMode)
                {
                    case Tasks.Vision.Core.RunningMode.IMAGE:
                    case Tasks.Vision.Core.RunningMode.VIDEO:
                        if (taskApi.TryDetectForVideo(image, GetCurrentTimestampMillisec(), imageProcessingOptions, ref result))
                        {
                            LatestResult = result; // MENGISI DATA
                            _handLandmarkerResultAnnotationController.DrawNow(result);
                        }
                        break;
                    case Tasks.Vision.Core.RunningMode.LIVE_STREAM:
                        taskApi.DetectAsync(image, GetCurrentTimestampMillisec(), imageProcessingOptions);
                        break;
                }
            }
        }

        private void OnHandLandmarkDetectionOutput(HandLandmarkerResult result, Image image, long timestamp)
        {
            LatestResult = result; // MENGISI DATA LIVE STREAM
            _handLandmarkerResultAnnotationController.DrawLater(result);
        }
    }
}