using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamBreak2D : ISetup
{
    public DamBreak2D()
    {

    }
    public float GetHeight(float x, float y)
    {
        if (Math.Sqrt((x * x) + (y * y)) < 10)
        {
            return 10;
        }
        else
        {
            return 5;
        }
    }
    public float GetMomentumX(float x, float y)
    {
        return 0.0f;
    }
    public float GetMomentumY(float x, float y)
    {
        return 0.0f;
    }

    public float GetBathymetry(float x, float y)
    {
        return 0.0f;
    }
}