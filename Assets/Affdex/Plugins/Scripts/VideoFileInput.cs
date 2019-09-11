﻿// Unity derives Video File Input Component UI from this file
using UnityEngine;
using System.Collections;
using UnityEngine.Video;

namespace Affdex
{


    /// <summary>
    /// Samples a movie texture for faces.
    /// </summary>
    [RequireComponent(typeof(Detector))]
    public class VideoFileInput : MonoBehaviour, IDetectorInput
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_XBOXONE
        /// <summary>
        /// Texture to sample
        /// </summary>
        public VideoPlayer movie;
#endif
        /// <summary>
        /// Number of samples per second
        /// </summary>
        public float sampleRate = 20;

        private Texture2D t2d;

        /// <summary>
        /// Core detector object.  Handles all communication with the native APIs.  
        /// </summary>
        public Detector detector
        {
            get; private set;
        }

        /// <summary>
        /// Allows access to texture's from a movie file
        /// </summary>
        public Texture Texture
        {
            get
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_XBOXONE
                return movie.targetTexture;
#else
                return new Texture();
#endif
            }
        }

        void Start()
        {
            movie.Play();
            movie.isLooping = true;
            t2d = new Texture2D(movie.targetTexture.width, movie.targetTexture.height, TextureFormat.RGB24, false);
            detector = GetComponent<Detector>();
        }

        private void Update()
        {
            if (Input.GetKeyDown("space") && movie.isPlaying)
                movie.Pause();
            else if (Input.GetKeyDown("space") && !movie.isPlaying)
                movie.Play();

        }

        void OnEnable()
        {
            if (!AffdexUnityUtils.ValidPlatform())
                return;

            if (sampleRate > 0)
                StartCoroutine(SampleRoutine());
        }

        private IEnumerator SampleRoutine()
        {
            Debug.Log("startign sample coroutine");
            if (enabled) Debug.Log("enabled");

            while (enabled)
            {
                
                yield return new WaitForSeconds(1 / sampleRate);
                if (detector.IsRunning)
                {
                    ProcessFrame();
                }

            }
        }


        private void ProcessFrame()
        {
            Debug.Log("Process Frame");
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_XBOXONE
            if (this.movie != null)
            {

                //A render texture is required to copy the pixels from the movie clip
                RenderTexture rt = RenderTexture.GetTemporary(movie.targetTexture.width, movie.targetTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
                
                RenderTexture.active = rt;
                
                //Copy the movie texture to the render texture
                Graphics.Blit(movie.targetTexture, rt);

                //Read the render texture to our temporary texture
                t2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

                //apply the bytes
                t2d.Apply();

                //Send to the detector
                Frame frame = new Frame(t2d.GetPixels32(), t2d.width, t2d.height, Frame.Orientation.Upright, Time.realtimeSinceStartup);
                detector.ProcessFrame(frame);

                RenderTexture.ReleaseTemporary(rt);
            }
#endif
        }
    }
}
