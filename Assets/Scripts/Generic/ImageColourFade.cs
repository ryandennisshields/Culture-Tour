using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GCU.CultureTour
{
    [RequireComponent(typeof(Image))]
    public class ImageColourFade : MonoBehaviour
    {
        [Header("Colours")]
        [SerializeField]
        Color _startColour = new Color(1,1,1,0);

        [SerializeField]
        Color _endColour = new Color(1, 1, 1, 1);

        [Header("Timings")]
        [SerializeField]
        [Range(0f, 30f)]
        float _fadeTime = 1f;

        public void StartFadeToEndColour()
        {
            StopAllCoroutines();
            StartCoroutine(StartFading(true));
        }

        public void StartFadeToStartColour()
        {
            StopAllCoroutines();
            StartCoroutine(StartFading(false));
        }

        private IEnumerator StartFading( bool goToEnd )
        {
            Image img = GetComponent<Image>();

            if ( img == null )
            {
                yield break;
            }

            var startingColour = img.color;
            var endColour = goToEnd ? _endColour : _startColour;

            float t;
            float runningTime;

            if (_fadeTime > 0f)
            {

                runningTime = 0f;
                t = 0f;

                while (t < 1)
                {
                    runningTime += Time.deltaTime;
                    t = runningTime / _fadeTime;

                    img.color = ColorHelpers.LerpColor(startingColour, endColour, t);

                    yield return null;
                }

            }

            img.color = endColour;
        }
    }
}
