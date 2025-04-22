using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public static class TextGetterUrl
{
    private static readonly string url = "https://raw.githubusercontent.com/glazunov/my_quize/refs/heads/main/Assets/Scripts/Questions.txt";
    static readonly HttpClient client = new HttpClient();

    public static async Task<string> GetText()
    {
        string responseBody = "";
        try
        {
            responseBody = await client.GetStringAsync(url);
//            Debug.Log(responseBody);
        }
        catch (HttpRequestException e)
        {
            Debug.Log("\nException Caught!");
            Debug.Log("Message :{0} " + e.Message);
        }

        return responseBody;
    }

   
    
}