using UnityEngine;
using GCU.CultureTour.Map;
using System.Collections.Generic;
using System.Linq;

namespace GCU.CultureTour
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "GCU/Create Game Settings")]
    public class GameSettingsSO : ScriptableObject
    {

        [Header("Collectibles")]
        [Tooltip("The order these are in is the order they will appear in the logbook if they are undiscovered.")]
        public CollectibleSO[] Collectibles = new CollectibleSO[0];

        public IReadOnlyList<CollectibleSO> OrderedCollectiblesList
        {
            get
            {
                List<CollectibleSO> l = new List<CollectibleSO>();

                // Add in the collected objects sorted by the order they were collected in.
                l.AddRange(
                    Collectibles
                    .Where(c => c.CollectedOrder >= 0)
                    .OrderBy(c => c.CollectedOrder)
                    );

                // add in the uncollected objects in their original order
                l.AddRange(Collectibles.Where(c => c.CollectedOrder < 0));

                return l;
            }
        }

        public string AreaTextWhenObEjectNotDiscovered = string.Empty;

        [Header("Animations")]
        [Tooltip("The order these are in is the order the player will see them played.")]
        public AnimationSO[] Animations = new AnimationSO[0];
        
        [Header("API Map Markers")] 
        public MapMarkerSO[] Markers = new MapMarkerSO[0];

    }
}
