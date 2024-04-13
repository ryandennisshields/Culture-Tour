using com.cyborgAssets.inspectorButtonPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GCU.CultureTour
{
    public class StatusMessageDisplay : MonoBehaviour
    {
        [SerializeField]
        TMPro.TextMeshProUGUI StatusMessage;

        [SerializeField]
        CanvasGroup MessageHolder;

        [SerializeField, Min(0.5f), Tooltip("In seconds.")]
        float _messageDuration = 10f;
        public float MessageDuration => _messageDuration;
        
        [SerializeField, Min(0f), Tooltip("In seconds.")]
        float _fadeDuration = 1.5f;
        public float FadeDuration => _fadeDuration;

        public bool IsVisible { get; private set; } = false;
        public bool HasMessages
        {
            get
            {
                if ( IsVisible )
                {
                    return true;
                }

                if ( _messageQueue.Any( ) )
                {
                    return true;
                }

                if ( ! string.IsNullOrEmpty ( CurrentlyDisplayedMessage ) )
                {
                    return true;
                }

                return false;
            }
        }
        public string CurrentlyDisplayedMessage => StatusMessage.text;

        private Queue<string> _messageQueue = new ();
        private float _currentMessageDisplayTime = 0f;

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

            _messageQueue.Enqueue(message);
        }

        public void ClearMessageQueue()
        {
            _messageQueue.Clear ();
            StatusMessage.text = string.Empty;

            // this forces the first Update after a new message is added to check for messages in the queue.
            _currentMessageDisplayTime = _messageDuration;
        }

        private void CheckConnection()
        {
                if (Application.internetReachability == NetworkReachability.NotReachable && !Input.location.isEnabledByUser)
                {
                    // No internet connection and location - display an error message
                    DisplayMessage("Please enable Internet and Location!", true);
                }
                else if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    // No internet connection - display an error message
                    DisplayMessage("Please enable Internet!", true);
                }
                else if (!Input.location.isEnabledByUser)
                {
                    // No location access - display an error message
                    DisplayMessage("Please enable Location!", true);
                }
                else 
                {
                    ClearMessageQueue();
                }
        }

        /// <summary>
        /// Will cause the next message in the queue to be displayed.
        /// If there are no new messages then the message system will be cleared.
        /// </summary>
        public void JumpToNextMessage()
        {
            _currentMessageDisplayTime = _messageDuration;
        }

        private void Start () 
        {
            StatusMessage.text = string.Empty;
            MessageHolder.alpha = 0f;
            
            // this forces the first Update after a new message is added to check for messages in the queue.
            _currentMessageDisplayTime = _messageDuration;
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Map")
            {
                CheckConnection();
            }

            if (!IsVisible && _messageQueue.Count == 0)
            {
                return;
            }

            _currentMessageDisplayTime += Time.deltaTime;

            if (_currentMessageDisplayTime >= _messageDuration)
            {
                if (_messageQueue.Count > 0)
                {
                    // Display next message
                    var newMessage = _messageQueue.Dequeue();
                    SetMessage(newMessage);
                }
                else
                {
                    // Fade out
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
            _currentMessageDisplayTime = 0;

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

            StatusMessage.text = string.Empty;
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
