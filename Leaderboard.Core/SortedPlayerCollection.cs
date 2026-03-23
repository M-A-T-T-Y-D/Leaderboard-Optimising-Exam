using System.Collections.ObjectModel;

namespace Leaderboard.Core;

public sealed class SortedPlayerCollection : IPlayerCollection
{
    private readonly Dictionary<string, Player> _playersById;
    private readonly SortedSet<Player> _sortedPlayers;
    private readonly PlayerRankingComparer _comparer;

    public SortedPlayerCollection()
    {
        _playersById = new Dictionary<string, Player>(StringComparer.OrdinalIgnoreCase);
        _comparer = new PlayerRankingComparer();
        _sortedPlayers = new SortedSet<Player>(_comparer);
    }

    public int Count
    {
        get { return _playersById.Count; }
    }

    public void Add(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        if (!_playersById.TryAdd(player.Id, player))
        {
            throw new DuplicatePlayerIdException(player.Id);
        }

        _sortedPlayers.Add(player);
    }

    public bool TryGetById(string playerId, out Player player)
    {
        if (playerId == null)
        {
            throw new ArgumentNullException(nameof(playerId));
        }

        return _playersById.TryGetValue(playerId, out player);
    }

    public IEnumerable<Player> GetAll()
    {
        return new ReadOnlyCollection<Player>(new List<Player>(_sortedPlayers));
    }

    public void ReplaceAll(IEnumerable<Player> players)
    {
        if (players == null)
        {
            throw new ArgumentNullException(nameof(players));
        }

        _playersById.Clear();
        _sortedPlayers.Clear();

        foreach (Player p in players)
        {
            _playersById[p.Id] = p;
            _sortedPlayers.Add(p);
        }
    }

    public void UpdatePlayerRank(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        _sortedPlayers.Remove(player);
        _sortedPlayers.Add(player);
    }

    public IEnumerable<Player> GetAllSorted()
    {
        return new ReadOnlyCollection<Player>(new List<Player>(_sortedPlayers));
    }

    private sealed class PlayerRankingComparer : IComparer<Player>
    {
        public int Compare(Player x, Player y)
        {
            if (x.Score > y.Score) return -1;
            if (x.Score < y.Score) return 1;

            return string.Compare(x.Id, y.Id, StringComparison.Ordinal);
        }
    }
}
