using Amazon.DynamoDBv2.DataModel;

using UnityEngine;

public class KeyTable : MonoBehaviour
{
    [DynamoDBHashKey]
    public string PK { get; set; }

    [DynamoDBRangeKey]
    public string SK { get; set; }
}