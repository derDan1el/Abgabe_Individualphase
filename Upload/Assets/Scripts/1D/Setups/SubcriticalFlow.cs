using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SubcriticalFlow : ISetup
{
    public float GetHeight(float x, float y)
    {
        return -GetBathymetry(x, 0);
    }

    public float GetMomentumX(float x, float y)
    {
        return 4.42f;
    }

    public float GetMomentumY(float x, float y)
    {
        return 0;
    }

    // As long as the x-value stays between 8 and 12, return (-1.8 - 0.05 * Math.Pow((x - 10), 2));
    // Otherwise, return -2 for the bathymetry
    public float GetBathymetry(float x, float y)
    {
        if (x > 8 && x < 12)
        {
            return (float)(-1.8 - 0.05 * Math.Pow((x - 10), 2));
        }
        else
        {
            return -2;
        }
    }
}
