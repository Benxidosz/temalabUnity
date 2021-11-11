using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Random = System.Random;

public class CardDealer : MonoBehaviour {
    [SerializeField] private List<CardSO> blueCardSos;
    [SerializeField] private List<CardSO> greenCardSos;
    [SerializeField] private List<CardSO> yellowCardSos;

    private List<CardSO> blueDeck;
    private List<CardSO> greenDeck;
    private List<CardSO> yellowDeck;
    
    private List<CardSO> blueDiscardDeck;
    private List<CardSO> greenDiscardDeck;
    private List<CardSO> yellowDiscardDeck;

    public CardSO NextBlueCard {
        get {
            CardSO tmp = blueDeck[0];
            blueDeck.RemoveAt(0);
            if (blueDeck.Count == 0) {
                while (blueDiscardDeck.Count > 0) {
                    int pos = rng.Next(0, blueDiscardDeck.Count);
                    blueDeck.Add(blueDiscardDeck[pos]);
                    blueDiscardDeck.RemoveAt(pos);
                }
            }
            return tmp;
        }
    }
    public CardSO NextGreenCard {
        get {
            CardSO tmp = greenDeck[0];
            greenDeck.RemoveAt(0);
            
            if (greenDeck.Count == 0) {
                while (greenDiscardDeck.Count > 0) {
                    int pos = rng.Next(0, greenDiscardDeck.Count);
                    greenDeck.Add(greenDiscardDeck[pos]);
                    greenDiscardDeck.RemoveAt(pos);
                }
            }
            
            return tmp;
        }
    }
    public CardSO NextYellowCard {
        get {
            CardSO tmp = yellowDeck[0];
            yellowDeck.RemoveAt(0);
            
            if (yellowDeck.Count == 0) {
                while (yellowDiscardDeck.Count > 0) {
                    int pos = rng.Next(0, yellowDiscardDeck.Count);
                    yellowDeck.Add(yellowDiscardDeck[pos]);
                    yellowDiscardDeck.RemoveAt(pos);
                }
            }
            
            return tmp;
        }
    }

    private Random rng;
    public CardDealer Instance { get; private set; }

    private void Awake() {
        if (Instance is null) {
            Instance = this;

            blueDeck = new List<CardSO>();
            greenDeck = new List<CardSO>();
            yellowDeck = new List<CardSO>();
            
            blueDiscardDeck = new List<CardSO>();
            greenDiscardDeck = new List<CardSO>();
            yellowDiscardDeck = new List<CardSO>();
            
            rng = new Random();

            initDeck(blueDeck, blueCardSos);
            initDeck(greenDeck, greenCardSos);
            initDeck(yellowDeck, yellowCardSos);

            for (int i = 0; i < rng.Next(3, 10); i++) {
                blueDeck.Sort((c1, c2) => rng.Next(-1 , 1));    
                greenDeck.Sort((c1, c2) => rng.Next(-1 , 1));    
                yellowDeck.Sort((c1, c2) => rng.Next(-1 , 1));    
            }
            
            // blueDeck.ForEach(c => Debug.Log(c.name));
        } else
            Destroy(this);
    }

    private void initDeck(List<CardSO> deck, List<CardSO> fromDeck) {
        foreach (var card in fromDeck) {
            for (int i = 0; i < card.count; ++i)
                deck.Add(card);
        } 
    }

    private void addToDiscard(CardSO card) {
        if (blueCardSos.Contains(card)) {
            blueDiscardDeck.Add(card);
        } else if (greenCardSos.Contains(card)) {
            greenDiscardDeck.Add(card);
        } else if (yellowCardSos.Contains(card)) {
            yellowDiscardDeck.Add(card);
        }   
    }
}