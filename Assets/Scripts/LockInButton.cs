using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Text;
using System;
using System.Collections.Generic;

public class LockInButton : MonoBehaviour
{
    [SerializeField] private PlayAreaManager playAreaManager;
    [SerializeField] private Button button;
    
    private DataTable table = new DataTable();

    void Awake() => button.onClick.AddListener(CalculateOperation);

    void CalculateOperation()
    {
        var playedCards = playAreaManager.GetPlayedCards();

        if (playedCards.Count == 0)
        {
            GameManager.Instance.ProcessResult(0f, "0");
            return;
        }

        if (playedCards.Count == 1)
        {
            Card singleCard = playedCards[0].GetComponentInChildren<Card>();
            GameManager.Instance.ProcessResult(singleCard.numberValue, singleCard.numberValue.ToString());
            playAreaManager.ClearPlayArea();
            return;
        }

        try
        {
            string expression = BuildExpression(playedCards);
            double result = Convert.ToDouble(table.Compute(expression, ""));
            GameManager.Instance.ProcessResult((float)result, expression);
            playAreaManager.ClearPlayArea();
        }
        catch (DivideByZeroException)
        {
            GameManager.Instance.ProcessResult(0f, "Division by Zero");
            playAreaManager.ClearPlayArea();
        }
    }


    string BuildExpression(List<GameObject> cards)
    {
        StringBuilder sb = new StringBuilder();
        foreach (GameObject cardGO in cards)
        {
            Card card = cardGO.GetComponentInChildren<Card>();
            sb.Append(card.type == Card.CardType.Number ? 
                     card.numberValue.ToString() : 
                     card.operatorValue);
        }
        return sb.ToString();
    }
}