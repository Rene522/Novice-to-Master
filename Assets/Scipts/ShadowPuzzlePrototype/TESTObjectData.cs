using UnityEngine;

public class TESTObjectData : ScriptableObject
{
    [CreateAssetMenu(menuName = "Prototype/TESTObjectData")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public GameObject placedPrefab;
    }
}
