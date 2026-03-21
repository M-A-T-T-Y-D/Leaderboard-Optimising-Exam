namespace Leaderboard.Core;

public sealed class PlayerScoreTree
{
    public PlayerNode? Root { get; set; }

    private int Compare(Player a, Player b)
    {
        int scoreComparison = b.Score.CompareTo(a.Score); // Higher scores come first

        if (scoreComparison != 0)
        {
            return scoreComparison;
        }
        return string.Compare(a.Id, b.Id, StringComparison.Ordinal); // Tie-breaker: sort by Id
    }

    public void Insert(Player player) // inserts the player into correct ranked positions
    {
        Root = Insert(Root, player);
    }

    private PlayerNode Insert(PlayerNode? node, Player player)
    {
        if (node == null)
        {
            return new PlayerNode(player);
        }

        int comparison = Compare(player, node.Player);

        if (comparison < 0)
        {
            node.Left = Insert(node.Left, player);
        }
        else if (comparison > 0)
        {
            node.Right = Insert(node.Right, player);
        }
        else
        {
            node.Player.SetScore(player.Score);
        }

        return node;
    }

    //remove player before score update
    public void Remove(Player player)
    {
        Root = Remove(Root, player);
    }

    private PlayerNode? Remove(PlayerNode? node, Player player)
    {
        if (node == null)
        {
            return null;
        }

        int comparison = Compare(player, node.Player);

        if (comparison < 0)
        {
            node.Left = Remove(node.Left, player);
        }
        else if (comparison > 0)
        {
            node.Right = Remove(node.Right, player);
        }
        else
        {
            if (node.Left == null)
            {
                return node.Right;
            }
            else if (node.Right == null)
            {
                return node.Left;
            }

            PlayerNode min = FindMin(node.Right);

            node = new PlayerNode(
                min.Player, 
                node.Left, 
                Remove(node.Right, min.Player)
                );
        }
        return node;
    }

    private PlayerNode FindMin(PlayerNode node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

  public List<Player> DepthFirstTraversal()
    {
        var result = new List<Player>();

        void InOrder(PlayerNode? node)
        {
            if (node == null)
            {
                return;
            }

            InOrder(node.Left);
            result.Add(node.Player);
            InOrder(node.Right);
        }

        InOrder(Root);
        return result;

    }

    public List<Player> GetTopK(int k)
    {
        var result = new List<Player>();

        void Traverse(PlayerNode? node)
        {
            if (node == null || result.Count >= k)
            {
                return;
            }

            Traverse(node.Left);

            if (result.Count < k)
            {
                result.Add(node.Player);
            }

            Traverse(node.Right);
        }
        Traverse(Root);

        return result; 
    }
    public int GetRank(Player player)
    {
        int rank = 0;

        void Traverse(PlayerNode? node)
        {
            if (node == null)
            {
                return;
            }

            Traverse(node.Left);

            if (node.Player == player)
            {
                return;
            }

            Traverse(node.Right);

        }
        Traverse(Root);

        return rank;
    }

}