
using System.Collections.Generic;
using UnityEngine;
using PaiSho.Pieces;

namespace PaiSho.Game
{
    public enum ActionType { Placement, Move, Capture, Revival, EchoSummon }

    public struct LogEntry
    {
        public int Turn;
        public Player Player;
        public ActionType Action;
        public PieceType Type;
        public int From;
        public int To;

        public override string ToString()
        {
            string fromTo = (Action == ActionType.Placement || From == To) ? $"at {To}" : $"from {From} to {To}";
            return $"[Turn {Turn}] {Player} {Action}: {Type} {fromTo}";
        }
    }

    public class GameLogManager : MonoBehaviour
    {
        public static GameLogManager Instance;

        private List<LogEntry> entries = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void Log(ActionType action, Player player, PieceType type, int from, int to)
        {
            entries.Add(new LogEntry
            {
                Turn = GameManager.Instance.GetTurnNumber(),
                Player = player,
                Action = action,
                Type = type,
                From = from,
                To = to
            });
        }

        public void PrintLog()
        {
            DebugLogger.Log("======= Full Game Log =======");
            foreach (var entry in entries)
            {
                DebugLogger.Log(entry.ToString());
            }
        }

        public List<LogEntry> GetEntries()
        {
            return new List<LogEntry>(entries);
        }
    }
}
