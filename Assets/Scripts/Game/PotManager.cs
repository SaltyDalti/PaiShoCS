
using System.Collections.Generic;
using UnityEngine;
using PaiSho.Pieces;

namespace PaiSho.Game
{
    public class PotManager : MonoBehaviour
    {
        public static PotManager Instance;

        private Dictionary<Player, List<Piece>> potContents = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            potContents[Player.Host] = new List<Piece>();
            potContents[Player.Opponent] = new List<Piece>();
        }

        public void AddToPot(Piece captured)
        {
            potContents[captured.Owner].Add(captured);
            Debug.Log($"{captured.Owner}'s {captured.Type} added to the Pot.");
        }

        public List<Piece> GetPotContents(Player player)
        {
            return potContents[player];
        }

        public int GetPotCount(Player player)
        {
            return potContents[player].Count;
        }

        public int GetPotDifference(Player player)
        {
            Player opponent = player == Player.Host ? Player.Opponent : Player.Host;
            return potContents[opponent].Count - potContents[player].Count;
        }

        public bool IsLotusBlooming(Player player)
        {
            return GetPotCount(player) < GetPotCount(player == Player.Host ? Player.Opponent : Player.Host);
        }
    }
}
