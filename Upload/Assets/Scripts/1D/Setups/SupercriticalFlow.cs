using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SupercriticalFlow : ISetup
{
    public float GetHeight(float x, float y)
    {
        return -GetBathymetry(x, 0);
    }

    public float GetMomentumX(float x, float y)
    {
        return 0.18f;
    }

    public float GetMomentumY(float x, float y)
    {
        return 0;
    }

    // As long as the x-value stays between 8 and 12, return -0.13 - 0.05 * Math.Pow((x - 10), 2);
    // Otherwise, return -0.33 for the bathymetry
    public float GetBathymetry(float x, float y)
    {
        if (x > 8 && x < 12)
        {
            return (float)(-0.13 - 0.05 * Math.Pow((x - 10), 2));
        }
        else
        {
            return -0.33f;
        }
    }
}