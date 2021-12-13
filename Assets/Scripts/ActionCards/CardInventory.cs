using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<CardSO> _cardInventory;
    private Queue<CardSO> _cardQueue;

    void Start() {
        CardScript[] scripts = CardUi.GetComponentsInChildren<CardScript>();
        _cards = new List<Card>();
        _cardInventory = new List<CardSO>();

        _cardQueue = new Queue<CardSO>();
        foreach (var script in scripts) {
            _cards.Add(new Card(script));
            _cardInventory.Add(EmptyCard);
        }

        SwitchUiState();
    }

    private void RefillUI() {
        for (int i = 0; i < _cardInventory.Count; i++) {
            _cards[i].Script.Backend = _cardInventory[i];
        }

        if (_cardQueue.Count == 0) {
            SwitchToPlayState();
        } else {
            SwitchToDiscardState();
        }
    }

    private void EmptyUI() {
        _cards.ForEach(c => c.Script.Backend = EmptyCard);
        _cards.ForEach(c => c.Button.onClick.AddListener(() => { c.Script.Backend.Action?.Invoke(GameManager.Instance.CurrentPlayer); }));
    }

    private int FindEmpty() {
        foreach (var card in _cardInventory) {
            if (card.Equals(EmptyCard))
                return _cardInventory.IndexOf(card);
        }
        
        return -1;
    }
    private void SwitchToDiscardState() {
        errorMsg.enabled = true;
        errorMsg.text = $"Error Not enough space.\nChoose {_cardQueue.Count} card to replace.";
        _cards.ForEach(c => {
            c.Button.onClick.RemoveAllListeners();
            c.Button.onClick.AddListener(() => {
                DiscardACard(c);
                var idx = _cards.IndexOf(c);
                _cardInventory[idx] = _cardQueue.Dequeue();
                if (_cardQueue.Count == 0) {
                    SwitchToPlayState();
                }
                RefillUI();
            });
        });
    } 
    private void SwitchToPlayState() {
        errorMsg.enabled = false;
        _cards.ForEach(c => {
            c.Button.onClick.RemoveAllListeners();
            c.Button.onClick.AddListener(() => {
                CardSO tmp = c.Script.Backend;
                DiscardACard(c);
                tmp.Action?.Invoke(GameManager.Instance.CurrentPlayer);
            });
        });
    }
    private void DiscardACard(Card card) {
        if (!card.Script.Backend.Equals(EmptyCard)) {
            CardDealer.Instance.AddToDiscard(card.Script.Backend);
            card.Script.Backend = EmptyCard;

            int idx = _cards.IndexOf(card);
            _cardInventory[idx] = EmptyCard;
        }
    }

    public void AddCard(CardSO card) {
        var idx = FindEmpty();
        if (idx != -1) {
            _cardInventory[idx] = card;
        } else {
            _cardQueue.Enqueue(card);
        }
        RefillUI();
    }

    public void SwitchUiState() {
        CardUi.enabled = !CardUi.enabled;
        if (CardUi.enabled) {
            RefillUI();
        } else {
            EmptyUI();
        }
    }

    public void Disable() {
        CardUi.enabled = false;
        EmptyUI();
        errorMsg.enabled = false;
    }
}