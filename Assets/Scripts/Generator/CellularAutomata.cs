using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// Stripped down version of Cellular Automata class, without the simulation part
/// </summary>
public class CellularAutomata
{
    private List<Obstacle> CellMap { get; set; }
    /// <summary>
    /// Possibility to start as inanimate object (percentage ranging from 1 to 99. If 0% given it will be set to 1%)
    /// </summary>
    private int chanceToStartInanimate;

    public int ChanceToStartInanimate
    {
        get { return chanceToStartInanimate; }
        set { chanceToStartInanimate = value < 1 ? 1 : value; }
    }

    private ShuffleBag<int> ChancesShuffleBag { get; set; }

    private ShuffleBag<Obstacle> ObstaclesShuffleBag { get; set; }
    private int CellGetterCounter { get; set; }

    public CellularAutomata(int ChanceToStartInanimate, ShuffleBag<Obstacle> ObstaclesShuffleBag)
    {
        this.ChanceToStartInanimate = ChanceToStartInanimate;
        this.ChancesShuffleBag = new ShuffleBag<int>();
        this.ObstaclesShuffleBag = ObstaclesShuffleBag;
        this.CellGetterCounter = 0;
        this.CellMap = new List<Obstacle>();

        InitializeChances();
        InitializeMap();
    }

    private void InitializeChances()
    {
        this.ChancesShuffleBag.Add(ChanceToStartInanimate, ChanceToStartInanimate);
        this.ChancesShuffleBag.Add(99 - ChanceToStartInanimate, 99 - ChanceToStartInanimate);
    }

    private void InitializeMap()
    {
        var numberOfColumns = 5;
        var numberOfRows = this.ObstaclesShuffleBag.Size / numberOfColumns;
        this.CellMap = Enumerable.Repeat<Obstacle>(null, numberOfColumns * numberOfRows).ToList();
        for (int i = 0; i < numberOfColumns; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                if (ChancesShuffleBag.Next() < ChanceToStartInanimate)
                    CellMap[j * numberOfColumns + i] = ObstaclesShuffleBag.Next(o => o.IsMoving); // Inanimate Object
                else
                    CellMap[j * numberOfColumns + i] = ObstaclesShuffleBag.Next(o => !o.IsMoving); // Moving object
            }
        }
    }

    public Obstacle GetCell()
    {
        CellGetterCounter++;
        if (CellGetterCounter == CellMap.Count)
        {
            CellGetterCounter = 0;
        }
        return CellMap[CellGetterCounter];
    }
}
