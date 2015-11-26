using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ObstacleGenerator
{
    private ShuffleBag<Obstacle> ObstaclesShuffleBag { get; set; }
    private ShuffleBag<float> ObstacleOffsetShuffleBag { get; set; }
    private ShuffleBag<int> OffsetShuffleBag { get; set; }
    private CellularAutomata CellularAutomataMap { get; set; }

    public ObstacleGenerator(int PrecentageOfInanimateObjects)
    {
        ObstaclesShuffleBag = new ShuffleBag<Obstacle>();
        ObstacleOffsetShuffleBag = new ShuffleBag<float>();
        OffsetShuffleBag = new ShuffleBag<int>();
        InitializeShuffleBags();
        this.CellularAutomataMap = new CellularAutomata(PrecentageOfInanimateObjects, this.ObstaclesShuffleBag);
    }

    private void InitializeShuffleBags()
    {
        //Positive Range
        ObstacleOffsetShuffleBag.Add(9.5f, 1);
        ObstacleOffsetShuffleBag.Add(8.5f, 1);
        ObstacleOffsetShuffleBag.Add(7.5f, 1);
        ObstacleOffsetShuffleBag.Add(6.5f, 1);
        ObstacleOffsetShuffleBag.Add(5.5f, 1);
        ObstacleOffsetShuffleBag.Add(4.5f, 1);
        ObstacleOffsetShuffleBag.Add(3.5f, 1);
        ObstacleOffsetShuffleBag.Add(2.5f, 1);
        ObstacleOffsetShuffleBag.Add(1.5f, 1);
        ObstacleOffsetShuffleBag.Add(0.5f, 1);

        //Negative Range
        ObstacleOffsetShuffleBag.Add(-9.5f, 1);
        ObstacleOffsetShuffleBag.Add(-8.5f, 1);
        ObstacleOffsetShuffleBag.Add(-7.5f, 1);
        ObstacleOffsetShuffleBag.Add(-6.5f, 1);
        ObstacleOffsetShuffleBag.Add(-5.5f, 1);
        ObstacleOffsetShuffleBag.Add(-4.5f, 1);
        ObstacleOffsetShuffleBag.Add(-3.5f, 1);
        ObstacleOffsetShuffleBag.Add(-2.5f, 1);
        ObstacleOffsetShuffleBag.Add(-1.5f, 1);
        ObstacleOffsetShuffleBag.Add(-0.5f, 1);


        OffsetShuffleBag.Add(5, 30);
        OffsetShuffleBag.Add(7, 20);
        OffsetShuffleBag.Add(8, 5);
        OffsetShuffleBag.Add(10, 3);

        for (int i = 0; i < 40; i++)
        {
            ObstaclesShuffleBag.Add(new Obstacle(ObstaclesType.StuntPlane, new ToleranceRange(9.5f, -1.5f, ObstacleOffsetShuffleBag.Next(), OffsetShuffleBag.Next()), true));
            ObstaclesShuffleBag.Add(new Obstacle(ObstaclesType.Helicopter, new ToleranceRange(9.5f, -1.5f, ObstacleOffsetShuffleBag.Next(), OffsetShuffleBag.Next()), true));
            ObstaclesShuffleBag.Add(new Obstacle(ObstaclesType.FightingJet, new ToleranceRange(9.5f, -1.5f, ObstacleOffsetShuffleBag.Next(), OffsetShuffleBag.Next()), true));

        }
        for (int i = 0; i < 20; i++)
        {
            ObstaclesShuffleBag.Add(new Obstacle(ObstaclesType.Billboard, new ToleranceRange(-1.5f, -9.5f, ObstacleOffsetShuffleBag.Next(), OffsetShuffleBag.Next()), false));
            ObstaclesShuffleBag.Add(new Obstacle(ObstaclesType.WaterTower, new ToleranceRange(-0.5f, -6.5f, ObstacleOffsetShuffleBag.Next(), OffsetShuffleBag.Next()), false));
            ObstaclesShuffleBag.Add(new Obstacle(ObstaclesType.Satellite, new ToleranceRange(-14f, -10f, ObstacleOffsetShuffleBag.Next(), OffsetShuffleBag.Next()), false));
            ObstaclesShuffleBag.Add(new Obstacle(ObstaclesType.WreckingBall, new ToleranceRange(17f,17f,17f, OffsetShuffleBag.Next() + 5), false));
            ObstaclesShuffleBag.Add(new Obstacle(ObstaclesType.InsideBuilding, new ToleranceRange(2f, 2f, 2f, 30f), false));
        }
        //ObstaclesShuffleBag.ShuffleBagModifier((o) => o.OffsetRange.Top = ObstacleOffsetShuffleBag.Next());
    }

    public Obstacle GetObstacle()
    {
        return this.CellularAutomataMap.GetCell();
    }
}
