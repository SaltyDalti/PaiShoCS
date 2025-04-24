
using System.Collections.Generic;
using UnityEngine;
using PaiSho.Pieces;
using PaiSho.Board;

namespace PaiSho.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private bool gameStarted = false;
        private int currentPlayerIndex = 0;
        private Player[] players = new Player[] { Player.Host, Player.Opponent };
        private bool springPhase = true;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        void Start()
        {
            Debug.Log("Spring Opening Phase begins!");
            gameStarted = true;
            springPhase = true;
        }

        public Player GetCurrentPlayer()
        {
            return players[currentPlayerIndex];
        }

        public void EndTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % 2;

            if (springPhase && currentPlayerIndex == 0)
            {
                springPhase = false;
                Debug.Log("Spring Phase complete. Entering normal gameplay.");
            }

            // Evaluate lifecycle at start of next player's turn
            List<Piece> allPieces = BoardManager.Instance.GetAllPieces();
            TileLifecycleManager.Instance.OnTurnStart(allPieces);
        }

        public PieceType GetOpeningFlower(Player player)
        {
            return player == Player.Host ? PieceType.Jasmine : PieceType.Rose;
        }

        public bool IsSpringPhase()
        {
            return springPhase;
        }
    }
}
