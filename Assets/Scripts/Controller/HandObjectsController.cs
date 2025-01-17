using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
namespace RPSLS {
    public class HandObjectsController : MonoBehaviour
    {
        public GameObject handPrefab;
        public HandObjectScript oppnentHand;
        public Transform playerObjectParent;

        public Dictionary<HandType, HandObjectScript> playerHandObjects;
        void Start()
        {

        }
        void Update()
        {

        }
        public void PopulatePlayerHands(HandClassDictionary data, bool clearData = false)
        {
            playerHandObjects = new Dictionary<HandType, HandObjectScript>();
            playerHandObjects.Clear();
            if (clearData)
            {
                foreach (Transform tr in playerObjectParent)
                {
                    tr.gameObject.SetActive(false);
                }
            }
            int count = 0;
            foreach(var kpv in data)
            {
                HandObjectScript handObject = null;
                if (count < playerObjectParent.childCount)
                {
                    handObject = playerObjectParent.GetChild(count).GetComponent<HandObjectScript>();
                    handObject.gameObject.SetActive(true);
                }
                else
                {
                    handObject = Instantiate(handPrefab).GetComponent<HandObjectScript>();
                    handObject.transform.SetParent(playerObjectParent, false);
                }
                handObject.SetValues(kpv.Value, kpv.Key);
                playerHandObjects.Add(kpv.Key, handObject);
                count++;
            }
        }
        public void ResetButtons()
        {
            foreach (KeyValuePair<HandType, HandObjectScript> kpv in playerHandObjects)
            {
                kpv.Value.ToggleButton(true);
                kpv.Value.SetSelected(false);
            }
            oppnentHand.HideAIHand();
        }
        public void HandButtonClick(HandType hand, HandClass opponentHandClass)
        {
            DisableHandButtons();
            if (playerHandObjects.ContainsKey(hand))
            {
                playerHandObjects[hand].SetSelected(true);
            }
            oppnentHand.AIShowHand(opponentHandClass);
        }
        public void DisableHandButtons()
        {
            foreach (KeyValuePair<HandType, HandObjectScript> kpv in playerHandObjects)
            {
                kpv.Value.ToggleButton(false);
            }
        }
    }
}