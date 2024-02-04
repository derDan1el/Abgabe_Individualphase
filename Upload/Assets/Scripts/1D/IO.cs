using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
public class IO 
{
        public static List<float> Read(string filename, int columnIndex)
        {
            List<float> selectedColumn = new List<float>();

            try
            {
                using (StreamReader file = new StreamReader(filename))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] tokens = line.Split(',');

                        if (columnIndex < tokens.Length)
                        {
                            string token = tokens[columnIndex].Replace(',', '.');

                            if (float.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out float value))
                            {
                                selectedColumn.Add(value);
                            }
                            else
                            {
                                // Handle the case where parsing fails
                                Console.WriteLine($"Warning: Failed to parse value at column index {columnIndex} for line: {line}");
                            }
                        }
                        else
                        {
                            // Handle the case where columnIndex is out of bounds for the current line
                            Console.WriteLine($"Warning: Column index {columnIndex} is out of bounds for line: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {filename}\n{ex.Message}");
            }

            return selectedColumn;
        }
        
        public static  void Write2d(float i_dxy,
                                 uint i_nx,
                                 uint i_ny,
                                 uint i_stride,
                                 float i_domainstart_x,
                                 float i_domainstart_y,
                                 float[] i_h,
                                 float[] i_hu,
                                 float[] i_hv,
                                 float[] i_b,
                                 StreamWriter io_stream)


    {
        uint l_id = 0;
        CultureInfo culture = CultureInfo.InvariantCulture;
        // Write the CSV header
        io_stream.WriteLine("x,y" +
                         (i_h != null ? ",height" : "") +
                         (i_hu != null ? ",momentum_x" : "") +
                         (i_hv != null ? ",momentum_y" : "") +
                         (i_b != null ? ",bathymetry" : ""));//+
                           // (l_id != null ? ",l_id" : ""));


        for (uint l_iy = 1; l_iy < i_ny + 1; l_iy++)
        {
            for (uint l_ix = 1; l_ix < i_nx + 1; l_ix++)
            {
                // Derive coordinates
                float l_posX = ((l_ix - 1 + 0.5f) * i_dxy) + i_domainstart_x;
                float l_posY = ((l_iy - 1 + 0.5f) * i_dxy) + i_domainstart_y;

           

                 l_id = (l_iy * i_stride) + l_ix;

                 if(l_posX == 45.5f){
                    //Console.WriteLine("l_ix: " + l_ix + " " +  "l_iy: " + l_iy + " " +  "l_id: " + l_id);
                 }

                io_stream.WriteLine($"{l_posX.ToString(culture)},{l_posY.ToString(culture)}" +
                                              (i_h != null ? $",{i_h[l_id].ToString(culture)}" : "") +
                                              (i_hu != null ? $",{i_hu[l_id].ToString(culture)}" : "") +
                                              (i_hv != null ? $",{i_hv[l_id].ToString(culture)}" : "") +
                                              (i_b != null ? $",{i_b[l_id].ToString(culture)}" : "")); //+
                                              //(l_id != null ? $",{l_id.ToString(culture)}" : ""));
            }
        }

        io_stream.Flush();


    }

     public static  void Write1d(float i_dxy,
                                 uint i_nx,
                                 uint i_ny,
                                 uint i_stride,
                                 float i_domainstart_x,
                                 float[] i_h,
                                 float[] i_hu,
                                 float[] i_b,
                                 StreamWriter io_stream)
    {
        uint l_id = 0;
        CultureInfo culture = CultureInfo.InvariantCulture;
        // Write the CSV header
        io_stream.WriteLine("x,y" +
                         (i_h != null ? ",height" : "") +
                         (i_hu != null ? ",momentum_x" : "") +
                         (i_b != null ? ",bathymetry" : ""));//+
                           // (l_id != null ? ",l_id" : ""));


        for (uint l_iy = 0; l_iy < i_ny ; l_iy++)
        {
            for (uint l_ix = 0; l_ix < i_nx ; l_ix++)
            {
                // Derive coordinates
                float l_posX = ((l_ix  + 0.5f) * i_dxy) + i_domainstart_x;
                float l_posY = ((l_iy  + 0.5f) * i_dxy) ;

           

                 l_id = (l_iy * i_stride) + l_ix;

                 if(l_posX == 45.5f){
                    //Console.WriteLine("l_ix: " + l_ix + " " +  "l_iy: " + l_iy + " " +  "l_id: " + l_id);
                 }

                io_stream.WriteLine($"{l_posX.ToString(culture)},{l_posY.ToString(culture)}" +
                                              (i_h != null ? $",{i_h[l_id].ToString(culture)}" : "") +
                                              (i_hu != null ? $",{i_hu[l_id].ToString(culture)}" : "") +
                                              (i_b != null ? $",{i_b[l_id].ToString(culture)}" : "")); //+
                                              //(l_id != null ? $",{l_id.ToString(culture)}" : ""));
            }
        }

        io_stream.Flush();


    }  
}