using System.Collections.ObjectModel;

namespace Leaderboard.Core;

/// <summary>
/// Simple storage for players.
///
/// - Uses a List<Player>
/// - Checks for duplicate ids by scanning the whole list
/// - Finds players by scanning the whole list
///
/// </summary>
public sealed class NaivePlayerCollection : IPlayerCollection
{
    private readonly Dictionary<string, Player> _players;

    public NaivePlayerCollection()
    {
        _players = new Dictionary<string, Player>(StringComparer.OrdinalIgnoreCase);
    }

    public int Count
    {
        get { return _players.Count; }
    }

    public void Add(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        // changed to be faster 0(1) instead of 0(n) by using a dictionary instead of a list
        if (!_players.TryAdd(player.Id, player))
        {
            throw new DuplicatePlayerIdException(player.Id);
        }
    }

    public bool TryGetById(string playerId, out Player player)
    {
        if (playerId == null)
        {
            throw new ArgumentNullException(nameof(playerId));
        }

        // Lookup (slow): scan every existing player.
        return _players.TryGetValue(playerId, out player);
    }

    public IEnumerable<Player> GetAll()
    {
        // Return a read-only shallow copy of the player list.
        return new ReadOnlyCollection<Player>(new List<Player>(_players.Values));
    }

    public void ReplaceAll(IEnumerable<Player> players)
    {
        if (players == null)
        {
            throw new ArgumentNullException(nameof(players));
        }

        _players.Clear();
        foreach (Player p in players)
        {
            _players[p.Id] = p;
        }
    }
}