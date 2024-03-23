using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GCU.CultureTour
{
    public class ReadCamera : MonoBehaviour
    {
        private bool camAvailable;
        private WebCamTexture webCamTexture;
        private Texture defaultBackground;

        public RawImage background;
        public AspectRatioFitter fit;

        public TextMeshProUGUI output;
        [HideInInspector]
        public string path;

        private void Start()
        {
            defaultBackground = background.texture;
            WebCamDevice[] devices = WebCamTexture.devices;

            if (devices.Length == 0)
            {
                Debug.Log("No camera detected");
                camAvailable = false;
                return;
            }

            for (int i = 0; i < devices.Length; i++)
            {
                if (!devices[i].isFrontFacing)
                {
                    webCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                }
            }

            if (webCamTexture == null)
            {
                Debug.Log("Unable to find suitable camera");
                return;
            }

            webCamTexture.Play();
            background.texture = webCamTexture;

            camAvailable = true;
            // Start capturing images from the camera
            StartCoroutine(CaptureImages());
        }

        private void Update()
        {
            if (!camAvailable)
                return;

            float ratio = (float)webCamTexture.width / (float)webCamTexture.height;
            fit.aspectRatio = ratio;

            float scaleY = webCamTexture.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orient = -webCamTexture.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        }

        /// <summary>
        /// Written by Chat GPT 2024-03-23.
        /// </summary>

        IEnumerator CaptureImages()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                SaveImage(webCamTexture, $"image_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.jpg");

                yield return new WaitForSeconds(2f);
            }
        }

        public void SaveImage(WebCamTexture texture, string fileName)
        {
            Texture2D tex = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
            tex.SetPixels(texture.GetPixels());
            tex.Apply();

            byte[] bytes = tex.EncodeToJPG();
            Destroy(tex);

            path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllBytes(path, bytes);

            output.text = ($"Saved image: {path}");
        }
    }
}
