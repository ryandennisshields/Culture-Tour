using Niantic.Lightship.AR.LocationAR;
using UnityEngine;
using UnityEngine.UI;

namespace GCU.CultureTour.VPS
{
    public class VPSStatusIndicator : MonoBehaviour
    {
        [SerializeField]
        Image StatusIndicator;

        [SerializeField]
        Color GoodColour = Color.green;
        
        [SerializeField]
        Color BadColour = Color.red;

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
                    StatusIndicator.color = GoodColour;
                    return;
                }
            }

            StatusIndicator.color = BadColour;
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
            StatusIndicator.color = args.arPersistentAnchor.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking ? GoodColour : BadColour;
        }

    }
}
