using GCU.CultureTour.Map;
using UnityEngine;

namespace GCU.CultureTour
{
    public class PointerArrow : MonoBehaviour
    {
        void Update()
        {
            MapMarkerLogic[] mapMarkers =  FindObjectsOfType<MapMarkerLogic>();
            MapMarkerLogic closestMarker = null;
            float closestDistance = float.MaxValue;
            foreach (MapMarkerLogic mapMarker in mapMarkers)
            {
                float distance = Vector3.Distance(gameObject.transform.position, mapMarker.transform.position);
                if (distance < closestDistance && !mapMarker._collectible.Collected)
                {
                    gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                    closestDistance = distance;
                    closestMarker = mapMarker;
                }
                else if (!closestMarker)
                {
                    gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
            }
            if (closestMarker != null)
            {
                transform.LookAt(closestMarker.gameObject.transform.position);
            }
        }
    }
}
