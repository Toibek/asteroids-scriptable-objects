using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Player player = input.GetComponent<Player>();
        Players.Add(player);
        player.Setup(Ships[Random.Range(0, Ships.Count)]);
    }
    void OnPlayerLeft(PlayerInput input)
    {
        Players.Remove(input.GetComponent<Player>());
    }

    [ContextMenu("Load Ships")]
    void LoadShips()
    {
        Ships = new();
        Ships.AddRange(Resources.LoadAll<ShipSO>(""));
    }
}
