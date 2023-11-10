using com.cyborgAssets.inspectorButtonPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCU.CultureTour
{
    public class StatusMessageDisplay : MonoBehaviour
    {
        [SerializeField]
        TMPro.TextMeshProUGUI StatusMessage;

        [SerializeField]
        CanvasGroup MessageHolder;

        [SerializeField, Min(0.5f), Tooltip("In seconds.")]
        float messageDuration = 10f;
        [SerializeField, Min(0f), Tooltip("In seconds.")]
        float fadeDuration = 1.5f;

        public bool IsVisible { get; private set; } = false;
        public string CurrentlyDisplayedMessage => StatusMessage.text;
        private float currentMessageDisplayTime = 0f;

        Queue<string> MessageQueue = new ();

        [ProButton]
        public void DisplayMessage ( string message, bool clearQueue = false )
        {
            if (clearQueue)
            {
                ClearMessageQueue();
            }
            
            if ( string.IsNullOrWhiteSpace (message ) )
            {
                return;
            }

            MessageQueue.Enqueue(message);
        }

        public void ClearMessageQueue()
        {
            MessageQueue.Clear ();
            StatusMessage.text = string.Empty;

            // this forces the first Update after a new message is added to check for messages in the queue.
            currentMessageDisplayTime = messageDuration;
        }

        /// <summary>
        /// Will cause the next message in the queue to be displayed.
        /// If there are no new messages then the message system will be cleared.
        /// </summary>
        public void JumpToNextMessage()
        {
            currentMessageDisplayTime = messageDuration;
        }

        private void Start()
        {
            ClearMessageQueue ();
            MessageHolder.alpha = 0f;
        }

        public void Update ()
        {
            if ( !IsVisible && MessageQueue.Count == 0 )
            {
                return;
            }

            currentMessageDisplayTime += Time.deltaTime;

            if (currentMessageDisplayTime >= messageDuration)
            {
                if ( MessageQueue.Count > 0 )
                {
                    // display next message
                    var newMessage = MessageQueue.Dequeue();
                    SetMessage(newMessage);
                }
                else
                {
                    // fade out
                    StartCoroutine(FadeOut());
                }
            }
        }

        /// <summary>
        /// Display a message start its related timers.
        /// If the holder is not displayed it will be made visible.
        /// </summary>
        /// <param name="message"></param>
        private void SetMessage(string message)
        {
            if (message == null)
            {
                message = string.Empty;
            }

            StatusMessage.text = message;
            currentMessageDisplayTime = 0;

            if (!IsVisible)
            {
                StartCoroutine(FadeIn());
            }
        }

        #region Fading

        private IEnumerator FadeIn()
        {
            var r = Fade(1f);
            while (r.MoveNext())
            {
                yield return r;
            }

            IsVisible = true;
        }

        private IEnumerator FadeOut()
        {
            IsVisible = false;
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
                progress = timeSinceStart / fadeDuration;
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
