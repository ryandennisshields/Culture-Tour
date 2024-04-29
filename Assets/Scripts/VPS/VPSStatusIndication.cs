using Niantic.Lightship.AR.LocationAR;
using UnityEngine;
using UnityEngine.UI;

namespace GCU.CultureTour.VPS
{
    public class VPSStatusIndication : MonoBehaviour
    {
        enum Status
        {
            IDLE,   // used after stopped to allow for new tracking status notifications to be displayed
            STOPPED,// system is no longer attempting to track.
            GOOD,   // system is tracking
            BAD,    // system is attempting to tracking 
        }

        [SerializeField]
        Image StatusIndicatorIcon;

        Status currentStatus;
        ARLocationManager locationManger;

        private void Awake()
        {
            locationManger = FindObjectOfType<ARLocationManager>();

            if (locationManger == null)
            {
                Debug.LogWarning("Could not find AR Location Manager", gameObject);
                Destroy(this);
            }
        }

        private void OnEnable()
        {
            if (locationManger != null)
            {
                locationManger.arPersistentAnchorStateChanged += ARPersistentAnchorStateChanged;

                // Get the current state of tracking. If an AR location is enabled then tracking was happening.
                // This is a bit of a bodge as these objects aren't automatically disabled when tracking stops.
                foreach (ARLocation location in locationManger.ARLocations)
                {
                    if (location.gameObject.activeInHierarchy)
                    {
                        SetStatus(Status.GOOD);
                        return;
                    }
                }

                SetStatus(Status.BAD);
            }
        }

        private void OnDisable()
        {
            if (locationManger.ARLocations != null)
            {
                locationManger.arPersistentAnchorStateChanged -= ARPersistentAnchorStateChanged;
            }
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
                // Status hasn't changed just updated
                return;
            }

            SetStatus(status);
        }

        private void SetStatus(Status status)
        {
            if (currentStatus == Status.STOPPED && status != Status.IDLE)
            {
                // After being stopped status must be set back to IDLE before 
                // GOOD or BAD can be set
                return;
            }

            currentStatus = status;

            if (currentStatus == Status.IDLE)
            {
                // Nothing to do whilst idling
                return;
            }

            if (StatusIndicatorIcon != null)
            {
                StatusIndicatorIcon.GetComponent<Animator>().SetBool("Loaded", currentStatus == Status.GOOD ? true : false);
            }
        }
    }
}
