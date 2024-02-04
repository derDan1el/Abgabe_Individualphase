using System;
using UnityEditor;

public class WavePropagation2d : IWavePropagation
{
    private bool m_choiceBoundary;
    private uint m_xCells;
    private uint m_yCells;
    private float[] m_h;
    private float[] m_hu;
    private float[] m_hv;
    private float[] m_b;
    private float[] m_h_temp;
    private float[] m_momentum_temp;
    FWave FWave = new FWave();
    public WavePropagation2d(uint i_xCells, uint i_yCells, bool i_choiceBoundary)
    {
        m_choiceBoundary = i_choiceBoundary;
        m_xCells = i_xCells;
        m_yCells = i_yCells;

        // allocate memory including a single ghost cell on each side
        m_h = new float[(m_xCells + 2) * (m_yCells + 2)];
        m_hu = new float[(m_xCells + 2) * (m_yCells + 2)];
        m_hv = new float[(m_xCells + 2) * (m_yCells + 2)];
        m_b = new float[(m_xCells + 2) * (m_yCells + 2)];
        m_h_temp = new float[(m_xCells + 2) * (m_yCells + 2)];
        m_momentum_temp = new float[(m_xCells + 2) * (m_yCells + 2)];
    }

    public void timeStep(float i_scaling)
    {
        setGhostColumn();
        for (uint l_ce = 0; l_ce < ((m_xCells + 2) * (m_yCells + 2)); l_ce++)
        {
            m_h_temp[l_ce] = m_h[l_ce];
            m_momentum_temp[l_ce] = m_hu[l_ce];
        }

        for (uint l_ex = 0; l_ex < m_xCells + 1; l_ex++)
        {
            for (uint l_ey = 0; l_ey < m_yCells + 1; l_ey++)
            {
                float[][] l_netUpdates = new float[2][];
                l_netUpdates[0] = new float[2];
                l_netUpdates[1] = new float[2];

                uint l_ceL = GetIndex(l_ex, l_ey);
                uint l_ceR = GetIndex(l_ex + 1, l_ey);

                FWave.NetUpdates(m_h_temp[l_ceL],
                                 m_h_temp[l_ceR],
                                 m_momentum_temp[l_ceL],
                                 m_momentum_temp[l_ceR],
                                 m_b[l_ceL],
                                 m_b[l_ceR],
                                 out l_netUpdates[0],
                                 out l_netUpdates[1]);

                m_h[l_ceL] -= i_scaling * l_netUpdates[0][0];
                m_hu[l_ceL] -= i_scaling * l_netUpdates[0][1];
                m_h[l_ceR] -= i_scaling * l_netUpdates[1][0];
                m_hu[l_ceR] -= i_scaling * l_netUpdates[1][1];
            }
        }

        //----------------------------------------Y-SWEEP-----------------------------------------------------------------------------------------------------------------------------------------

        setGhostRow();

        for (uint l_ce = 0; l_ce < ((m_xCells + 2) * (m_yCells + 2)); l_ce++)
        {
            m_h_temp[l_ce] = m_h[l_ce];
            m_momentum_temp[l_ce] = m_hv[l_ce];
        }

        for (uint l_ex = 0; l_ex < m_xCells + 1; l_ex++)
        {
            for (uint l_ey = 0; l_ey < m_yCells + 1; l_ey++)
            {
                float[][] l_netUpdates = new float[2][];
                l_netUpdates[0] = new float[2];
                l_netUpdates[1] = new float[2];
                uint l_ceL = GetIndex(l_ex, l_ey);
                uint l_ceR = GetIndex(l_ex, l_ey + 1);

                FWave.NetUpdates(m_h_temp[l_ceL],
                                 m_h_temp[l_ceR],
                                 m_momentum_temp[l_ceL],
                                 m_momentum_temp[l_ceR],
                                 m_b[l_ceL],
                                 m_b[l_ceR],
                                 out l_netUpdates[0],
                                 out l_netUpdates[1]);
                m_h[l_ceL] -= i_scaling * l_netUpdates[0][0];
                m_hv[l_ceL] -= i_scaling * l_netUpdates[0][1];
                m_h[l_ceR] -= i_scaling * l_netUpdates[1][0];
                m_hv[l_ceR] -= i_scaling * l_netUpdates[1][1];
            }
        }
    }

