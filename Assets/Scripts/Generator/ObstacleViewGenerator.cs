using UnityEngine;
using System.Collections.Generic;

public class ObstacleViewGenerator : MonoBehaviour 
{
    private ObstacleGenerator obstacleGenerator;
    
    [Header("Movable")]
    public ObstacleViewItem planeItem;
    public ObstacleViewItem helicopterItem;
    public ObstacleViewItem jetItem;
    
    [Header("Static")]
    public ObstacleViewItem billboardItem;
    public ObstacleViewItem insideBuildingItem;
    public ObstacleViewItem satelliteItem;
    public ObstacleViewItem waterTowerItem;
    public ObstacleViewItem wreckingBallItem;

    private ObjectPool<ObstacleViewItem> planePool;
    private ObjectPool<ObstacleViewItem> helicopterPool;
    private ObjectPool<ObstacleViewItem> jetPool;

    private ObjectPool<ObstacleViewItem> billboardPool;
    private ObjectPool<ObstacleViewItem> insideBuildingPool;
    private ObjectPool<ObstacleViewItem> satelliteItemPool;
    private ObjectPool<ObstacleViewItem> waterTowerPool;
    private ObjectPool<ObstacleViewItem> wreckingBallPool;

    private Dictionary<ObstaclesType, ObjectPool<ObstacleViewItem>> viewDictionary;

    private float currentMovingSpeed { get; set; }

    private Queue<ObstacleViewItem> generatedItems = new Queue<ObstacleViewItem>();

    public bool IsActive { get; set; }
	void Awake () 
    {
        #if UNITY_EDITOR
        CheckReferences();
        #endif
        InitializePools();
        InitializeViewDictionary();
        CalculateSpeed(0);

	}
    public void Initialize(Transform target)
    {
        this.target = target;
    }

    private void CheckReferences()
    {
        if (planeItem == null) throw new MissingReferenceException("Missing a reference to the planeItem on " + this.gameObject);
        if (helicopterItem == null) throw new MissingReferenceException("Missing a reference to the helicopterItem on " + this.gameObject);
        if (jetItem == null) throw new MissingReferenceException("Missing a reference to the jetItem on " + this.gameObject);

        if (satelliteItem == null) throw new MissingReferenceException("Missing a reference to the satelliteItem on " + this.gameObject);
        if (waterTowerItem == null) throw new MissingReferenceException("Missing a reference to the waterTowerItem on " + this.gameObject);
        if (billboardItem == null) throw new MissingReferenceException("Missing a reference to the billboardPool on " + this.gameObject);

        if (wreckingBallItem == null) throw new MissingReferenceException("Missing a reference to the wreckingBallItem on " + this.gameObject);
        if (insideBuildingItem == null) throw new MissingReferenceException("Missing a reference to the insideBuildingItem on " + this.gameObject);


    }
    private void InitializePools()
    {
        planePool = new ObjectPool<ObstacleViewItem>(planeItem, 8);
        helicopterPool = new ObjectPool<ObstacleViewItem>(helicopterItem, 8);
        jetPool = new ObjectPool<ObstacleViewItem>(jetItem, 8);

        satelliteItemPool = new ObjectPool<ObstacleViewItem>(satelliteItem, 8);
        waterTowerPool = new ObjectPool<ObstacleViewItem>(waterTowerItem, 8);
        billboardPool = new ObjectPool<ObstacleViewItem>(billboardItem, 8);

        wreckingBallPool = new ObjectPool<ObstacleViewItem>(wreckingBallItem, 8);
        insideBuildingPool = new ObjectPool<ObstacleViewItem>(insideBuildingItem, 8);
    }

    private void InitializeViewDictionary()
    {
        viewDictionary = new Dictionary<ObstaclesType, ObjectPool<ObstacleViewItem>>();

        viewDictionary.Add(ObstaclesType.StuntPlane, planePool);
        viewDictionary.Add(ObstaclesType.FightingJet, jetPool);
        viewDictionary.Add(ObstaclesType.Helicopter, helicopterPool);

        viewDictionary.Add(ObstaclesType.Satellite, satelliteItemPool);
        viewDictionary.Add(ObstaclesType.WaterTower, waterTowerPool);
        viewDictionary.Add(ObstaclesType.Billboard, billboardPool);

        viewDictionary.Add(ObstaclesType.WreckingBall, wreckingBallPool);
        viewDictionary.Add(ObstaclesType.InsideBuilding, insideBuildingPool);

    }
    private Vector3 startLocation = new Vector3(50, 0, 0);
    private Vector3 lastLocation  = new Vector3(50, 0, 0);
    void GenerateNextChunk()
    {

        for (int i = 0; i < 30; i++)
        {
            var obstacle = obstacleGenerator.GetObstacle();

            lastLocation.y = obstacle.OffsetRange.SafeVerticalValue;

            if (obstacle.ObstacleType == ObstaclesType.InsideBuilding)
            {
                lastLocation.x += obstacle.OffsetRange.Right;
            }
            var obstacleViewPool = viewDictionary[obstacle.ObstacleType];

            var obstacleView = obstacleViewPool.Get();
            obstacleView.Show(lastLocation, currentMovingSpeed, obstacle);

            generatedItems.Enqueue(obstacleView);

            lastLocation.x += obstacle.OffsetRange.Right;

        }
    }

    private Transform target;
    public float visibleDistanceFront;
    public float visibleDistanceBehind;
    private Queue<ObstacleViewItem> passedObstacles = new Queue<ObstacleViewItem>();
    private void CheckVisibility()
    {

        if (generatedItems.Count != 0 &&  target.position.x + visibleDistanceFront >= generatedItems.Peek().transform.position.x)
        {
            var viewItem = generatedItems.Dequeue();
            viewItem.Activate();
            passedObstacles.Enqueue(viewItem);
        }

        if (passedObstacles.Count != 0 && passedObstacles.Peek().transform.position.x + visibleDistanceBehind < target.position.x)
        {
            var viewItem = passedObstacles.Dequeue();
            viewItem.Hide();

            var viewPool = viewDictionary[viewItem.Obstacle.ObstacleType];
            viewPool.Return(viewItem);
        }
    }

    public void Generate()
    {
        IsActive = true;
        //From prev run
        ReturnAllIObjectsToPool(generatedItems);
        ReturnAllIObjectsToPool(passedObstacles);

        lastLocation = startLocation;

        obstacleGenerator = new ObstacleGenerator(PrecentageOfInanimateObjects: 20);
        GenerateNextChunk();

    }
    private void ReturnAllIObjectsToPool(Queue<ObstacleViewItem> viewItems)
    {

        while (viewItems.Count > 0) 
        {
            var item = viewItems.Dequeue();
            item.Hide();

            var viewPool = viewDictionary[item.Obstacle.ObstacleType];
            viewPool.Return(item);
        }

    }
    public float distanceToGenerateNextChunk;
    void LateUpdate()
    {
        if (IsActive)
        {
            CheckVisibility();

            if (target.position.x + distanceToGenerateNextChunk >= lastLocation.x)
            {
                GenerateNextChunk();
            }
        }

    }

    public float maxObstacleSpeed;
    public float minObstacleSpeed;
    public float timeToReachMaxSpeed;

    public void CalculateSpeed(float timeSinceRunStarted)
    {
        currentMovingSpeed = Mathf.Lerp(minObstacleSpeed, maxObstacleSpeed, timeSinceRunStarted / timeToReachMaxSpeed);
    }
}
