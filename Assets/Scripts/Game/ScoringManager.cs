
using System.Collections.Generic;
using UnityEngine;
using PaiSho.Pieces;
using PaiSho.Board;

namespace PaiSho.Game
{
    public class ScoringManager : MonoBehaviour
    {
        public static ScoringManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public int GetTotalScore(Player player, List<Piece> allPieces)
        {
            int score = 0;
            score += GetHarmonyScore(player, allPieces);
            score += GetStylizedBonus(player, allPieces);
            score -= GetDisharmonyPenalty(player, allPieces);
            score += GetWiltBonus(player, allPieces); // Includes penalty logic too
            return score;
        }

        private int GetHarmonyScore(Player player, List<Piece> pieces)
        {
            int harmonyPoints = 0;

            foreach (var piece in pieces)
            {
                if (piece.Owner != player) continue;
                foreach (var other in pieces)
                {
                    if (other == piece || other.Owner != player) continue;
                    if (HarmonyManager.Instance.IsHarmony(piece, other))
                        harmonyPoints++;
                }
            }

            return harmonyPoints / 2; // Harmonies are bidirectional
        }

        private int GetDisharmonyPenalty(Player player, List<Piece> pieces)
        {
            int penalty = 0;

            foreach (var piece in pieces)
            {
                if (piece.Owner != player) continue;
                foreach (var other in pieces)
                {
                    if (other == piece || other.Owner != player) continue;
                    if (HarmonyManager.Instance.IsDisharmony(piece, other))
                        penalty++;
                }
            }

            return penalty / 2; // Disharmonies are bidirectional
        }

        private int GetWiltBonus(Player player, List<Piece> pieces)
        {
            int score = 0;

            foreach (var piece in pieces)
            {
                if (piece.Owner != player) continue;
                score += piece.PointValue;
            }

            return score;
        }

        private int GetStylizedBonus(Player player, List<Piece> pieces)
        {
            int bonus = 0;

            HashSet<PieceType> flowerTypes = new();
            int lightDarkMixes = 0;
            int retreatMoves = 0;
            int harmonyMoves = 0;
            int seasonalMatches = 0;

            foreach (var piece in pieces)
            {
                if (piece.Owner != player) continue;

                // Floral Palette Bonus
                if (piece.IsFlower())
                    flowerTypes.Add(piece.Type);

                // Oppositional Harmony
                foreach (var other in pieces)
                {
                    if (other.Owner == player || other == piece) continue;
                    if (HarmonyManager.Instance.IsHarmony(piece, other) && piece.IsFlower() && other.IsFlower())
                        lightDarkMixes++;
                }

                // Graceful Retreat
                if (piece.MovedFromDisharmonyToHarmony)
                    retreatMoves++;

                // Flow Bonus
                if (piece.HasMovedRecently() && piece.InHarmony)
                    harmonyMoves++;

                // Seasonal Blooming (example logic, adjustable by date/enum)
                if (piece.Type == PieceType.Jasmine || piece.Type == PieceType.Lily || piece.Type == PieceType.Jade)
                    seasonalMatches++;
            }

            if (flowerTypes.Count >= 4) bonus += 3; // Floral Palette Bonus
            bonus += lightDarkMixes;               // Oppositional Harmony
            bonus += retreatMoves * 2;             // Graceful Retreat
            if (harmonyMoves >= 3) bonus += 5;     // Flow Bonus
            bonus += seasonalMatches;              // Seasonal Blooming

            // Empty Harmony and Poetic Finish are board-based pattern checks (TODO)

            return bonus;
        }
    }
}
