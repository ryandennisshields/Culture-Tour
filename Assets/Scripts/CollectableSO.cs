using GCU.CultureTour.Map;
using System;
using UnityEngine;

namespace GCU.CultureTour
{
    [CreateAssetMenu(fileName = "Collectable", menuName = "GCU/Create Collectable")]
    public class CollectableSO : ScriptableObject
    {
        public MapMarkerSO MapMarker;
        [Header("Logbook")]
        public string AreaName;
        public string AreaText;
        public string ModelDescription;
        public GameObject LogbookModel;

        public bool Collected => CollectedOrder >= 0;
        public int CollectedOrder { get; private set; } = -1; 

        /// <summary>
        /// Mark the collectable as collected (or not). 
        /// The change will be persisted to PlayerPrefs.
        /// </summary>
        /// <param name="collectedOrder">
        /// 0 or a positive number indicates the order in which this collectable was collected.
        /// -1 indicates that this collectable has not been collected.
        /// </param>
        public void MarkCollected(int collectedOrder)
        {
            if (CollectedOrder < -1 )
            {
                CollectedOrder = -1;
            }

            CollectedOrder = collectedOrder;
            Save();
        }

        private const string KEY_PREFIX = "Collectable ";

        public void Load( )
        {
            string key = KEY_PREFIX + name;

            CollectedOrder = PlayerPrefs.HasKey(key) 
                ? PlayerPrefs.GetInt(key) 
                : -1;
        }

        private void Save()
        {
            string key = KEY_PREFIX + name;

            PlayerPrefs.SetInt(key, CollectedOrder);
            PlayerPrefs.Save();
        }
    }
}