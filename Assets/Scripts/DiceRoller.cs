using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour{

    [SerializeField] private Image whiteDiceImage;
    [SerializeField] private Image redDiceImage;

    [SerializeField] private Sprite[] whiteSprites = new Sprite[6];
    [SerializeField] private Sprite[] redSprites = new Sprite[6];
    
    public int RedDice{ get; private set; }
    public int WhiteDice{ get; private set; }
    public int Sum => RedDice + WhiteDice;

    public void Roll(){
        WhiteDice = Random.Range(1, 7);
        RedDice = Random.Range(1, 7);
        print(WhiteDice + " " + RedDice + " = " + Sum);
        StartCoroutine(Roller());
    }

    private IEnumerator Roller(){
        for (var i = 0; i < 10; i++){
            whiteDiceImage.sprite = whiteSprites[Random.Range(0, 6)];
            redDiceImage.sprite = redSprites[Random.Range(0, 6)];
            yield return new WaitForSeconds(0.1f);
        }
        whiteDiceImage.sprite = whiteSprites[WhiteDice - 1];
        redDiceImage.sprite = redSprites[RedDice - 1];
    }
    
}
