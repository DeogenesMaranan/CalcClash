using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;
using UnityEngine.UI;
using System.Collections;

public class PlayAreaManager : MonoBehaviour
{
    [SerializeField] private int maxPlayAreaSize;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private HandManager handManager;
    [SerializeField] private Transform handParent;
    public Transform playAreaParent;
    private List<GameObject> playedCards = new List<GameObject>();
    private Vector3 originalCardScale = Vector3.one;
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();
     
    public bool AddCardToPlayArea(GameObject card) {
        if(playedCards.Count >= maxPlayAreaSize) return false;
        
        Card cardComponent = card.GetComponentInChildren<Card>();
        Button button = cardComponent.GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ReturnAllCardsToHand());
        button.enabled = true;
        
        Vector3 currentScale = card.transform.localScale;
        originalScales[card] = currentScale;
        card.transform.DOScale(currentScale * 0.8f, 0.25f);
        
        playedCards.Add(card);
        UpdatePlayAreaPositions();

        return true;
    }
    public void ReturnAllCardsToHand()
    {
        StartCoroutine(ReturnCardsRoutine());
    }
    private IEnumerator ReturnCardsRoutine()
        {
            for (int i = playedCards.Count - 1; i >= 0; i--)
            {
                GameObject card = playedCards[i];
                card.transform.SetParent(handParent);

                if (originalScales.TryGetValue(card, out Vector3 originalScale))
                {
                    card.transform.DOScale(originalScale, 0.25f);
                    originalScales.Remove(card);
                }
                
                Card cardComponent = card.GetComponentInChildren<Card>();
                if (cardComponent != null)
                {
                    Button button = cardComponent.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    
                    button.onClick.AddListener(() => handManager.OnCardClicked(card));
                }
                
                handManager.ReturnCardToHand(card);
                playedCards.RemoveAt(i);
                
                yield return new WaitForSeconds(0.1f);
            }
            
            UpdatePlayAreaPositions();
        }

    private void UpdatePlayAreaPositions()
    {
        if (playedCards.Count == 0) return;
        float cardSpacing = 1f/ maxPlayAreaSize;
        float firstCardPosition = 0.5f - (playedCards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < playedCards.Count; i++)
        {   
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = splineContainer.transform.TransformPoint(spline.EvaluatePosition(p));
            Vector3 finalPosition = splinePosition + new Vector3(0, 0, -i * 0.01f);
            playedCards[i].transform.DOMove(finalPosition, 0.25f);
            playedCards[i].transform.DOLocalRotateQuaternion(Quaternion.identity, 0.25f);
        }
    }
}