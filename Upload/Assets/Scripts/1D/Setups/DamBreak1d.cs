using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamBreak1d : ISetup
{
    private float m_heightLeft = 0;
    private float m_heightRight = 0;
    private float m_locationDam= 0;

    public DamBreak1d(float heightLeft, float heightRight, float locationDam)
    {
        m_heightLeft = heightLeft;
        m_heightRight = heightRight;
        m_locationDam = locationDam;
    }

    public float GetHeight(float x,float y)
    {
        if (x < m_locationDam)
        {
            return m_heightLeft;
        }
        else
        {
            return m_heightRight;
        }
    }

    public float GetMomentumX(float x, float y)
    {
        return 0;
    }

    public float GetMomentumY(float x, float y)
    {
        return 0;
    }

    public float GetBathymetry(float x, float y)
    {
        return 0;
    }
}

