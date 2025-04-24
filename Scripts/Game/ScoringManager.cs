
using System.Collections.Generic;
using UnityEngine;
using PaiSho.Pieces;
using PaiSho.Board;

namespace PaiSho.Game
{
    public class ScoringManager : MonoBehaviour
    {
        public static ScoringManager Instance;

        private Dictionary<Player, int> totalScores = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            totalScores[Player.Host] = 0;
            totalScores[Player.Opponent] = 0;
        }

        public int CalculateScore(Player player, List<Piece> pieces)
        {
            int score = 0;

            foreach (Piece piece in pieces)
            {
                if (piece.Owner != player || piece.IsGhost) continue;

                score += piece.GetScoreValue();

                if (piece.Type == PieceType.Lotus && piece.IsBlooming())
                    score += 2;

                if (SeasonManager.Instance.IsInSeason(piece.Type))
                    score += 1;

                if (piece.PreviousWiltLevel > piece.WiltLevel && piece.WiltLevel < 2)
                    score += 2; // revival bonus
            }

            score += ApplyPoeticBonuses(player, pieces);

            totalScores[player] += score;
            return score;
        }

        private int ApplyPoeticBonuses(Player player, List<Piece> pieces)
        {
            int bonus = 0;

            // Placeholder ideas:
            // - All tiles in harmony (Flow Bonus)
            // - Oppositional harmony (Dark + Light)
            // - Empty Harmony (harmony with no neighbors)
            // - Poetic Finish (tile placed that ends the game also forms harmony)

            // Simulate a poetic bonus here for now
            if (pieces.FindAll(p => p.Owner == player && p.InHarmony).Count >= 5)
            {
                bonus += 3;
                DebugLogger.Log($">>> {player} earned a Flow Bonus (+3)");
            }

            return bonus;
        }

        public int GetTotalScore(Player player)
        {
            return totalScores[player];
        }

        public Dictionary<Player, int> GetAllScores()
        {
            return new Dictionary<Player, int>(totalScores);
        }
    }
}

        public void AwardBonus(Player player, int points)
        {
            if (points <= 0) return;
            if (!totalScores.ContainsKey(player)) return;
            totalScores[player] += points;
        }
