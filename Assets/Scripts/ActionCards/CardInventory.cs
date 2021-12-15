using System;
using System.Collections.Generic;
using ScriptableObjects.CardObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ActionCards
{
    internal class Card
    {
        public CardScript Script { get; set; }
        public Button Button { get; set; }

        public Card(CardScript script)
        {
            Script = script;
            Button = script.GetComponentInChildren<Button>();
        }
    }

    public class CardInventory : MonoBehaviour
    {
        private List<Card> _cards;
        [SerializeField] private TextMeshProUGUI errorMsg;

        [FormerlySerializedAs("CardUi")] [SerializeField]
        private Canvas cardUi;

        [FormerlySerializedAs("EmptyCard")] [SerializeField]
        private CardObject emptyCard;

        private List<CardObject> _cardInventory;
        private Queue<CardObject> _cardQueue;

        private void Start()
        {
            var scripts = cardUi.GetComponentsInChildren<CardScript>();
            _cards = new List<Card>();
            _cardInventory = new List<CardObject>();

            _cardQueue = new Queue<CardObject>();
            foreach (var script in scripts)
            {
                _cards.Add(new Card(script));
                _cardInventory.Add(emptyCard);
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
        _cards.ForEach(c => c.Script.Backend = emptyCard);
        _cards.ForEach(c => c.Button.onClick.AddListener(() => { c.Script.Backend.action?.Invoke(GameManager.Instance.Player); }));
    }

    private int FindEmpty() {
        foreach (var card in _cardInventory) {
            if (card.Equals(emptyCard))
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
                CardObject tmp = c.Script.Backend;
                DiscardACard(c);
                tmp.action?.Invoke(GameManager.Instance.Player);
            });
        });
    }
    private void DiscardACard(Card card) {
        if (!card.Script.Backend.Equals(emptyCard)) {
            CardDealer.Instance.AddToDiscard(card.Script.Backend);
            card.Script.Backend = emptyCard;

            int idx = _cards.IndexOf(card);
            _cardInventory[idx] = emptyCard;
        }
    }

        public void AddCard(CardObject card)
        {
            var idx = FindEmpty();
            if (idx != -1)
            {
                _cardInventory[idx] = card;
            }
            else
            {
                _cardQueue.Enqueue(card);
            }

            RefillUI();
        }

        public void SwitchUiState()
        {
            cardUi.enabled = !cardUi.enabled;
            if (cardUi.enabled)
            {
                RefillUI();
            }
            else
            {
                EmptyUI();
            }
        }

        public void Disable()
        {
            cardUi.enabled = false;
            EmptyUI();
            errorMsg.enabled = false;
        }
    }
}