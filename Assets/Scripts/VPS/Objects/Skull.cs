using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

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

        [SerializeField]
        LocalizedStringTable localizedStringTable;
        private StringTable stringTable;

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
                var tableLoading = localizedStringTable.GetTable();
                stringTable = tableLoading;
                statusDisplay.DisplayMessage($"{piecesCollected} / {movedSkullPieces.Length} " + stringTable.GetEntry("Skull_collect_piece").Value, true);
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
                    var tableLoading = localizedStringTable.GetTable();
                    stringTable = tableLoading;
                    statusDisplay.DisplayMessage(stringTable.GetEntry("Skull_missing_pieces").Value, true);
                }
            }
        }
    }
}
