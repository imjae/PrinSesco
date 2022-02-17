using Amazon.DynamoDBv2.DataModel;

public class KeyTable
{
    [DynamoDBHashKey]
    public string PK { get; set; }

    [DynamoDBRangeKey]
    public string SK { get; set; }
}