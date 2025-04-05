using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {
    public GameObject cardPrefab;
    public Transform deckParent;
    private List<Card> deck = new List<Card>();

    void Start() {
        InitializeDeck();
        ShuffleDeck();
    }

    void InitializeDeck() {
        if (cardPrefab == null) {
            Debug.LogError("Card Prefab not assigned in DeckManager!");
            return;
        }
        for (int i = 1; i <= 10; i++) {
            for (int j = 0; j < 3; j++) {
                CreateCard(Card.CardType.Number, i.ToString(), i);
            }
        }

        string[] operators = { "+", "-", "*", "/" };
        foreach (string op in operators) {
            for (int j = 0; j < 3; j++) {
                CreateCard(Card.CardType.Operator, op, 0);
            }
        }
    }

    void CreateCard(Card.CardType type, string value, int numberValue) {
        GameObject cardGO = Instantiate(cardPrefab, deckParent);
        Card card = cardGO.GetComponentInChildren<Card>(true); 
        if (card == null) {
            Debug.LogError("Card component missing on prefab!");
            Destroy(cardGO);
            return;
        }
        card.type = type;
        card.numberValue = numberValue;
        card.operatorValue = value;
        card.UpdateCardDisplay();
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
}