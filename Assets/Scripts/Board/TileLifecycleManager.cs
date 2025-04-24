
using System.Collections.Generic;
using UnityEngine;
using PaiSho.Pieces;
using PaiSho.Game;
using PaiSho.Rules;

namespace PaiSho.Board
{
    public class TileLifecycleManager : MonoBehaviour
    {
        public static TileLifecycleManager Instance;

        private Dictionary<int, int> inactivityCounters = new(); // coord → inactive turns

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void OnTurnStart(List<Piece> allPieces)
        {
            foreach (var piece in allPieces)
            {
                int coord = piece.GetPosition();

                if (piece.Type == PieceType.Orchid || piece.Type == PieceType.Knotweed)
                    continue;

                if (IsNurtured(piece, allPieces))
                {
                    inactivityCounters[coord] = 0;
                }
                else
                {
                    if (!inactivityCounters.ContainsKey(coord))
                        inactivityCounters[coord] = 1;
                    else
                        inactivityCounters[coord]++;
                }

                ApplyWiltEffects(piece, inactivityCounters[coord]);
            }
        }

        private bool IsNurtured(Piece piece, List<Piece> allPieces)
        {
            if (piece.HasMovedRecently())
                return true;

            foreach (var other in allPieces)
            {
                if (other == piece) continue;
                if (HarmonyManager.Instance.IsHarmony(piece, other))
                    return true;
            }

            foreach (var other in allPieces)
            {
                if (other.Type == PieceType.Lotus && BloomingManager.Instance.IsBlooming(other.Owner))
                {
                    int dist = Mathf.Abs(other.GetPosition() / 19 - piece.GetPosition() / 19)
                             + Mathf.Abs(other.GetPosition() % 19 - piece.GetPosition() % 19);
                    if (dist <= 2)
                        return true;
                }
            }

            return false;
        }

        private void ApplyWiltEffects(Piece piece, int turnsInactive)
        {
            if (turnsInactive == 2)
            {
                piece.SetVisualState("fading");
                piece.PointValue = 0;
            }
            else if (turnsInactive == 3)
            {
                piece.SetVisualState("wilting");
                piece.PointValue = -1;
            }
            else if (turnsInactive >= 4)
            {
                piece.SetVisualState("withered");
                BoardManager.Instance.RemovePiece(piece);
            }
        }

        public void OnPieceMoved(Piece piece)
        {
            inactivityCounters[piece.GetPosition()] = 0;
        }
    }
}
