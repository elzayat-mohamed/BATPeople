using System;
using System.Collections.Generic;

public class ShuffleBag<T>
{
    private Random random { get; set; }
    private List<T> data { get; set; }
    private T CurrentItem { get; set; }
    private int CurrentPosition = -1;
    public int Size { get { return data.Count; } }

    public ShuffleBag()
    {
        this.data = new List<T>();
        this.random = new Random();
    }

    public void Add(T item, int Amount)
    {
        for (int i = 0; i < Amount; i++)
            this.data.Add(item);

        this.CurrentPosition = this.Size - 1;
    }

    public void Add(T item)
    {
        this.data.Add(item);
    }

    public T Next()
    {
        if (CurrentPosition < 0)
        {
            this.CurrentPosition = this.Size - 1;
            CurrentItem = this.data[0];
            return CurrentItem;
        }
        
        var position = this.random.Next(this.CurrentPosition);
        CurrentItem = this.data[position];
        this.data[position] = data[CurrentPosition];
        this.data[CurrentPosition] = CurrentItem;
        CurrentPosition--;

        return CurrentItem;
    }

    public T Next(Func<T, bool> Predicate)
    {
        if (CurrentPosition < 0)
        {
            this.CurrentPosition = this.Size - 1;
            CurrentItem = this.data[0];
            if (Predicate(CurrentItem))
                return CurrentItem;
        }

        foreach (var item in this.data)
        {
            if (Predicate(item))
            {
                var position = this.random.Next(this.CurrentPosition);
                CurrentItem = this.data[position];
                this.data[position] = data[CurrentPosition];
                this.data[CurrentPosition] = CurrentItem;
                CurrentPosition--;
                return CurrentItem;
            }
        }
        return default(T);
    }

    public void ShuffleBagModifier(Action<T> ModifierAction)
    {
        this.data.ForEach(ModifierAction);
    }
}