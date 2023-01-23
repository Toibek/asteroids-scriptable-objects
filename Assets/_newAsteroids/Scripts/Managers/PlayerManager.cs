using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class PlayerManager : MonoBehaviour
{
    [SerializeField] List<ShipSO> Ships;
    [SerializeField] List<Player> Players;
    PlayerInputManager pim;
    private void Start()
    {
        pim = GetComponent<PlayerInputManager>();
    }
    void OnPlayerJoined(PlayerInput input)
    {
        loadShips();
        Player player = input.GetComponent<Player>();
        Players.Add(player);
        player.Setup(Ships[Random.Range(0, Ships.Count)]);
        if (Players.Count == 1) SendMessage("OnGameStart");
    }
    void OnPlayerLeft(PlayerInput input)
    {
        Players.Remove(input.GetComponent<Player>());
        Debug.Log("Player left, remaining: " + Players.Count);
        if (Players.Count == 0) SendMessage("OnGameStop");
    }
    void loadShips()
    {
#if UNITY_EDITOR
        string[] allPaths = Directory.GetFiles("Assets/Data", "*.asset", SearchOption.AllDirectories);
        foreach (string path in allPaths)
        {
            string cleanPath = path.Replace("\\", "/");
            ScriptableObject baseObj = (ScriptableObject)AssetDatabase.LoadAssetAtPath(cleanPath, typeof(ScriptableObject));
            switch (baseObj.GetType().ToString())
            {
                case "ShipSO":
                    Ships.Add((ShipSO)baseObj);
                    break;
                default:
                    Debug.Log("Not ship or component");
                    break;
            }
        }
#endif
    }
    [ContextMenu("Load Ships")]
    void LoadShips()
    {
        Ships = new();
        Ships.AddRange(Resources.LoadAll<ShipSO>(""));
    }
}
