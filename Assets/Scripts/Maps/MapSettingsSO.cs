using UnityEngine;

namespace GCU.CultureTour.Map
{
    [CreateAssetMenu(fileName = "MapMarker", menuName = "Map/Create Map Settings")]
    public class MapSettingsSO : ScriptableObject
    {
        public MapMarkerSO[] Markers = new MapMarkerSO[0];
    }
}
