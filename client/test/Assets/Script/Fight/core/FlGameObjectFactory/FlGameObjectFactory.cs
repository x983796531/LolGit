using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameProtocol.dto.fight;
using System.Xml;

public class FlGameObjectFactory : MonoBehaviour {
    public static FlGameObjectFactory instance;
    public Dictionary<int, FlGameObject> allFlGameObject= new Dictionary<int, FlGameObject>();
    public Dictionary<int, string> gameObjectProducerDic = new Dictionary<int, string>();
    private void Awake()
    {
        instance = this;
    }

    void LoadProducerConfig()
    {
        XmlDocument doc = new XmlDocument();
        
        string path = Application.streamingAssetsPath + "/ProducerConfig.xml";
        doc.Load(path);
        XmlNode root = doc.SelectSingleNode("root");
        XmlNodeList nodeList = root.ChildNodes;

        foreach(XmlNode node in nodeList)
        {
            int modelId = int.Parse(node.Attributes["modelId"].Value);
            string producerClass = node.Attributes["producerClass"].Value;
            gameObjectProducerDic.Add(modelId, producerClass);

        }

    }

    public void GenerateFlGameObject(FightRoomModel room)
    {
        int myTeam = -1;

        foreach (AbsFightModel item in room.teamOne)
        {
            if (item.id == GameData.User.id)
                myTeam = item.team;
        }
        if (myTeam == -1)
        {
            foreach (AbsFightModel item in room.teamTwo)
            {
                if (item.id == GameData.User.id)
                    myTeam = item.team;
            }
        }



    }






}
