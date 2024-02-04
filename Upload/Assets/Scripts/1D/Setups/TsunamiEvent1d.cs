using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TsunamiEvent1d : ISetup
{
    private List<float> m_bathymetryValues;
    private const float m_delta = 20f;

    public TsunamiEvent1d()
    {
        const string filename = "Assets/data/data_end.csv";
        int columnIndex = 3;
        m_bathymetryValues = IO.Read(filename, columnIndex);
    }

    public float GetBathymetry(float x, float y)
    {
        float bin = GetBathymetryCsv(x);
        if (bin < 0)
        {
            if (bin < -m_delta)
            {
                return bin + Displacement(x);
            }
            else
            {
                return -m_delta + Displacement(x);
            }
        }
        else
        {
            if (bin > m_delta)
            {
                return bin + Displacement(x);
            }
            else
            {
                return m_delta + Displacement(x);
            }
        }
    }

    public float GetHeight(float x, float y)
    {
        float bin = GetBathymetryCsv(x);
        if (bin < 0)
        {
            if (-bin < m_delta)
            {
                return m_delta;
            }
            else
            {
                return -bin;
            }
        }
        else
        {
            return 0;
        }
    }

    public float Displacement(float x)
    {
        if (x > 175000 && x < 250000)
        {
            return (float)(10 * Math.Sin(((x - 175000) / (37500)) * Math.PI + Math.PI));
        }
        else
        {
            return 0;
        }
    }

    public float GetBathymetryCsv(float x)
    {
        // x gets divided by 250 because every cell is in 250m steps
        int index = (int)(x / 250);
        return m_bathymetryValues[index];
    }


    public float GetMomentumX(float x, float y)
    {
        return 0;
    }

    public float GetMomentumY(float x, float y)
    {
        return 0;
    }


}