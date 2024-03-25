using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Google.Protobuf.WellKnownTypes;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace GCU.CultureTour
{
    /// <summary>
    /// Written (mostly) by Chat GPT 2024-03-23.
    /// Ryan - To clarify, I say "mostly" due to not knowing much about APIs and using them in Unity. 
    /// So most code here is thanks to the help of Chat GPT's output and explaining how things are done.
    /// </summary>

    public class ImageDetection : MonoBehaviour
    {
        public TextMeshProUGUI output;
        public TextMeshProUGUI output1;

        private static string authToken = null;

        public ReadCamera cameraInfo;
        private string path;

        void Awake()
        {
            // Temp - Game Manager needs to be called somewhere to exist in a scene
            GameManager.Instance.enabled = true;
            GetKey(new string[] { });
        }

        public class ApiResponse
        {
            public string data { get; set; }
            public List<string> class_name { get; set; }
            public List<string> score { get; set; }
        }

        private async Task GetKey(string[] args)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://adsai.buas.ngrok.app/vis_api/auth/login");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent("glasgow@buas.nl"), "email");
                content.Add(new StringContent("#GlasgowVIS2023!"), "password");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<ApiResponse>(responseContent);

                authToken = responseObject.data;
                StartCoroutine(RunDetectImage());
            }
            catch (Exception ex)
            {
                Debug.Log($"An API key getter error occurred: {ex.Message}");
            }
        }

        // Used as to not overload the API or the game.
        IEnumerator RunDetectImage()
        {
            while (true)
            {
                path = cameraInfo.path;
                yield return DetectImage();

                yield return new WaitForSeconds(2f);
            }
        }

        private async Task DetectImage()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://adsai.buas.ngrok.app/vis_api/evaluate/evaluate");
                request.Headers.Add("Authorization", $"Bearer {authToken}");

                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(File.OpenRead($"{path}")), "file", $"{path}");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<List<ApiResponse>>(responseContent);

                output.text = responseObject[0].class_name[0];
                output1.text = responseObject[0].score[0];
            }
            catch (Exception ex)
            {
                Debug.Log($"A detect image error occurred: {ex.Message}");
            }
        }
    }
}
