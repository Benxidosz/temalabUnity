using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.CardObjects;
using Unity.Netcode;
using UnityEngine;

namespace ActionCards
{
    public class CardDealer : NetworkBehaviour
    {
        internal enum CardColor
        {
            Blue,
            Green,
            Yellow
        }

        private class Deck
        {
            private List<CardObject> _deck = new List<CardObject>();
            private readonly List<CardObject> _discardDeck = new List<CardObject>();

            public Deck(List<CardObject> deck)
            {
                _deck.AddRange(deck);
                Utilities.ShuffleList(_deck);
            }

            public CardObject GetNextCard()
            {
                return _deck[0];
            }

            public void RemoveTopCard()
            {
                _deck.RemoveAt(0);
            }

            public int DeckCount => _deck.Count;
            public int DiscardDeckCount => _discardDeck.Count;

            public void Reshuffle(List<int> newOrder)
            {
                _deck = newOrder.Select(index => _discardDeck[index]).ToList();
                _discardDeck.Clear();
            }

            public void AddToDiscard(CardObject card)
            {
                if (_deck.Count == 0)
                {
                    _deck.Add(card);
                }
                else
                {
                    _discardDeck.Add(card);
                }
            }
        }

        [SerializeField] private List<CardObject> blueCardSos;
        [SerializeField] private List<CardObject> greenCardSos;
        [SerializeField] private List<CardObject> yellowCardSos;

        private Deck _yellowDeck;
        private Deck _greenDeck;
        private Deck _blueDeck;

        public static CardDealer Instance { get; private set; }

        private Deck GetColorDeck(CardColor color)
        {
            Deck deck;
            switch (color)
            {
                case CardColor.Blue:
                    deck = _blueDeck;
                    break;
                case CardColor.Green:
                    deck = _greenDeck;
                    break;
                default:
                    deck = _yellowDeck;
                    break;
            }

            return deck;
        }

        [ServerRpc]
        private void InitDecksServerRpc()
        {
            var blueDeckOrder = Enumerable.Range(0, blueCardSos.Count).ToList();
            var greenDeckOrder = Enumerable.Range(0, greenCardSos.Count).ToList();
            var yellowDeckOrder = Enumerable.Range(0, yellowCardSos.Count).ToList();
            Utilities.ShuffleList(blueDeckOrder);
            Utilities.ShuffleList(greenDeckOrder);
            Utilities.ShuffleList(yellowDeckOrder);
            InitDecksClientRpc(blueDeckOrder.ToArray(), greenDeckOrder.ToArray(), yellowDeckOrder.ToArray());
        }

        [ClientRpc]
        private void InitDecksClientRpc(int[] blueDeckOrder, int[] greenDeckOrder, int[] yellowDeckOrder)
        {
            var blueDeck = blueDeckOrder.Select(index => blueCardSos[index]).ToList();
            var greenDeck = greenDeckOrder.Select(index => greenCardSos[index]).ToList();
            var yellowDeck = yellowDeckOrder.Select(index => yellowCardSos[index]).ToList();
            _blueDeck = new Deck(blueDeck);
            _greenDeck = new Deck(greenDeck);
            _yellowDeck = new Deck(yellowDeck);
        }
        
        private void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void Initialize()
        {
            if (IsServer)
            {
                InitDecksServerRpc();
            }

            Debug.Log("Initialized");
        }

        [ServerRpc]
        private void RemoveTopCardServerRpc(CardColor color)
        {
            RemoveTopCardClientRpc(color);
        }

        [ClientRpc]
        private void RemoveTopCardClientRpc(CardColor color)
        {
            GetColorDeck(color).RemoveTopCard();
        }

        [ServerRpc]
        private void RemoveTopCardAndReshuffleServerRpc(CardColor color, int remainingCount)
        {
            var order = Enumerable.Range(0, remainingCount).ToList();
            Utilities.ShuffleList(order);
            RemoveTopCardAndReshuffleClientRpc(color, order.ToArray());
        }

        [ClientRpc]
        private void RemoveTopCardAndReshuffleClientRpc(CardColor color, int[] newOrder)
        {
            var deck = GetColorDeck(color);
            deck.RemoveTopCard();
            deck.Reshuffle(newOrder.ToList());
            Debug.Log("Reshuffled");
        }
        
        private CardObject GetNextCard(CardColor color)
        {
            var deck = GetColorDeck(color);
            if (deck.DeckCount == 0)
                return null;

            var card = deck.GetNextCard();
            if (deck.DeckCount <= 1)
            {
                RemoveTopCardAndReshuffleServerRpc(color, deck.DiscardDeckCount);
            }
            else
            {
                RemoveTopCardServerRpc(color);
            }

            return card;
        }

        public CardObject NextBlueCard => GetNextCard(CardColor.Blue);
        public CardObject NextGreenCard => GetNextCard(CardColor.Green);
        public CardObject NextYellowCard => GetNextCard(CardColor.Yellow);

        [ServerRpc]
        private void AddCardToDiscardServerRpc(CardColor color, string cardName)
        {
            AddCardToDiscardClientRpc(color, cardName);
        }

        [ClientRpc]
        private void AddCardToDiscardClientRpc(CardColor color, string cardName)
        {
            CardObject card;
            switch (color)
            {
                case CardColor.Blue:
                    card = blueCardSos.Find(c => c.name == cardName);
                    break;
                case CardColor.Green:
                    card = greenCardSos.Find(c => c.name == cardName);
                    break;
                case CardColor.Yellow:
                    card = yellowCardSos.Find(c => c.name == cardName);
                    break;
                default:
                    return;
            }
            GetColorDeck(color).AddToDiscard(card);
        }
        
        public void AddToDiscard(CardObject card)
        {
            Debug.Log($"Discard {card.name}");
            if (blueCardSos.Contains(card))
            {
                AddCardToDiscardServerRpc(CardColor.Blue, card.name);
            }
            else if (greenCardSos.Contains(card))
            {
                AddCardToDiscardServerRpc(CardColor.Green, card.name);
            }
            else if (yellowCardSos.Contains(card))
            {
                AddCardToDiscardServerRpc(CardColor.Yellow, card.name);
            }
        }

        private void OnGUI()
        {
            if (IsServer)
            {
                if (GUI.Button(new Rect(10, 200, 150, 30), "Initialize decks"))
                {
                    Initialize();
                }
            }

            if (IsClient || IsHost)
            {
                if (GUI.Button(new Rect(10, 240, 150, 30), "Take top blue"))
                {
                    Debug.Log(NextBlueCard);
                }

                if (GUI.Button(new Rect(10, 280, 150, 30), "Take top green"))
                {
                    Debug.Log(NextGreenCard);
                }

                if (GUI.Button(new Rect(10, 320, 150, 30), "Take top yellow"))
                {
                    Debug.Log(NextYellowCard);
                }
            }
        }
    }
}