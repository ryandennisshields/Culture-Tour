using Niantic.Lightship.AR.LocationAR;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GCU.CultureTour.VPS
{
    public class VPSStatusIndicator : MonoBehaviour
    {
        enum Status
        {
            IDLE,   // used after stopped to allow for new tracking status notifications to be displayed
            STOPPED,// system is no longer attempting to track.
            GOOD,   // system is tracking
            BAD,    // system is attempting to tracking 
        }

        [Header("UI Border Indicator")]
        [SerializeField]
        Image StatusIndicatorImage;

        [Header("Colours")]
        [SerializeField]
        Color GoodColourAlert  = Color.green;

        [SerializeField]
        Color GoodColourSubtle = Color.green;

        [SerializeField]
        Color BadColourAlert   = Color.red;

        [SerializeField]
        Color BadColourSubtle  = Color.red;

        [Header("Timings")]
        [SerializeField]
        [Range(0f, 30f)]
        float FadeToAlertTime = 0f;

        [SerializeField]
        [Range(0f, 30f)]
        float DwellTime = 2f;

        [SerializeField]
        [Range(0f, 30f)]
        float FadeToSubtleTime = 10f;

        [SerializeField]
        [Range(0f, 30f)]
        float FadeToStoppedTime = 10f;

        [Header("UI Icon Indicator")]
        [SerializeField]
        Image StatusIndicatorIcon;

        [SerializeField]
        Sprite GoodIcon;
        
        [SerializeField]
        Sprite BadIcon;

        Status currentStatus;
        ARLocationManager locationManger;

        private void Awake()
        {
            locationManger = FindObjectOfType<ARLocationManager>();

            if ( locationManger == null ) 
            {
                Debug.LogWarning("Could not find AR Location Manager", gameObject);
                Destroy( this );
            }
        }

        private void OnEnable()
        {
            locationManger.arPersistentAnchorStateChanged += ARPersistentAnchorStateChanged;

            // get the current state of tracking. If an AR location is enabled then tracking was happening.
            // this is a bit of a bodge as these objects aren't automatically disabled when tracking stops.
            foreach (ARLocation location in locationManger.ARLocations)
            {
                if ( location.gameObject.activeInHierarchy )
                {
                    SetStatus(Status.GOOD);
                    return;
                }
            }
            
            SetStatus(Status.BAD);
        }

        private void OnDisable()
        {
            locationManger.arPersistentAnchorStateChanged -= ARPersistentAnchorStateChanged;
        }

        public void TrackingStopped()
        {
            SetStatus(Status.STOPPED);
        }
        
        /// <summary>
        /// Triggered when the AR Location manager updates the status for it's tracked entity.
        /// Currently the AR Location manager can only track one entity at a time,
        /// Though this may change in later releases of ARDK.
        /// </summary>
        /// <param name="args"></param>
        private void ARPersistentAnchorStateChanged(Niantic.Lightship.AR.PersistentAnchors.ARPersistentAnchorStateChangedEventArgs args)
        {
            //Debug.Log($"AR Persistent Anchor Status Change.\tAnchor:{args.arPersistentAnchor.name}\t{UnityEngine.XR.ARSubsystems.TrackingState.Tracking}.");

            var status = args.arPersistentAnchor.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking ? Status.GOOD : Status.BAD;

            if (currentStatus == status)
            {
                // status hasn't changed just updated.
                return;
            }

            SetStatus(status);
        }

        private void SetStatus ( Status status )
        {
            if ( currentStatus == Status.STOPPED && status != Status.IDLE )
            {
                // After being stopped status must be set back to IDLE before 
                // GOOD or BAD can be set.
                return;
            }

            currentStatus = status;

            if ( currentStatus == Status.IDLE )
            {
                // nothing to do whilst idling.
                return;
            }

            if ( StatusIndicatorImage != null )
            {
                StopAllCoroutines();
                if (status == Status.STOPPED)
                {
                    StartCoroutine(StopStatusOnImage());
                }
                else
                {
                    StartCoroutine(SetStatusOnImage(status));
                }
            }

            if ( StatusIndicatorIcon != null )
            {
                StatusIndicatorIcon.sprite = currentStatus == Status.GOOD ? GoodIcon : BadIcon;
            }
        }

        /// <summary>
        /// Used when status is GOOD or BAD.
        /// The specified alert status colour will be faded in using the FadeToAlert time.
        /// The dwell delay will then be executed.
        /// Then the indicator will fade to the subtle status colour over the FadeToSubtle time.
        /// </summary>
        /// <param name="status">Must be GOOD or BAD.</param>
        /// <returns>IEnumerator for Unity Coroutines</returns>
        private IEnumerator SetStatusOnImage (Status status)
        {
            Color startingColour = StatusIndicatorImage.color;
            Color alertColour  = status == Status.GOOD ? GoodColourAlert : BadColourAlert;
            Color subtleColour = status == Status.GOOD ? GoodColourSubtle : BadColourSubtle;

            float t;
            float runningTime;

            // fade to alert
            if ( FadeToAlertTime >  0 )
            {
                runningTime = 0f;
                t = 0f;

                while ( t < 1 )
                {
                    runningTime += Time.deltaTime; 
                    t = runningTime / FadeToAlertTime;

                    StatusIndicatorImage.color = ColorHelpers.LerpColor(startingColour, alertColour, t);

                    yield return null;
                }
            }

            StatusIndicatorImage.color = alertColour;
            yield return null;

            // dwell
            yield return new WaitForSeconds( DwellTime );

            // fade to subtle
            if ( FadeToSubtleTime > 0 )
            {
                runningTime = 0f;
                t = 0f;

                while (t < 1)
                {
                    runningTime += Time.deltaTime;
                    t = runningTime / FadeToSubtleTime;

                    StatusIndicatorImage.color = ColorHelpers.LerpColor(alertColour, subtleColour, t);

                    yield return null;
                }
            }

            StatusIndicatorImage.color = subtleColour;
        }

        /// <summary>
        /// Used when status is set to STOPPED.
        /// This will hide the status indicator in the time specified by FadeToStoppedTime.
        /// </summary>
        /// <returns>IEnumerator for Unity Coroutines</returns>
        private IEnumerator StopStatusOnImage()
        {
            Color startingColour = StatusIndicatorImage.color;
            Color hideColour = startingColour;
            hideColour.a = 0;

            float t;
            float runningTime;

            // fade to stopped
            if (FadeToStoppedTime > 0)
            {
                runningTime = 0f;
                t = 0f;

                while (t < 1)
                {
                    runningTime += Time.deltaTime;
                    t = runningTime / FadeToStoppedTime;

                    StatusIndicatorImage.color = ColorHelpers.LerpColor(startingColour, hideColour, t);

                    yield return null;
                }
            }

            StatusIndicatorImage.color = hideColour;
        }

    }
}
