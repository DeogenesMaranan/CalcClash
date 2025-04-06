using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Splines;

public class HandManager : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private int initialHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private PlayAreaManager playAreaManager;
    [SerializeField] private Transform handParent;
    private List<GameObject> handCards = new();

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space)) DrawCard();
    }

    void Start() {
        StartCoroutine(DrawInitialHandWithDelay());
    }

    private IEnumerator DrawInitialHandWithDelay() {
        for (int i = 0; i < initialHandSize; i++) {
            DrawCard();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return;
        
        Card drawnCard = deckManager.DrawCard();
        if (drawnCard == null) return;
        
        GameObject rootCardGO = drawnCard.rootCardObject;
        rootCardGO.transform.SetParent(handParent);
        rootCardGO.transform.position = spawnPoint.position;
        rootCardGO.transform.rotation = spawnPoint.rotation;
        rootCardGO.SetActive(true);
        
        handCards.Add(rootCardGO);
        
        Button cardButton = drawnCard.GetComponent<Button>();
        cardButton.onClick.AddListener(() => OnCardClicked(rootCardGO));
        
        UpdateCardPositions();
    }

    public void OnCardClicked(GameObject rootCardGO) {
        if (handCards.Contains(rootCardGO)) {
            Card card = rootCardGO.GetComponentInChildren<Card>();
            card.GetComponent<Button>().onClick.RemoveAllListeners();
            
            bool isCardAdded = playAreaManager.AddCardToPlayArea(rootCardGO);
            if (isCardAdded) {
                handCards.Remove(rootCardGO);
                UpdateCardPositions();
            }
        }
    }

    public void ReturnCardToHand(GameObject card)
    {
        if (!handCards.Contains(card))
        {
            card.transform.SetParent(handParent);
            handCards.Add(card);
            UpdateCardPositions();
        }
    }

    private void UpdateCardPositions(){
        if (handCards.Count == 0) return;
        float cardSpacing = 1f/ maxHandSize;
        float firstCardPosition = 0.5f - (handCards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < handCards.Count; i++){
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = splineContainer.transform.TransformPoint(spline.EvaluatePosition(p));
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Vector3 finalPosition = splinePosition + new Vector3(0, 0, -i * 0.01f);
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            handCards[i].transform.DOMove(finalPosition, 0.25f);
            handCards[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }
}
