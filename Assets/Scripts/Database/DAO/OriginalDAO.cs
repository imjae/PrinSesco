public class OriginalDAO
{
    public DynamoDBManager ddbm;

    public OriginalDAO()
    {
        ddbm = DynamoDBManager.Instance;
    }
}
