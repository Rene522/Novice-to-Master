using UnityEngine;

public class ItemType : MonoBehaviour
{
    public Types type;
    public string str;

    public string ReturnText()
    { return str; }

    public Types GetTypes() 
    { return type; }
}


public enum Types
{ 
    Eye,
    BlindEye,
    Player,
    Apple,
    Orange,
    Wall
}