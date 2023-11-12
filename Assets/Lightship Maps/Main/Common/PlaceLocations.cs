using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using System;
using UnityEngine;

namespace GCU.CultureTour.Map
{
    public class PlaceLocations : MonoBehaviour
    {
        /// <summary>
        /// This code places the objects on the map and gives them their names.
        /// </summary>

        [Serializable]
        private class LocationMarker
        {
            public double Lat;
            public double Lng;
            public string Name;
            public string SceneToLoadOnInteraction;
        }

        [SerializeField] private LocationMarker[] markers;
        [SerializeField] private LayerGameObjectPlacement mapLayer;

        private void Start()
        {
            PlaceMarkers();
        }

        private void PlaceMarkers()
        {
            foreach (LocationMarker marker in markers)
            {
                var obj = mapLayer.PlaceInstance( new LatLng( marker.Lat, marker.Lng ), marker.Name);
                obj.Value.GetComponent<ObjectLogic>()?.Initalise(marker.SceneToLoadOnInteraction);
            }
        }
    }
}