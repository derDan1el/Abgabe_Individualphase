using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 public class FWave
 {

     public static  float m_g = 9.80665f;
     public static  float m_gSqrt = 3.131557121f;

     public  void Eigenvalues(float i_hL, float i_hR, float i_uL, float i_uR, out float o_waveSpeedL, out float o_waveSpeedR)
     {
         float l_hSqrtL = MathF.Sqrt(i_hL);
         float l_hSqrtR = MathF.Sqrt(i_hR);

         float l_hRoe = 0.5f * (i_hL + i_hR);
         float l_uRoe = (l_hSqrtL * i_uL + l_hSqrtR * i_uR) / (l_hSqrtL + l_hSqrtR);

         float l_ghSqrtRoe = m_gSqrt * MathF.Sqrt(l_hRoe);

         o_waveSpeedL = l_uRoe - l_ghSqrtRoe;
         o_waveSpeedR = l_uRoe + l_ghSqrtRoe;
     }

     public  void Flux(float i_hL, float i_hR, float i_huL, float i_huR, out float[] o_fdelta)
     {
         o_fdelta = new float[2];
         float pow2HuL = i_huL * i_huL / i_hL;
         float gPowHL = m_g * i_hL * i_hL;
         float totalL = pow2HuL + 0.5f * gPowHL;
         float[] fql = { i_huL, totalL };

         float pow2HuR = i_huR * i_huR / i_hR;
         float gPowHR = m_g * i_hR * i_hR;
         float totalR = pow2HuR + 0.5f * gPowHR;
         float[] fqr = { i_huR, totalR };

         o_fdelta[0] = fqr[0] - fql[0];
         o_fdelta[1] = fqr[1] - fql[1];
     }

     public  void Decompose(float[] i_alphas, float[] i_eigens, out float[] o_minus_A_deltaQ, out float[] o_plus_A_deltaQ)
     {
         o_minus_A_deltaQ = new float[2];
         o_plus_A_deltaQ = new float[2];

         if (i_eigens[0] < 0)
         {
             o_minus_A_deltaQ[0] += i_alphas[0];
             o_minus_A_deltaQ[1] += i_alphas[0] * i_eigens[0];
         }
         else
         {
             o_plus_A_deltaQ[0] += i_alphas[0];
             o_plus_A_deltaQ[1] += i_alphas[0] * i_eigens[0];
         }

         if (i_eigens[1] < 0)
         {
             o_minus_A_deltaQ[0] += o_minus_A_deltaQ[0] + i_alphas[1];
             o_minus_A_deltaQ[1] += o_minus_A_deltaQ[1] + i_alphas[1] * i_eigens[1];
         }
         else
         {
             o_plus_A_deltaQ[0] += o_plus_A_deltaQ[0] + i_alphas[1];
             o_plus_A_deltaQ[1] += o_plus_A_deltaQ[1] + i_alphas[1] * i_eigens[1];
         }
        
     }

     public  void InverseMatrix(float i_eigen1, float i_eigen2, out float[] o_inverse)
     {
         o_inverse = new float[4]; // Initialize the o_inverse array

         float det = 1 / (i_eigen2 - i_eigen1);

         o_inverse[0] = i_eigen2 * det;
         o_inverse[1] = -1 * det;
         o_inverse[2] = -i_eigen1 * det;
         o_inverse[3] = 1 * det;
     }

     public  void EigencoefficientAlpha(float[] i_inverse, float[] i_delta_f, float i_b, out float[] o_eigencoefficients)
     {
         i_delta_f[1] = i_delta_f[1] - i_b;

         o_eigencoefficients = new float[2]; // Initialize the o_eigencoefficients array

         o_eigencoefficients[0] = (i_inverse[0] * i_delta_f[0]) + (i_inverse[1] * i_delta_f[1]);
         o_eigencoefficients[1] = (i_inverse[2] * i_delta_f[0]) + (i_inverse[3] * i_delta_f[1]);
     }

     public  void NetUpdates(float i_hL, float i_hR, float i_huL, float i_huR, float i_bL, float i_bR, out float[] o_minus_A_deltaQ, out float[] o_plus_A_deltaQ)
     {
         
         
         o_minus_A_deltaQ = new float[2]; // Assign initial value to o_minus_A_deltaQ
         o_minus_A_deltaQ[1] = 0;
         o_minus_A_deltaQ[0] = 0;
         
         o_plus_A_deltaQ = new float[2]; // Assign initial value to o_plus_A_deltaQ
         o_plus_A_deltaQ[1] = 0;
         o_plus_A_deltaQ[0] = 0;
         
         float[] temp = new float[2];

         if (i_hL <= 0)
         {
             if (i_hR <= 0)
             {
                 return;
             }

             i_hL = i_hR;
             i_huL = -i_huR;
             i_bL = i_bR;
             o_minus_A_deltaQ = temp;
         }
         else if (i_hR <= 0)
         {
             i_hR = i_hL;
             i_huR = -i_huL;
             i_bR = i_bL;
             o_plus_A_deltaQ = temp;
         }

         float l_uL = i_huL / i_hL;
         float l_uR = i_huR / i_hR;

         float l_sL = 0;
         float l_sR = 0;

         Eigenvalues(i_hL, i_hR, l_uL, l_uR, out l_sL, out l_sR);

         float[] l_inverse = new float[4];

         InverseMatrix(l_sL, l_sR, out l_inverse);

         float[] l_fdelta = { 0, 0 };
         Flux(i_hL, i_hR, i_huL, i_huR, out l_fdelta);

         float l_bathymetry = (-m_g) * (i_bR - i_bL) * ((i_hL + i_hR) / 2);

         float[] l_eigencoefficients = new float[2];
         EigencoefficientAlpha(l_inverse, l_fdelta, l_bathymetry,out l_eigencoefficients);

         float[] l_eigens = { l_sL, l_sR };
         Decompose(l_eigencoefficients, l_eigens, out o_minus_A_deltaQ, out o_plus_A_deltaQ);
     }
 }