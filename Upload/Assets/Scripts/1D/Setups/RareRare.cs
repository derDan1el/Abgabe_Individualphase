using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RareRare : ISetup
{
    private float m_height = 0;
    private float m_hu = 0;
    private float m_locationDam = 0;

    
    public RareRare(float height, float hu, float locationDam)
    {
        m_height = height;
        m_hu = hu;
        m_locationDam = locationDam;
    }

    public float GetHeight(float x, float y)
    {
        return m_height;
    }


    public float GetMomentumX(float x, float y)
    {
        return x <= m_locationDam ? -m_hu : m_hu;
    }


    public float GetBathymetry(float x, float y)
    {
        return 0;
    }


    public float GetMomentumY(float x, float y)
    {
        return 0;
    }
}
