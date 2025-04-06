using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Text;
using System;
using System.Collections.Generic;

public class LockInButton : MonoBehaviour
{
    [SerializeField] private PlayAreaManager playAreaManager;
    [SerializeField] private Text resultText;
    [SerializeField] private Button button;
    
    private DataTable table = new DataTable();

    void Awake() => button.onClick.AddListener(CalculateOperation);

    void CalculateOperation()
    {
        var playedCards = playAreaManager.GetPlayedCards();
        
        if (playedCards.Count == 1)
        {
            Card singleCard = playedCards[0].GetComponentInChildren<Card>();
            ShowResult(singleCard.numberValue.ToString("F2"));
            return;
        }

        try
        {
            string expression = BuildExpression(playedCards);
            double result = Convert.ToDouble(table.Compute(expression, ""));
            ShowResult(result.ToString("F2"));
        }
        catch (DivideByZeroException)
        {
            ShowResult("0");
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

    void ShowResult(string message)
    {
        resultText.color = Color.green;
        resultText.text = $"Result: {message}";
        playAreaManager.ClearPlayArea();
    }
}