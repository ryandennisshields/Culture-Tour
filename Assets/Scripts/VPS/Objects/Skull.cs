using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GCU.CultureTour
{
    public class Skull : Triggered
    {
        [SerializeField]
        private Material notCompletedMaterial;
        [SerializeField]
        private Material completedMaterial;
        [SerializeField]
        private GameObject[] skullPieces;
        [SerializeField]
        private GameObject[] movedSkullPieces;

        private int piecesCollected = 0;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var piece in skullPieces)
            {
                piece.GetComponent<Renderer>().material = notCompletedMaterial;
            }
            foreach (var piece in movedSkullPieces)
            {
                piece.AddComponent<ClickableObject>();
                piece.GetComponent<ClickableObject>().Clicked += PieceCollected;
            }
        }

        private void PieceCollected(GameObject pieceObject)
        {
            piecesCollected++;
            pieceObject.SetActive(false);
            var statusDisplay = FindObjectOfType<StatusMessageDisplay>();
            if (statusDisplay != null)
            {
                statusDisplay.DisplayMessage($"{piecesCollected} / {movedSkullPieces.Length} Collected", true);
            }
            if (piecesCollected == movedSkullPieces.Length)
            {
                foreach (var piece in skullPieces)
                {
                    piece.GetComponent<Renderer>().material = completedMaterial;
                }
            }
        }

        private void OnMouseDown()
        {
            if (piecesCollected == movedSkullPieces.Length)
            {
                hiddenObjectScript.Tapped(gameObject);
            }
            else
            {
                var statusDisplay = FindObjectOfType<StatusMessageDisplay>();
                if (statusDisplay != null)
                {
                    statusDisplay.DisplayMessage("It's missing pieces...", true);
                }
            }
        }
    }
}
