using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace RPSLS
{
    public class HandObjectScript : MonoBehaviour
    {
        public HandType mytType;
        public Image handImage;
        public Image bgImage;
        public Image bgSelectImage;
        public TextMeshProUGUI handName;
        public Button myButton;
        public float Speed;
        Coroutine ShowHideCur;
        void Start()
        {

        }
        public void SetValues(HandClass data, HandType hand)
        {
            mytType = hand;
            handImage.sprite = data.handSprite;
            bgImage.sprite = data.bGSprite;
            handName.text = data.handDisplayName;
        }
        public void OnHandClick()
        {
            GameController.HandClicked?.Invoke(mytType);
        }
        public void ToggleButton(bool setTo)
        {
            myButton.interactable = setTo;
        }
        public void SetSelected(bool setTo)
        {
            bgSelectImage.enabled = setTo;
        }
        public void AIShowHand(HandClass hand)
        {
            if (ShowHideCur != null)
            {
                StopCoroutine(ShowHideCur);
            }
            ShowHideCur = StartCoroutine(ShowHideHand(hand));
        }
        public void HideAIHand()
        {

            if (ShowHideCur != null)
            {
                StopCoroutine(ShowHideCur);
            }
            ShowHideCur = StartCoroutine(ShowHideHand());
        }
        IEnumerator ShowHideHand(HandClass hand = null)
        {
            while (myButton.transform.localScale.x > 0)
            {
                myButton.transform.localScale -= new Vector3(Time.deltaTime * Speed, 0, 0);
                yield return new WaitForEndOfFrame();
            }
            if(hand != null)
            {
                handImage.enabled = true;
                handImage.sprite = hand.handSprite;
                handName.text = hand.handDisplayName;
            }
            else
            {
                handImage.enabled = false;
                handName.text = "?";
            }
            while (myButton.transform.localScale.x < 1)
            {
                myButton.transform.localScale += new Vector3(Time.deltaTime * Speed, 0, 0);
                yield return new WaitForEndOfFrame();
            }
            myButton.transform.localScale = Vector3.one;
        }
    }
}