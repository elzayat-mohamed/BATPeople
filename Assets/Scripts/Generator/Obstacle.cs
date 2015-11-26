using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Obstacle
{
    public ObstaclesType ObstacleType { get; set; }
    public ToleranceRange OffsetRange { get; set; }
    public bool IsMoving { get; set; }

    public Obstacle(ObstaclesType ObstacleType, ToleranceRange OffsetRange, bool IsMoving, int TopOffsetTolerance = 3)
    {
        this.ObstacleType = ObstacleType;
        this.IsMoving = IsMoving;
        this.OffsetRange = OffsetRange;
    }
}