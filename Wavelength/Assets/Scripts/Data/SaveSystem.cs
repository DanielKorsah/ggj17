using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;
using System.IO;
using System;

public class SaveSystem
{
    private static SaveSystem instance = null;
    Data data;
    JsonSerializer serializer = new JsonSerializer();
    string jFilePath = Path.Combine(Application.persistentDataPath, "PlayerInfo.Json");
    string bFilePath = Path.Combine(Application.persistentDataPath, "PlayerInfo.dat");

    private SaveSystem()
    {
        data = new Data();
    }

    public static SaveSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveSystem();
            }
            return instance;
        }
    }

    public Data Data
    {
        get { return data; }
        set { value = data; }
    }

    public void JWrite()
    {
        string jOut = JsonConvert.SerializeObject(data);
        File.WriteAllText(jFilePath, jOut);
    }

    public void BWrite()
    {
        FileStream fs = File.Open(bFilePath, FileMode.Create);
        BsonWriter bWriter = new BsonWriter(fs);
        serializer.Serialize(bWriter, data);
    }

    public Data JRead()
    {
        string jIn = File.ReadAllText(jFilePath);
        return data = JsonConvert.DeserializeObject<Data>(jIn);
    }

    public Data BRead()
    {
        byte[] bIn = Convert.FromBase64String("MQAAAAJOYW1lAA8AAABNb3ZpZSBQcmVtaWVyZQAJU3RhcnREYXRlAMDgKWE8AQAAAA==");

        MemoryStream stream = new MemoryStream(bIn);
        BsonReader bReader = new BsonReader(stream);
        JsonSerializer serializer = new JsonSerializer();
        return data = serializer.Deserialize<Data>(bReader);
    }

}