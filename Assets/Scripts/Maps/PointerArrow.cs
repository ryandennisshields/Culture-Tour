using GCU.CultureTour.Map;
using System.Linq;
using UnityEngine;

namespace GCU.CultureTour
{
    public class PointerArrow : MonoBehaviour
    {
        void Update()
        {
            MapMarkerLogic[] mapMarkers =  FindObjectsOfType<MapMarkerLogic>();
            GameObject closestMarker = null;
            float closestDistance = float.MaxValue;
            foreach (MapMarkerLogic mapMarker in mapMarkers)
            {
                float distance = Vector3.Distance(gameObject.transform.position, mapMarker.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMarker = mapMarker.gameObject;
                }
            }
            if (closestMarker != null)
            {
                transform.LookAt(closestMarker.transform.position);
            }
        }
    }
}