    public void setGhostRow()
    {
        // bottom row & top row
        if (m_choiceBoundary)
        {
            for (uint l_g = 1; l_g < m_xCells + 1; l_g++)
            {
                m_h[l_g] = m_h[GetIndex(l_g, 1)];
                m_h[GetIndex(l_g, m_yCells + 1)] = m_h[GetIndex(l_g, m_yCells)];
                m_hv[l_g] = -m_hv[GetIndex(l_g, 1)];
                m_hv[GetIndex(l_g, m_yCells + 1)] = -m_hv[GetIndex(l_g, m_yCells)];
                m_b[l_g] = m_b[GetIndex(l_g, 1)];
                m_b[GetIndex(l_g, m_yCells + 1)] = m_b[GetIndex(l_g, m_yCells)];
            }
        }
        else
        {
            for (uint l_g = 1; l_g < m_xCells + 1; l_g++)
            {
                m_h[l_g] = m_h[GetIndex(l_g, 1)];
                m_h[GetIndex(l_g, m_yCells + 1)] = m_h[GetIndex(l_g, m_yCells)];
                m_hv[l_g] = m_hv[GetIndex(l_g, 1)];
                m_hv[GetIndex(l_g, m_yCells + 1)] = m_hv[GetIndex(l_g, m_yCells)];
                m_b[l_g] = m_b[GetIndex(l_g, 1)];
                m_b[GetIndex(l_g, m_yCells + 1)] = m_b[GetIndex(l_g, m_yCells)];
            }
        }
    }

    public void setGhostColumn()
    {
        // leftmost and rightmost column
        if (m_choiceBoundary)
        {
            for (uint l_g = 1; l_g < m_yCells + 1; l_g++)
            {
                m_h[GetIndex(0, l_g)] = m_h[GetIndex(1, l_g)];
                m_h[GetIndex(m_xCells + 1, l_g)] = m_h[GetIndex(m_xCells, l_g)];
                m_hu[GetIndex(0, l_g)] = -m_hu[GetIndex(1, l_g)];
                m_hu[GetIndex(m_xCells + 1, l_g)] = -m_hu[GetIndex(m_xCells, l_g)];
                m_b[GetIndex(0, l_g)] = m_b[GetIndex(1, l_g)];
                m_b[GetIndex(m_xCells + 1, l_g)] = m_b[GetIndex(m_xCells, l_g)];
            }
        }
        else
        {
            for (uint l_g = 1; l_g < m_yCells + 1; l_g++)
            {
                m_h[GetIndex(0, l_g)] = m_h[GetIndex(1, l_g)];
                m_h[GetIndex(m_xCells + 1, l_g)] = m_h[GetIndex(m_xCells, l_g)];
                m_hu[GetIndex(0, l_g)] = m_hu[GetIndex(1, l_g)];
                m_hu[GetIndex(m_xCells + 1, l_g)] = m_hu[GetIndex(m_xCells, l_g)];
                m_b[GetIndex(0, l_g)] = m_b[GetIndex(1, l_g)];
                m_b[GetIndex(m_xCells + 1, l_g)] = m_b[GetIndex(m_xCells, l_g)];
            }
        }
    }

    public uint GetStride()
    {
        return m_xCells + 2;
    }
    private uint GetIndex(uint i_x, uint i_y)
    {
        return i_x + i_y * (m_xCells + 2);
    }

    public float getHeight(uint i_x, uint i_y)
    {
        return m_h[GetIndex(i_x + 1, i_y + 1)];
    }

    public float getMomentumX(uint i_x, uint i_y)
    {
        return m_hu[GetIndex(i_x + 1, i_y + 1)];
    }

    public float getMomentumY(uint i_x, uint i_y)
    {
        return m_hv[GetIndex(i_x + 1, i_y + 1)];
    }

    public float getBathymetry(uint i_x, uint i_y)
    {
        return m_b[GetIndex(i_x + 1, i_y + 1)];
    }


    public void setHeight(uint i_x, uint i_y, float i_value)
    {
        m_h[GetIndex(i_x + 1, i_y + 1)] = i_value;
    }

    public void setBathymetry(uint i_x, uint i_y, float i_value)
    {
        m_b[GetIndex(i_x + 1, i_y + 1)] = i_value;
    }

    public void setMomentumX(uint i_x, uint i_y, float i_value)
    {
        m_hu[GetIndex(i_x + 1, i_y + 1)] = i_value;
    }

    public void setMomentumY(uint i_x, uint i_y, float i_value)
    {
        m_hv[GetIndex(i_x + 1, i_y + 1)] = i_value;
    }

    public float[] getHeightValues()
    {
        return m_h;
    }

    public float[] getBathymetryValues()
    {
        return m_b;
    }

    public float[] getMomentumXValues()
    {
        return m_hu;
    }
    public float[] getMomentumYValues()
    {
        return m_hv;
    }
    public uint getGhostcellX()
    {
        return 1;
    }

    public uint getGhostcellY()
    {
        return 1; // ? so unnötig bald wie möglich entfernen (außer jz kb )
    }

}