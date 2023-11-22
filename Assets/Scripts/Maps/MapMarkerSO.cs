using UnityEngine;

namespace GCU.CultureTour.Map
{
    [CreateAssetMenu(fileName = "MapMarker", menuName = "Map/Create Marker")]
    public class MapMarkerSO : ScriptableObject
    {
        public GameObject MarkerPrefab;
        public double Lat;
        public double Lng;
        public Vector3 Rotation;
        [Range(0f, 15f), Tooltip("Distance in meters.")]
        public float InteractionRadius;
        public string Name;
        public string SceneToLoadOnInteraction;
    }
}
