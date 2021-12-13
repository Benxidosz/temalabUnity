using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ActionDice {
    blue, green, yellow, black
}
public class DiceRoller : MonoBehaviour {
    public static DiceRoller Instance { get; private set; }

    [SerializeField] private Image whiteDiceImage;
    [SerializeField] private Image redDiceImage;
    [SerializeField] private Image actionDiceImage;

    [SerializeField] private Sprite[] whiteSprites = new Sprite[6];
    [SerializeField] private Sprite[] redSprites = new Sprite[6];
    [Serializable] public struct ActionSprites {
        public Sprite sprite;
        public ActionDice dice;
    }
    [SerializeField] private ActionSprites[] actionDice = new ActionSprites[6];

    private GameManager _gameManager;

    public int RedDice{ get; private set; }
    public int WhiteDice{ get; private set; }
    public ActionDice ActionDice{ get; private set; }
    public int Sum => RedDice + WhiteDice;

    private Sprite actionSprite;

    private void Start() {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(this);
        }
        _gameManager = GameManager.Instance;
    }

    public void Roll() {
        PlayerController current = _gameManager.CurrentPlayer;
        if (current.DiceSet) {
            WhiteDice = current.WhiteDice;
            RedDice = current.RedDice;
            current.DiceSet = false;
        } else {
            WhiteDice = Random.Range(1, 7);
            RedDice = Random.Range(1, 7);
        }
        
        ActionSprites tmpAction = actionDice[Random.Range(0, 6)];
        ActionDice = tmpAction.dice;
        actionSprite = tmpAction.sprite;
        print(WhiteDice + " " + RedDice + " = " + Sum);
        _gameManager.DrawActionCard(ActionDice);
        
        StartCoroutine(Roller());
        _gameManager.Rolled();
    }

    private IEnumerator Roller(){
        for (var i = 0; i < 10; i++){
            whiteDiceImage.sprite = whiteSprites[Random.Range(0, 6)];
            redDiceImage.sprite = redSprites[Random.Range(0, 6)];
            actionDiceImage.sprite = actionDice[Random.Range(0, 6)].sprite;
            yield return new WaitForSeconds(0.1f);
        }
        whiteDiceImage.sprite = whiteSprites[WhiteDice - 1];
        redDiceImage.sprite = redSprites[RedDice - 1];
        actionDiceImage.sprite = actionSprite;
    }
    
}
