using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class Card {
    public CardScript Script { get; set; }
    public Button Button { get; set; }

    public Card(CardScript script) {
        Script = script;
        Button = script.GetComponentInChildren<Button>();
    }
}

public class CardInventory : MonoBehaviour {
    private Action lastCalled;
    private List<Card> _cards;
    [SerializeField] private TextMeshProUGUI errorMsg;
    [SerializeField] private Canvas CardUi;
    [SerializeField] private CardSO EmptyCard;
    
    void Start() {
        CardScript[] scripts = CardUi.GetComponentsInChildren<CardScript>();
        _cards = new List<Card>();
        foreach (var script in scripts) {
            _cards.Add(new Card(script));
        }
        _cards.ForEach(c => c.Script.Backend = EmptyCard);
        _cards.ForEach(c => c.Button.onClick.AddListener(() => {
            c.Script.Backend.Action?.Invoke("Player");
            DiscardACard(c);
        }));
        
        errorMsg.gameObject.SetActive(false);
    }

    private CardScript FindEmpty() {
        foreach (var card in _cards) {
            if (card.Script.Backend.Equals(EmptyCard))
                return card.Script;
        }
        errorMsg.gameObject.SetActive(true);
        foreach (var card in _cards) {
            card.Button.onClick.RemoveAllListeners();
            card.Button.onClick.AddListener(() => {
                DiscardACard(card);
                lastCalled?.Invoke();
                _cards.ForEach(c => {
                    c.Button.onClick.RemoveAllListeners();
                    c.Button.onClick.AddListener(() => {
                        c.Script.Backend.Action?.Invoke("Player");
                        DiscardACard(c);
                    });
                });
                errorMsg.gameObject.SetActive(false);
            });
        }
        return null;
    }

    private void DiscardACard(Card card) {
        if (!card.Script.Backend.Equals(EmptyCard)) {
            CardDealer.Instance.AddToDiscard(card.Script.Backend);
            card.Script.Backend = EmptyCard;
        }
    }

    public void AddBlueCard() {
        CardScript emptySlot = FindEmpty();
        if (!(emptySlot is null)) {
            if (!emptySlot.Backend.Equals(EmptyCard)) {
                CardDealer.Instance.AddToDiscard(emptySlot.Backend);   
            }
            emptySlot.Backend = CardDealer.Instance.NextBlueCard;
        } else {
            lastCalled = AddBlueCard;
        }
    }
    public void AddGreenCard() {
        CardScript emptySlot = FindEmpty();
        if (!(emptySlot is null)) {
            if (!emptySlot.Backend.Equals(EmptyCard)) {
                CardDealer.Instance.AddToDiscard(emptySlot.Backend);   
            }
            emptySlot.Backend = CardDealer.Instance.NextGreenCard;
        } else {
            lastCalled = AddGreenCard;
        }
    }
    public void AddYellowCard() {
        CardScript emptySlot = FindEmpty();
        if (!(emptySlot is null)) {
            if (!emptySlot.Backend.Equals(EmptyCard)) {
                CardDealer.Instance.AddToDiscard(emptySlot.Backend);   
            }
            emptySlot.Backend = CardDealer.Instance.NextYellowCard;
        } else {
            lastCalled = AddYellowCard;
        }
    }
}