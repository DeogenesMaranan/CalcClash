using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour {
    [SerializeField] private Text cardValueTextSmall;
    [SerializeField] private Text cardValueTextBig;
    [HideInInspector] public GameObject rootCardObject;
    private GameObject _rootObject;
    public enum CardType { Number, Operator }
    public CardType type;
    public int numberValue;
    public string operatorValue;

    public void Initialize(CardType newType, string value, int numValue) {
        rootCardObject = transform.parent.gameObject;
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

    public GameObject RootObject => _rootObject;
}