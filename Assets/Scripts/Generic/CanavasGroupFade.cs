using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GCU.CultureTour
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFade : MonoBehaviour
    {
        [Header("Alpha")]
        [SerializeField]
        [Range(0f, 1f)]
        float _start = 1f;

        [SerializeField]
        [Range(0f, 1f)]
        float _end = 0f;

        [Header("Timings")]
        [SerializeField]
        [Range(0f, 30f)]
        float _fadeTime = 1f;

        [SerializeField]
        [Range(0f, 10f)]
        float _delay = 0f;

        public void StartFadeToEnd()
        {
            StopAllCoroutines();
            StartCoroutine(StartFading(true));
        }

        public void StartFadeToStart()
        {
            StopAllCoroutines();
            StartCoroutine(StartFading(false));
        }

        private IEnumerator StartFading( bool goToEnd )
        {
            var cg = GetComponent<CanvasGroup>();

            if ( cg == null )
            {
                yield break;
            }

            var startingValue = cg.alpha;
            var endValue = goToEnd ? _end : _start;

            float t;
            float runningTime;

            yield return new WaitForSeconds(_delay);

            if (_fadeTime > 0f)
            {

                runningTime = 0f;
                t = 0f;

                while (t < 1)
                {
                    runningTime += Time.deltaTime;
                    t = runningTime / _fadeTime;

                    cg.alpha = Mathf.Lerp(startingValue, endValue, t);

                    yield return null;
                }

            }

            cg.alpha = endValue;
        }
    }
}
