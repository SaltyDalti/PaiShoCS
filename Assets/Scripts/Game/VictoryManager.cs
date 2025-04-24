

using System.Collections.Generic;

using UnityEngine;

using PaiSho.Pieces;

using PaiSho.Board;



namespace PaiSho.Game

{

    public class VictoryManager : MonoBehaviour

    {

        public static VictoryManager Instance;



        private void Awake()

        {

            if (Instance != null && Instance != this)

                Destroy(gameObject);

            else

                Instance = this;

        }



        public bool CheckForHarmonyRingEnd(Player player, List<Piece> pieces)

        {

            int centerPort = 180;

            HashSet<int> playerCoords = new();

            Dictionary<int, Piece> coordToPiece = new();



            foreach (var piece in pieces)

            {

                if (piece.Owner == player)

                {

                    int coord = piece.GetPosition();

                    playerCoords.Add(coord);

                    coordToPiece[coord] = piece;

                }

            }



            List<int> ring = FindPotentialRing(centerPort, playerCoords, coordToPiece);

            if (ring.Count >= 4 && FormsClosedRing(ring, coordToPiece))

            {

                Debug.Log($">>> Harmony Ring formed by {player}. Ending game.");

                GameManager.Instance.EndGame(player);

                return true;

            }



            return false;

        }



        private List<int> FindPotentialRing(int center, HashSet<int> playerCoords, Dictionary<int, Piece> pieces)

        {

            int[] ringOffsets = { -20, -19, -18, -1, 1, 18, 19, 20 }; // Approximate neighbors

            List<int> ring = new();



            foreach (int offset in ringOffsets)

            {

                int coord = center + offset;

                if (playerCoords.Contains(coord))

                    ring.Add(coord);

            }



            return ring;

        }



        private bool FormsClosedRing(List<int> coords, Dictionary<int, Piece> pieces)

        {

            if (coords.Count < 4) return false;



            for (int i = 0; i < coords.Count; i++)

            {

                int a = coords[i];

                int b = coords[(i + 1) % coords.Count];

                if (!HarmonyManager.Instance.IsHarmony(pieces[a], pieces[b]))

                    return false;

            }



            return true;

        }

    }

}

