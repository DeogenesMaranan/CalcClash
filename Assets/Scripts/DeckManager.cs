using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {
    public GameObject cardPrefab;
    public Transform deckParent;
    private List<Card> deck = new List<Card>();

    public bool IsDeckEmpty() => deck.Count == 0;

    void Start() {
        InitializeDeck();
        ShuffleDeck();
    }

    void InitializeDeck() {
        for (int i = 1; i <= 10; i++) {
            for (int j = 0; j < 3; j++) {
                CreateCard(Card.CardType.Number, i.ToString(), i);
            }
        }

        string[] operators = { "+", "-", "*", "/" };
        foreach (string op in operators) {
            for (int j = 0; j < 4; j++) {
                CreateCard(Card.CardType.Operator, op, 0);
            }
        }
    }

    void CreateCard(Card.CardType type, string value, int numberValue) {
        GameObject cardGO = Instantiate(cardPrefab, deckParent);
        Card card = cardGO.GetComponentInChildren<Card>(true); 
        card.Initialize(type, value, numberValue);
        deck.Add(card);
    }

    public void ShuffleDeck() {
        for (int i = deck.Count - 1; i > 0; i--) {
            int j = Random.Range(0, i + 1);
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    public Card DrawCard() {
        if (deck.Count == 0) return null;
        Card drawnCard = deck[0];
        deck.RemoveAt(0);
        return drawnCard;
    }

    public void AppendNewDeck()
    {
        InitializeDeck();
        ShuffleDeck();
    }
}