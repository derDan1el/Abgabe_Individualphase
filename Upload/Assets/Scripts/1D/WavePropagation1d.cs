using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

 public class WavePropagation1d : IWavePropagation
{
    private bool m_choiceBoundry;
    private uint m_nCells;
    private float[] m_h;
    private float[] m_hu;
    private float[] m_h_temp;
    private float[] m_hu_temp;
    private float[] m_b;
    FWave fwave = new FWave();

    public WavePropagation1d(uint i_nCells, bool i_choiceBoundry)
    {
        m_choiceBoundry = i_choiceBoundry;
        m_nCells = i_nCells;


        // allocate memory including a single ghost cell on each side
        m_h = new float[m_nCells + 2];
        m_hu = new float[m_nCells + 2];
        m_h_temp = new float[m_nCells + 2];
        m_hu_temp = new float[m_nCells + 2];
        m_b = new float[m_nCells + 2];

    }

    public void timeStep(float i_scaling)
    {

        setGhostColumn();
        // init new cell quantities
        for (int l_ce = 0; l_ce < m_nCells + 1; l_ce++)
        {
            m_h_temp[l_ce] = m_h[l_ce];
            m_hu_temp[l_ce] = m_hu[l_ce];
        }

        // iterate over edges and update with Riemann solutions
        for (int l_ed = 0; l_ed < m_nCells; l_ed++)  /// ? hier vllt +1
        {
            // determine left and right cell-id
            int l_ceL = l_ed;
            int l_ceR = l_ed + 1;

            // compute net-updates
            float[][] l_netUpdates = new float[2][];
            l_netUpdates[0] = new float[2];
            l_netUpdates[1] = new float[2];


            fwave.NetUpdates(m_h_temp[l_ceL],
                                m_h_temp[l_ceR],
                                m_hu_temp[l_ceL],
                                m_hu_temp[l_ceR],
                                m_b[l_ceL],
                                m_b[l_ceR],
                                out l_netUpdates[0],
                                out l_netUpdates[1]);


            // update the cells' quantities
            m_h[l_ceL] -= i_scaling * l_netUpdates[0][0];
            m_hu[l_ceL] -= i_scaling * l_netUpdates[0][1];

            m_h[l_ceR] -= i_scaling * l_netUpdates[1][0];
            m_hu[l_ceR] -= i_scaling * l_netUpdates[1][1];
        }

    }

    public void setGhostColumn()
    {
        if (m_choiceBoundry)
        {
            // Reflecting conditions
            m_h[0] = m_h[1];
            m_hu[0] = m_hu[1];
            m_b[0] = m_b[1];

            m_h[m_nCells + 1] = m_h[m_nCells];
            m_hu[m_nCells + 1] = m_hu[m_nCells];
            m_b[m_nCells + 1] = m_b[m_nCells];
        }
        else
        {
            // Outflow conditions
            m_h[0] = m_h[1];
            m_hu[0] = m_hu[1];
            m_b[0] = m_b[1];

            m_h[m_nCells + 1] = m_h[m_nCells];
            m_hu[m_nCells + 1] = m_hu[m_nCells];
            m_b[m_nCells + 1] = m_b[m_nCells];
        }
    }


    public uint GetStride()
    {
        return m_nCells + 2;
    }
    public void setHeight(uint j, uint i, float l_h)
    {
        m_h[j + 1] = l_h;
    }

    public void setMomentumX(uint j, uint i, float l_hu)
    {
        m_hu[j + 1] = l_hu;
    }

    public void setMomentumY(uint j, uint i, float l_hv)
    {
        throw new NotImplementedException();
    }

    public void setBathymetry(uint j, uint i, float l_bv)
    {
        m_b[j + 1] = l_bv;
    }

    public float getHeight(uint l_i, uint v)
    {
        return m_h[l_i + 1];
    }

    public float getMomentumX(uint l_i, uint v)
    {
        return m_hu[l_i + 1];
    }

    public float getMomentumY(uint l_i, uint v)
    {
        throw new NotImplementedException();
    }

    public float getBathymetry(uint l_i, uint v)
    {
        return m_b[l_i + 1];
    }

    public uint getGhostcellX()
    {
        return 1;
    }

    public uint getGhostcellY()
    {
        return 0;
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
        return null;
    }
}