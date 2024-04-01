using GCU.CultureTour.VPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCU.CultureTour
{
    public class Triggered : MonoBehaviour
    {
        [HideInInspector]
        public HiddenObject hiddenObjectScript;
        [HideInInspector]
        public GameObject hiddenObject;

        internal int counter = 0;

        internal Vector3 holdPosition;
        internal Vector3 startPosition;
        internal Vector3 newPosition;
        internal bool isHolding;

        void Awake()
        {
            hiddenObjectScript = GetComponentInParent<HiddenObject>();
            hiddenObject = hiddenObjectScript.gameObject;
        }
    }
}
