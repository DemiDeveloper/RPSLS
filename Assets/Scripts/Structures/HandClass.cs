using System.Collections.Generic;
using System;
using UnityEngine;
namespace RPSLS
{
    [Serializable]
    public class HandClass
    {
        public Sprite handSprite;
        public Sprite bGSprite;
        public HandRelationDictionary handRelation;
        public string handDisplayName;
    }
    [Serializable]
    public class HandClassDictionary : SerializableDictionary<HandType, HandClass>
    {
        public Sprite GetHandSprite(HandType handType) 
        {
            if (this.ContainsKey(handType))
            {
                return this[handType].handSprite;
            }
            return null;
        }
        public Sprite GetBGSprite(HandType handType)
        {
            if (this.ContainsKey(handType))
            {
                return this[handType].bGSprite;
            }
            return null;
        }
        public string GetWinString(HandType firstHandType, HandType secondHandType, out GameResult result)
        {
            GameResult winresult = GameResult.Draw;
            string winString = "Draw";
            if (this.ContainsKey(firstHandType))
            {
                if (this[firstHandType].handRelation.ContainsKey(secondHandType))
                {
                    winresult = GameResult.Win;
                    winString = this[firstHandType].handRelation[secondHandType];
                }
                else
                {
                    if (this.ContainsKey(secondHandType))
                    {
                        if (this[secondHandType].handRelation.ContainsKey(firstHandType))
                        {
                            winresult = GameResult.Lose;
                            winString = this[secondHandType].handRelation[firstHandType];
                        }
                    }
                }
            }
            result = winresult;
            return winString;
        }
        public HandType GetRandomHand()
        {
            int rInt = UnityEngine.Random.Range(0, this.Count);
            foreach(var kpv in this)
            {
                rInt--;
                if (rInt <= 0)
                {
                    return kpv.Key;
                }
            }
            return HandType.Rock;
        }
        public HandClass FetchHandClassData(HandType handType)
        {
            if (this.ContainsKey(handType))
            {
                return this[handType];
            }
            return null;
        }
    }
    [Serializable]
    public class HandRelationDictionary : SerializableDictionary<HandType,string>
    {

    }

    [Serializable]
    public class ResultColorDictionary : SerializableDictionary<GameResult, Color>
    {

    }
}