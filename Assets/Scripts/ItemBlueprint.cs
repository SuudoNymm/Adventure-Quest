using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class ItemBlueprint
{
    public string itemName;

    public string Req1;

    public string Req2;

    public int Req1Amount;

    public int Req2Amount;

    public int numOfRequirments;

    public ItemBlueprint(string name, string req1, int req1Amount, string req2, int req2Amount, int numOfReq)
    {
        itemName = name;
        Req1 = req1;
        Req2 = req2;
        Req1Amount = req1Amount;
        Req2Amount = req2Amount;
        numOfRequirments = numOfReq;
    }
}
