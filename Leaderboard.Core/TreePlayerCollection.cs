namespace Leaderboard.Core;

public sealed class TreePlayerCollection : IPlayerCollection
{
    private readonly Dictionary<string, Player> playersById;
    private readonly PlayerScoreTree rankingTree;

    public TreePlayerCollection()
    {
        playersById = new Dictionary<string, Player>();
        rankingTree = new PlayerScoreTree();
    }

    // REQUIRED BY INTERFACE
    public int Count => playersById.Count;

    public void Add(Player player)
    {
        playersById[player.Id] = player;

        rankingTree.Insert(player);
    }

    public bool TryGetById(string id, out Player player)
    {
        return playersById.TryGetValue(id, out player);
    }

    public void ReplaceAll(IEnumerable<Player> players)
    {
        playersById.Clear();

        foreach (var p in players)
            Add(p);
    }

    public IEnumerable<Player> GetAll()
    {
        return rankingTree.DepthFirstTraversal();
    }

    public void UpdateScore(string id, int delta)
    {
        Player p = playersById[id];

        rankingTree.Remove(p);

        p.AddScore(delta);

        rankingTree.Insert(p);
    }

    public List<Player> GetTopK(int k)
    {
        return rankingTree.GetTopK(k);
    }

    public int GetRank(string id)
    {
        return rankingTree.GetRank(playersById[id]);
    }
}