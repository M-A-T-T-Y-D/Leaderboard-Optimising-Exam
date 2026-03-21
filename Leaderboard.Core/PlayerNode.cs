using System.Xml;

namespace Leaderboard.Core;
public sealed class PlayerNode
{
    public PlayerNode(Player value, PlayerNode? left = null, PlayerNode? right = null)
    {
        Player = value;
        Left = left;
        Right = right;
    }

    public Player Player { get; }
    public PlayerNode? Left { get; set; }
    public PlayerNode? Right { get; set;}

        
    }



    