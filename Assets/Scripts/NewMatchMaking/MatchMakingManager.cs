using System.Collections.Generic;
using UnityEngine;

public class MatchMakingManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";
    private static Dictionary<string, PlyrMacthMaking> players = new Dictionary<string, PlyrMacthMaking>();
    public static List<string> PlayersNameConnected = new List<string>();


    public int AllPlyrs;
    public static Dictionary<string, PlyrMacthMaking> GetDictionnary
    {
        get
        {
            return players;
        }
    }
    public static void RegisterPlayer(string netID, PlyrMacthMaking player)
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
        PlayersNameConnected.Add(playerId);
    }

    public static void UnregisterPlayer(string playerID)
    {
        players.Remove(playerID);
        PlayersNameConnected.Remove(playerID);

    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerId in players.Keys)
        {
            GUILayout.Label(playerId + " - " + players[playerId].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

    }

    private void Update()
    {
        AllPlyrs = players.Count;
    }

    public static PlyrMacthMaking GetPlayer(string playerId)
    {
        return players[playerId];
    }
}
