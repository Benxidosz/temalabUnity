using System;
using System.Collections.Generic;
using System.Linq;
using Map;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : NetworkBehaviour
{
    [Serializable]
    public struct NamedUI
    {
        public UIKeys key;
        public Canvas ui;
    }

    [SerializeField] private NamedUI[] namedUis;

    public enum TurnState {
        BeforeRoll,
        Rolled
    }

    public enum UIKeys {
        DicePicker,
        MaterialPicker,
        AlertDialog,
        RobberMsg
    }

    public static GameManager Instance { get; private set; }

    [FormerlySerializedAs("_players")] [SerializeField]
    private List<PlayerController> players = new List<PlayerController>();

    public PlayerController Player { get; private set; }
    public TurnState CurrentTurnState { get; private set; }

    public Dictionary<UIKeys, Canvas> UIs;
    public List<PlayerController> Players => players;

    [SerializeField] private List<TileController> _tileControllers = new List<TileController>();
    private NetworkVariable<ulong> _currentPlayerIdx = new NetworkVariable<ulong>(0UL);

    [SerializeField] private TextMeshProUGUI barbarianText;

    [SerializeField] private Sprite emptyCard;
    public Sprite EmptyCard => emptyCard;
    private int _barbarianTurn = 7;
    public Robber Robber { private get; set; }
    private bool _robberMovable;

    public bool IsCurrent => NetworkManager.Singleton.LocalClientId == _currentPlayerIdx.Value;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentTurnState = TurnState.BeforeRoll;
            UIs = new Dictionary<UIKeys, Canvas>();
            foreach (var ui in namedUis)
            {
                UIs[ui.key] = ui.ui;
                ui.ui.enabled = false;
            }
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddTileController(TileController controller)
    {
        _tileControllers.Add(controller);
    }

    public void RegisterPlayer(PlayerController player)
    {
        if (players.Count == 0)
        {
            Player = player;
        }
        else
        {
            player.PointsSwitchState();
        }

        player.PointIncreased += CheckWin;
        players.Add(player);
    }

    private void Alert(string msg)
    {
        UIs[UIKeys.AlertDialog].GetComponent<AlertDialog>().ShowDialog(msg);
    }

    private void RefreshBarbarianText()
    {
        barbarianText.text = $"{_barbarianTurn} Black Rolls Until Barbarians";
    }

    private void BarbariansComing()
    {
        Debug.Log("Barbarians!");
    }

    public void BlackRolled()
    {
        if (IsCurrent)
        {
            _barbarianTurn--;
            if (_barbarianTurn == 0)
            {
                BarbariansComing();
                _barbarianTurn = 7;
            }

            RefreshBarbarianText();
        }
        else
        {
            Alert("Not your turn!");
        }
    }

    public void DrawActionCard(ActionDice action)
    {
        if (IsCurrent)
        {
            Player.DrawActionCard(action);
        }
        else
        {
            Alert("Not your turn!");
        }
    }

    public void Rolled(int sum) {
        if (IsCurrent) {
            if (sum == 7) {
                RobberMovable();
            }
            CurrentTurnState = TurnState.Rolled;
            foreach (var controller in _tileControllers.Where(oc => oc.MyNumber == sum)){
                controller.Harvest();
            }
        } else
        {
            Alert("Not your turn!");
        }
    }

    public void RobberMovable() {
        _robberMovable = true;
        UIs[UIKeys.RobberMsg].enabled = true;
    }
    public void MoveRobber(GameObject tile)
    {
        if (_robberMovable == false) return;
        GameObject.FindWithTag("Robber").GetComponent<Robber>().ChangeTile(tile);
        _robberMovable = false;
        UIs[UIKeys.RobberMsg].enabled = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void NextPlayerServerRPC()
    {
        _currentPlayerIdx.Value++;
        if (_currentPlayerIdx.Value >= (ulong) NetworkManager.Singleton.ConnectedClientsList.Count)
            _currentPlayerIdx.Value = 0UL;
        NotifyPlayersClientRPC();
    }

    [ClientRpc]
    private void NotifyPlayersClientRPC()
    {
        if (IsCurrent)
        {
            Player.PointsSwitchState();
            CurrentTurnState = TurnState.BeforeRoll;
            Alert("Your turn!");
        }
    }

    public void EndTurn()
    {
        if (!IsCurrent)
        {
            Alert("Not your turn!");
            return;
        }

        if (CurrentTurnState == TurnState.Rolled)
        {
            Player.ResetTemporaryNeed();
            Player.PointsSwitchState();
            NextPlayerServerRPC();
        }
        else
        {
            Alert("You have not rolled yet!");
        }
    }

    public void Village()
    {
        if (IsCurrent)
        {
            Player.BuildingController.BuildVillage();
        }
        else
        {
            Alert("Not your turn!");
        }
    }

    public void City()
    {
        if (IsCurrent)
        {
            Player.BuildingController.BuildCity();
        }
        else
        {
            Alert("Not your turn!");
        }
    }

    public void Road()
    {
        if (IsCurrent)
        {
            Player.BuildingController.BuildRoad();
        }
        else
        {
            Alert("Not your turn!");
        }
    }

    public void UpdatePanel()
    {
        if (IsCurrent)
        {
            Player.MaterialController.UpdatePanel();
        }
        else
        {
            Alert("Not your turn!");
        }
    }

    public void ShowPickMaterial(Action showUI, Action<MaterialType?> callBack)
    {
        showUI();
        UIs[UIKeys.MaterialPicker].GetComponentInChildren<MaterialSubmitButton>().OnClick = callBack;
    }

    public IEnumerable<TileController> GetTiles(MaterialType type)
    {
        List<TileController> res = new List<TileController>();
        _tileControllers.ForEach(tile =>
        {
            if (tile.MyType == type)
                res.Add(tile);
        });
        return res;
    }

    private void CheckWin(int point, PlayerController player) {
        if (point >= 10) {
            Alert($"{player.Id} is Won!");
        }
    }
}