using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Consumer.Models;
using System.Text.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Consumer;

public class Function
{
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {

    }


    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach(var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        try
        {

            var messageQueue = JsonSerializer.Deserialize<RequestModel>(message.Body);

            if (messageQueue is null)
                throw new NullReferenceException("Mensagem vazia");

            if (messageQueue.OrderPrice < 0)
                throw new ArgumentOutOfRangeException("Preço do pedido não pode ser menor que zero");

            var dynamoClient = new AmazonDynamoDBClient(
                "",
                "",
                RegionEndpoint.USEast1);

            var dynamoContext = new DynamoDBContext(dynamoClient);

            await dynamoContext.SaveAsync(messageQueue);

            context.Logger.LogInformation($"Processed message {message.Body}");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            context.Logger.LogInformation($"Error: {ex.Message}");
            throw new Exception(ex.Message);
        }
    }
}