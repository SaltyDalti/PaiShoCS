

using UnityEngine;

using PaiSho.Pieces;

using PaiSho.Board;



namespace PaiSho.Game

{

    public class TileSelector : MonoBehaviour

    {

        public static TileSelector Instance;



        private void Awake()

        {

            if (Instance != null && Instance != this)

                Destroy(gameObject);

            else

                Instance = this;

        }



        public void TryPlaceTile(Player player, PieceType type, int coordinate)

        {

            if (!ReserveManager.Instance.HasTile(player, type))

            {

                Debug.Log("No tiles of this type left in reserve.");

                return;

            }



            if (!PlacementValidator.Instance.CanPlace(player, type, coordinate))

            {

                Debug.Log("Invalid placement.");

                return;

            }



            BoardManager.Instance.PlacePiece(player, type, coordinate);

            ReserveManager.Instance.RemoveFromReserve(player, type);

            GameManager.Instance.MarkTurnComplete();

            GameManager.Instance.EndTurn();

        }

    }

}

