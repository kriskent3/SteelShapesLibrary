using System.Collections.Generic;

namespace SteelShapesLibrary
{
      public class SteelShape
      {
                public string Name { get; set; } = "";
                public string Type { get; set; } = ""; // W, C, L
                public double Depth { get; set; }
                public double Width { get; set; }
                public double WebThickness { get; set; }
                public double FlangeThickness { get; set; }
                public double KDet { get; set; }     // Distance to flange-web fillet end
                public double ToeRadius { get; set; } // Radius of the flange tip
                public double Gage { get; set; }
      }

      public static class AiscDatabase
      {
                public static List<SteelShape> AllShapes = new List<SteelShape>();

                static AiscDatabase()
                {
                              // --- W-Beams (W4 to W18) ---
                              AllShapes.Add(new SteelShape { Name = "W4x13", Type = "W", Depth = 4.16, Width = 4.06, WebThickness = 0.28, FlangeThickness = 0.345, KDet = 0.625, ToeRadius = 0.125, Gage = 2.25 });
                              AllShapes.Add(new SteelShape { Name = "W5x16", Type = "W", Depth = 5.01, Width = 5.00, WebThickness = 0.24, FlangeThickness = 0.360, KDet = 0.750, ToeRadius = 0.125, Gage = 3.50 });
                              AllShapes.Add(new SteelShape { Name = "W6x15", Type = "W", Depth = 5.99, Width = 5.99, WebThickness = 0.23, FlangeThickness = 0.260, KDet = 0.625, ToeRadius = 0.125, Gage = 3.50 });
                              AllShapes.Add(new SteelShape { Name = "W8x24", Type = "W", Depth = 7.93, Width = 6.49, WebThickness = 0.245, FlangeThickness = 0.400, KDet = 0.8125, ToeRadius = 0.125, Gage = 3.50 });
                              AllShapes.Add(new SteelShape { Name = "W10x30", Type = "W", Depth = 10.47, Width = 5.81, WebThickness = 0.30, FlangeThickness = 0.510, KDet = 0.9375, ToeRadius = 0.125, Gage = 3.50 });
                              AllShapes.Add(new SteelShape { Name = "W12x26", Type = "W", Depth = 12.22, Width = 6.49, WebThickness = 0.23, FlangeThickness = 0.380, KDet = 0.875, ToeRadius = 0.125, Gage = 3.50 });
                              AllShapes.Add(new SteelShape { Name = "W14x22", Type = "W", Depth = 13.74, Width = 5.00, WebThickness = 0.23, FlangeThickness = 0.335, KDet = 0.875, ToeRadius = 0.125, Gage = 3.50 });
                              AllShapes.Add(new SteelShape { Name = "W16x31", Type = "W", Depth = 15.88, Width = 5.52, WebThickness = 0.275, FlangeThickness = 0.440, KDet = 1.000, ToeRadius = 0.125, Gage = 3.50 });
                              AllShapes.Add(new SteelShape { Name = "W18x35", Type = "W", Depth = 17.70, Width = 6.00, WebThickness = 0.30, FlangeThickness = 0.425, KDet = 1.0625, ToeRadius = 0.125, Gage = 3.50 });

                              // --- Channels (C3 to C15) ---
                              AllShapes.Add(new SteelShape { Name = "C3x6", Type = "C", Depth = 3.00, Width = 1.59, WebThickness = 0.356, FlangeThickness = 0.273, KDet = 0.5625, ToeRadius = 0.125, Gage = 1.125 });
                              AllShapes.Add(new SteelShape { Name = "C4x5.4", Type = "C", Depth = 4.00, Width = 1.58, WebThickness = 0.18, FlangeThickness = 0.296, KDet = 0.625, ToeRadius = 0.125, Gage = 1.125 });
                              AllShapes.Add(new SteelShape { Name = "C5x6.7", Type = "C", Depth = 5.00, Width = 1.75, WebThickness = 0.19, FlangeThickness = 0.320, KDet = 0.625, ToeRadius = 0.125, Gage = 1.125 });
                              AllShapes.Add(new SteelShape { Name = "C6x8.2", Type = "C", Depth = 6.00, Width = 1.92, WebThickness = 0.20, FlangeThickness = 0.343, KDet = 0.6875, ToeRadius = 0.125, Gage = 1.125 });
                              AllShapes.Add(new SteelShape { Name = "C8x11.5", Type = "C", Depth = 8.00, Width = 2.26, WebThickness = 0.22, FlangeThickness = 0.390, KDet = 0.750, ToeRadius = 0.125, Gage = 1.375 });
                              AllShapes.Add(new SteelShape { Name = "C10x15.3", Type = "C", Depth = 10.00, Width = 2.60, WebThickness = 0.24, FlangeThickness = 0.436, KDet = 0.8125, ToeRadius = 0.125, Gage = 1.50 });
                              AllShapes.Add(new SteelShape { Name = "C12x20.7", Type = "C", Depth = 12.00, Width = 3.00, WebThickness = 0.28, FlangeThickness = 0.501, KDet = 1.125, ToeRadius = 0.125, Gage = 1.75 });
                              AllShapes.Add(new SteelShape { Name = "C15x33.9", Type = "C", Depth = 15.00, Width = 3.40, WebThickness = 0.40, FlangeThickness = 0.650, KDet = 1.250, ToeRadius = 0.125, Gage = 2.00 });

                              // --- Angles (L1 to L6) ---
                              AllShapes.Add(new SteelShape { Name = "L1x1x1/8", Type = "L", Width = 1.00, Depth = 1.00, WebThickness = 0.125, FlangeThickness = 0.125, KDet = 0.375, ToeRadius = 0.0625, Gage = 0.625 });
                              AllShapes.Add(new SteelShape { Name = "L2x2x1/4", Type = "L", Width = 2.00, Depth = 2.00, WebThickness = 0.25, FlangeThickness = 0.25, KDet = 0.625, ToeRadius = 0.09375, Gage = 1.125 });
                              AllShapes.Add(new SteelShape { Name = "L3x3x3/8", Type = "L", Width = 3.00, Depth = 3.00, WebThickness = 0.375, FlangeThickness = 0.375, KDet = 0.8125, ToeRadius = 0.125, Gage = 1.75 });
                              AllShapes.Add(new SteelShape { Name = "L4x4x1/2", Type = "L", Width = 4.00, Depth = 4.00, WebThickness = 0.50, FlangeThickness = 0.50, KDet = 1.000, ToeRadius = 0.125, Gage = 2.50 });
                              AllShapes.Add(new SteelShape { Name = "L5x5x1/2", Type = "L", Width = 5.00, Depth = 5.00, WebThickness = 0.50, FlangeThickness = 0.50, KDet = 1.125, ToeRadius = 0.125, Gage = 3.00 });
                              AllShapes.Add(new SteelShape { Name = "L6x6x1/2", Type = "L", Width = 6.00, Depth = 6.00, WebThickness = 0.50, FlangeThickness = 0.50, KDet = 1.250, ToeRadius = 0.125, Gage = 3.50 });
                }
      }
}
