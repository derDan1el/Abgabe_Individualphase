public interface IWavePropagation
{
    uint GetStride();
    float getHeight(uint i_x, uint i_y);
    float getMomentumX(uint i_x, uint i_y);
    float getMomentumY(uint i_x, uint i_y);
    float getBathymetry(uint i_x, uint i_y);
    uint getGhostcellX();
    uint getGhostcellY();
    void setHeight(uint i_x, uint i_y, float i_value);
    void setBathymetry(uint i_x, uint i_y, float i_value);
    void setMomentumX(uint i_x, uint i_y, float i_value);
    void setMomentumY(uint i_x, uint i_y, float i_value);
    float[] getHeightValues();
    float[] getBathymetryValues();
    float[] getMomentumXValues();
    float[] getMomentumYValues();
    void timeStep(float i_scaling);
    void setGhostColumn();
}