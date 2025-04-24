

using UnityEngine;

using System.Collections.Generic;

using PaiSho.Pieces;

using PaiSho.Board;



namespace PaiSho.Game

{

    public class GameEndManager : MonoBehaviour

    {

        public static GameEndManager Instance;



        private void Awake()

        {

            if (Instance != null && Instance != this)

                Destroy(gameObject);

            else

                Instance = this;

        }



        public void ResolveFinalScore()

        {

            List<Piece> allPieces = BoardManager.Instance.GetAllPieces();

            int hostScore = ScoringManager.Instance.GetTotalScore(Player.Host, allPieces);

            int opponentScore = ScoringManager.Instance.GetTotalScore(Player.Opponent, allPieces);



            Debug.Log("======== FINAL SCORES ========");

            Debug.Log($"Host: {hostScore}");

            Debug.Log($"Opponent: {opponentScore}");



            if (hostScore > opponentScore)

                Debug.Log("🏆 Host wins!");

            else if (opponentScore > hostScore)

                Debug.Log("🏆 Opponent wins!");

            else

                Debug.Log("🤝 It's a draw!");



            // TODO: Hook into UI end screen later

        }

    }

}

