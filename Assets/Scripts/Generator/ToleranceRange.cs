using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Stripped down version of Tolerance class for the purpose of this project
/// </summary>
public class ToleranceRange
{
    public float MaxVerticalTolerance { get; set; }
    public float MinVerticalTolerance { get; set; }
    public float Top { get; set; }
    public float Right { get; set; }
    public float SafeVerticalValue
    {
        get
        {
            if (this.Top > this.MaxVerticalTolerance)
                return this.MaxVerticalTolerance;
            if (this.Top < this.MinVerticalTolerance)
                return this.MinVerticalTolerance;
            return this.Top;
        }
    }

    public ToleranceRange(float Max, float Min, float Top, float Right)
    {
        this.MaxVerticalTolerance = Max;
        this.MinVerticalTolerance = Min;
        this.Top = Top;
        this.Right = Right;
    }
}
