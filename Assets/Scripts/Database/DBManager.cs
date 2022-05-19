using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

public class DynamoDBManager : Singleton<DynamoDBManager>
{
    CognitoAWSCredentials credentials;

    public DynamoDBContext context;
    public AmazonDynamoDBClient client;

    public override void Awake()
    {
        base.Awake();

        UnityInitializer.AttachToGameObject(this.gameObject);
        //credentials = new CognitoAWSCredentials(Env.AWS_COGNITO_KEY, RegionEndpoint.APNortheast2);

        client = new AmazonDynamoDBClient(credentials, RegionEndpoint.APNortheast2);
        context = new DynamoDBContext(client);
    }
}
