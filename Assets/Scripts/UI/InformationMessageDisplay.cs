using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GCU.CultureTour
{
    public class InformationMessageDisplay : MonoBehaviour
    {
        [SerializeField]
        CanvasGroup MessageHolder;

        [SerializeField]
        Button informationButton;
        
        [SerializeField, Min(0f), Tooltip("In seconds.")]
        float _fadeDuration = 1f;
        public float FadeDuration => _fadeDuration;

        private void Start () 
        {
            MessageHolder.alpha = 0f;

            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Map" && !PlayerPrefs.HasKey("MapFirst"))
            {
                informationButton.onClick.Invoke();
                PlayerPrefs.SetInt("MapFirst", 1);
                PlayerPrefs.Save();
            }
            if (!PlayerPrefs.HasKey("VPSFirst") && sceneName != "Start" && sceneName != "Map")
            {
                informationButton.onClick.Invoke();
                PlayerPrefs.SetInt("VPSFirst", 1);
                PlayerPrefs.Save();
            }
            if (sceneName == "Start")
            {
                MessageHolder.alpha = 1f;
            }
        }

        public void ShowMessage()
        {
            MessageHolder.blocksRaycasts = true;
            StartCoroutine(FadeIn());
        }

        public void HideMessage()
        {
            StartCoroutine(FadeOut());
            MessageHolder.blocksRaycasts = false;
        }

        #region Fading

        private IEnumerator FadeIn()
        {
            var r = Fade(1f);
            while (r.MoveNext())
            {
                yield return r;
            }
        }

        private IEnumerator FadeOut()
        {
            var r = Fade(0f);
            while (r.MoveNext())
            {
                yield return r;
            }
        }

        private IEnumerator Fade(float target)
        {
            float start = MessageHolder.alpha;
            float progress = 0f;
            float timeSinceStart = 0f;

            while (progress < 1f)
            {
                timeSinceStart += Time.deltaTime;
                progress = timeSinceStart / _fadeDuration;
                float amount = Mathf.Lerp(start, target, progress);
                MessageHolder.alpha = amount;

                // Wait for next frame
                yield return null;
            }

            MessageHolder.alpha = target;
        }

        #endregion

    }
}
