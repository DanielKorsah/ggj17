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
    //string jFilePath = Path.Combine(Application.streamingAssetsPath, "PersistentData.Json");
    //string bFilePath = Path.Combine(Application.streamingAssetsPath, "PersistentData.dat");
    string jFilePath = Path.Combine(Application.streamingAssetsPath, "PersistentData.Json");
    string bFilePath = Path.Combine(Application.streamingAssetsPath, "PersistentData.dat");

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
        if (!File.Exists(jFilePath))
        {
            Debug.Log("json created");
            File.Create(jFilePath);
        }
        string jsonOut = JsonConvert.SerializeObject(data);
        File.WriteAllText(jFilePath, jsonOut);
        Debug.Log(jFilePath);
        Debug.Log("Change filepath to Application.persistentDataPath before release!");
    }

    public void BWrite()
    {
        if (!File.Exists(bFilePath))
        {
            Debug.Log("bson created");
            File.Create(bFilePath);
        }
        FileStream fs = File.Open(bFilePath, FileMode.Create);
        BsonWriter bWriter = new BsonWriter(fs);
        serializer.Serialize(bWriter, data);
        Debug.Log(bFilePath);
        Debug.Log("Change filepath to Application.persistentDataPath before release!");
    }

    public Data JRead()
    {
        string jsonIn = File.ReadAllText(jFilePath);

        Debug.Log(jFilePath);
        Debug.Log("Change filepath to Application.persistentDataPath before release!");

        return data = JsonConvert.DeserializeObject<Data>(jsonIn);
    }

    public Data BRead()
    {
        byte[] binaryIn = File.ReadAllBytes(bFilePath);

        MemoryStream binaryStream = new MemoryStream(binaryIn);
        BsonReader bReader = new BsonReader(binaryStream);
        JsonSerializer serializer = new JsonSerializer();

        Debug.Log(bFilePath);
        Debug.Log("Change filepath to Application.persistentDataPath before release!");

        return data = serializer.Deserialize<Data>(bReader);
    }

}