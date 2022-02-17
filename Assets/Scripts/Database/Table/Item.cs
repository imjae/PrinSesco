using Amazon.DynamoDBv2.DataModel;

using System.Collections.Generic;

[DynamoDBTable("PrinSesco")]
public class Item : KeyTable
{
    public new string name { get; set; }
    public int amount { get; set; }
    public int attack { get; set; }
    public int defense { get; set; }
    public int speed { get; set; }
    public int value { get; set; }
}