using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QTableEntry
{
    public int distX;
    public int distYTop;
    public int distYBottom;
    public int action; // 0 for not flap, 1 for flap
    public float value;

    // Constructor
    public QTableEntry(int x, int yTop, int yBottom, int action, float value)
    {
        this.distX = x;
        this.distYTop = yTop;
        this.distYBottom = yBottom;
        this.action = action;
        this.value = value;
    }
}

[Serializable]
public class QTable
{
    public List<QTableEntry> entries = new List<QTableEntry>();
}
