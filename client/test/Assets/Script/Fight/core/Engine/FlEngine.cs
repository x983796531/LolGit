

using GameProtocol.dto.fight;
using System.Collections.Generic;
using System.Reflection;

public class FlEngine  {
    private static FlEngine instance;
	public static FlEngine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FlEngine();
            }
            return instance;
        }
    }
    
    private Dictionary<int, FlGameObject> allGameObjects = new Dictionary<int, FlGameObject>();

    private List<FlGameObject> allCollisionGameObject = new List<FlGameObject>();

    private List<FlGameObject> allBehaveGameObject = new List<FlGameObject>();

    private MethodInfo awakeMethodInfo = typeof(FlBehaviour).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
    private MethodInfo updateMethodInfo = typeof(FlBehaviour).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
    private MethodInfo start1MethodInfo = typeof(FlBehaviour).GetMethod("Start1", BindingFlags.NonPublic | BindingFlags.Instance);
    private MethodInfo start2MethodInfo = typeof(FlBehaviour).GetMethod("Start2", BindingFlags.NonPublic | BindingFlags.Instance);
  

    ICollisionDetect2D physicsEngine = new CollisionDetect2DImpl();

    public void AddGameObject(FlGameObject gameObject)
    {
        allGameObjects.Add(gameObject.id, gameObject);
        if (gameObject.hasBehaviour)
        {
            allBehaveGameObject.Add(gameObject);
        }
        if (gameObject.hasCollision)
        {
            allCollisionGameObject.Add(gameObject);
        }
    }

    void InitPhysicsEngine()
    {
        physicsEngine.InitAllRect(allCollisionGameObject);
    }

    public void Awake()
    {
        //初始化物理引擎
        InitPhysicsEngine();
        int[] indexArray= Utils.GenerateRandomIndexArray(allBehaveGameObject.Count);
        for (int i = 0; i < indexArray.Length; i++)
        {
            int index = indexArray[i];
            awakeMethodInfo.Invoke(allBehaveGameObject[index], null);
        }
    }

    public void start1()
    {
        int[] indexArray = Utils.GenerateRandomIndexArray(allBehaveGameObject.Count);
        for (int i = 0; i < indexArray.Length; i++)
        {
            int index = indexArray[i];
            start1MethodInfo.Invoke(allBehaveGameObject[index], null);
        }
    }

    public void start2()
    {
        int[] indexArray = Utils.GenerateRandomIndexArray(allBehaveGameObject.Count);
        for (int i = 0; i < indexArray.Length; i++)
        {
            int index = indexArray[i];
            start2MethodInfo.Invoke(allBehaveGameObject[index], null);
        }
    }

  
    public void Update(AllClientOp allClientOp)
    {
        int[] indexArray = Utils.GenerateRandomIndexArray(allBehaveGameObject.Count);

        for (int i = 0; i < indexArray.Length; i++)
        {
            int index = indexArray[i];
            List<FlComponent> allBehaviour = allBehaveGameObject[index].GetAllBehavour();

            int[] allBehaviourIndex = Utils.GenerateRandomIndexArray(allBehaviour.Count);
            for (int j = 0; j < allBehaviourIndex.Length; j++)
            {
                updateMethodInfo.Invoke(allBehaviourIndex[i],new object[] { allClientOp.allClientOp[allBehaveGameObject[index].id] });
            }
            
        }
    }

    public void HandleFrame(AllClientOp allClientOp)
    {
        //碰撞计算
        physicsEngine.DoCollisionDetect();

        //update
        Update( allClientOp);
    }

    
}
