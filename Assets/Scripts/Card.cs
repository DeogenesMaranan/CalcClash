using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour {
    [SerializeField] private Text cardValueTextSmall;
    [SerializeField] private Text cardValueTextBig;
    public enum CardType { Number, Operator }
    public CardType type;
    public int numberValue;
    public string operatorValue;

    // private bool isInPlayArea = false;

    public void Initialize(CardType newType, string value, int numValue) {
        type = newType;
        numberValue = numValue;
        operatorValue = value;
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay() {
        string value = type == CardType.Number ? numberValue.ToString() : operatorValue;
        cardValueTextSmall.text = value;
        cardValueTextBig.text = value;
    }


    // public void OnPointerClick(PointerEventData eventData) {
    //     if (isInPlayArea) {
    //         ReturnToHand();
    //     } else {
    //         MoveToPlayArea();
    //     }
    // }

    // void MoveToPlayArea() {
    //     Transform playArea = DropZone.Instance.transform;
    //     transform.SetParent(playArea);
    //     transform.localPosition = Vector3.zero;
    //     isInPlayArea = true;
    //     DropZone.Instance.AddCard(this);
    // }

    // public void ReturnToHand() {
    //     Transform hand = HandManager.Instance.handParent;
    //     transform.SetParent(hand);
    //     transform.localPosition = Vector3.zero;
    //     isInPlayArea = false;
    //     DropZone.Instance.RemoveCard(this);
    // }
}