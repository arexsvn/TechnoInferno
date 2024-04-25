using Newtonsoft.Json;
using System;
using UnityEngine;

public class NewtonsoftJsonSerializationOption : ISerializationOption
{
    public string ContentType => "application/json";

    public T Deserialize<T>(string text)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(text);
            Debug.Log($"Success: {text}");
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not DeserializeObject {text}. {ex.Message}");
            return default;
        }
    }

    public string Serialize<T>(T value)
    {
        try
        {
            var result = JsonConvert.SerializeObject(value);
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not serialize object. {ex.Message}");
            return default;
        }
    }
}
