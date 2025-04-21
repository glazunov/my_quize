using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public static class TextGetterUrl
{
    private static readonly string url = "https://storage.yandexcloud.net/historygarry/Texts/STORYID.txt";
    static readonly HttpClient client = new HttpClient();

    public static async Task<string> GetText(string id)
    {
        string responseBody = "";
        try
        {
            responseBody = await client.GetStringAsync(url.Replace("STORYID", id).Replace(" ", "%20"));
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