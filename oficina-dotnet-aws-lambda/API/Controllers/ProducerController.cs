using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[Route("api/[controller]")]
public class ProducerController : ControllerBase
{
    
    [HttpPost]
    public async Task Post([FromBody] RequestModel request)
    {
        try
        {
            request.OrderId = Guid.NewGuid();

            AmazonSQSClient client = new AmazonSQSClient(
                "", 
                "", 
                RegionEndpoint.USEast1);

            var response = await client.SendMessageAsync
                (
                    "https://sqs.us-east-1.amazonaws.com/117557792929/oficina-queue",
                    JsonSerializer.Serialize(request)
                );

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"falha ao enviar para a fila: {response.HttpStatusCode}");

            Ok(response);
        }
        catch (Exception ex) 
        { 
            BadRequest(ex);
        }


    }

}