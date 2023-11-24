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
            GOOD,
            BAD,
        }

        [SerializeField]
        Image StatusIndicator;

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
            locationManger.locationTrackingStateChanged += LocationTrackingStateChanged;
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
            locationManger.locationTrackingStateChanged -= LocationTrackingStateChanged;
        }

        private void LocationTrackingStateChanged(Niantic.Lightship.AR.PersistentAnchors.ARLocationTrackedEventArgs args)
        {
            Debug.Log($"AR Location Manager Status Change.\tLocation:{args.ARLocation.name}\t{(args.Tracking ? "is tracking" : "is not tracking")}.");
        }
        
        private void ARPersistentAnchorStateChanged(Niantic.Lightship.AR.PersistentAnchors.ARPersistentAnchorStateChangedEventArgs args)
        {
            Debug.Log($"AR Persistent Anchor Status Change.\tAnchor:{args.arPersistentAnchor.name}\t{(args.arPersistentAnchor.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking ? "is tracking" : "is not tracking")}.");

            var status = args.arPersistentAnchor.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking ? Status.GOOD : Status.BAD;

            if (currentStatus == status)
            {
                // status hasen't changed just updated.
                return;
            }

            StopAllCoroutines( );
            StartCoroutine( SetStatus ( status ) );
        }

        private IEnumerator SetStatus (Status status)
        {
            currentStatus = status;

            Color startingColour = StatusIndicator.color;
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

                    StatusIndicator.color = LerpColor(startingColour, alertColour, t);

                    yield return null;
                }
            }

            StatusIndicator.color = alertColour;
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

                    StatusIndicator.color = LerpColor(alertColour, subtleColour, t);

                    yield return null;
                }
            }

            StatusIndicator.color = subtleColour;
        }

        private static Color LerpColor( Color a, Color b, float t)
        {
            Color c;

            c.r = Mathf.Lerp(a.r, b.r, t);
            c.g = Mathf.Lerp(a.g, b.g, t);
            c.b = Mathf.Lerp(a.b, b.b, t);
            c.a = Mathf.Lerp(a.a, b.a, t);

            return c;
        }


    }
}
