using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

public class DynamoDBManager : Singleton<DynamoDBManager>
{
    CognitoAWSCredentials credentials;

    public DynamoDBContext context;
    public AmazonDynamoDBClient client;

    public override void Awake()
    {
        base.Awake();

        UnityInitializer.AttachToGameObject(this.gameObject);

        // Env가 없어서 컴파일 오류가 나길래 임시 주석처리했습니다!... -시온-
        //credentials = new CognitoAWSCredentials(Env.AWS_COGNITO_KEY, RegionEndpoint.APNortheast2);

        client = new AmazonDynamoDBClient(credentials, RegionEndpoint.APNortheast2);
        context = new DynamoDBContext(client);
    }
}
