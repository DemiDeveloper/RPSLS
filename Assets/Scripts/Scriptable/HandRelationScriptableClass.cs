using UnityEngine;
namespace RPSLS
{
    [CreateAssetMenu(fileName = "HandRelationScriptableClass", menuName = "Scriptable Objects/HandRelationScriptableClass")]
    public class HandRelationScriptableClass : ScriptableObject
    {
        public HandClassDictionary handClassDictionary;
    }
}