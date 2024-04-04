using com.cyborgAssets.inspectorButtonPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GCU.CultureTour
{
    public class InformationMessageDisplay : MonoBehaviour
    {
        [SerializeField]
        TMPro.TextMeshProUGUI InformationMessage;

        [SerializeField]
        CanvasGroup MessageHolder;

        [SerializeField]
        Button informationButton;
        
        [SerializeField, Min(0f), Tooltip("In seconds.")]
        float _fadeDuration = 1.5f;
        public float FadeDuration => _fadeDuration;
        
        [SerializeField]
        bool checkFirstTimeScene;

        private const string SceneKeyPrefix = "FirstTimeInScene_";

        private void Start () 
        {
            if (InformationMessage != null)
            {
                InformationMessage.text = string.Empty;
                MessageHolder.alpha = 0f;
            }

            string sceneName = SceneManager.GetActiveScene().name;

            if (!PlayerPrefs.HasKey(SceneKeyPrefix + sceneName) && checkFirstTimeScene)
            {
                informationButton.onClick.Invoke();
                Debug.Log("true");
                PlayerPrefs.SetInt(SceneKeyPrefix + sceneName, 1);
                PlayerPrefs.Save();
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

                // wait for next frame
                yield return null;
            }

            MessageHolder.alpha = target;
        }

        #endregion

    }
}
