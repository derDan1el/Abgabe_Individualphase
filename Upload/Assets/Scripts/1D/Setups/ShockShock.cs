using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ShockShock : ISetup
{
    private float m_height;
    private float m_hu;
    private float m_locationDam;

    public ShockShock(float i_height, float i_hu, float i_locationDam)
    {
        m_height = i_height;
        m_hu = i_hu;
        m_locationDam = i_locationDam;
    }

    public float GetHeight(float x, float y)
    {
        return m_height;
    }

    public float GetMomentumX(float x, float y)
    {
        if (x <= m_locationDam)
        {
            return m_hu;
        }
        else
        {
            return -m_hu;
        }
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


