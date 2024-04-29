using UnityEngine;
using UnityEngine.Events;

namespace GCU.CultureTour
{
    public class ClickableObject : MonoBehaviour
    {
        public event UnityAction<GameObject> Clicked;

        private void OnMouseDown()
        {
            Clicked?.Invoke(gameObject);
        }
    }
}
