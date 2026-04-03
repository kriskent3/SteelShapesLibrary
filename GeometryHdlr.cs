using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;

namespace SteelShapesLibrary
{
      public static class GeometryHdlr
      {
                private const string STEEL_LAYER = "STEEL-GEOM";
                private const string GAGE_LAYER = "GAGE-LINES";
                private const double BULGE_90 = 0.41421356;

                public static void CreateShapeBlock(Database db, Transaction tr, SteelShape s, double length)
                {
                              BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                              string blockName = $"{s.Name}_{length}";
                              if (bt.Has(blockName)) return;

                              bt.UpgradeOpen();
                              BlockTableRecord btr = new BlockTableRecord { Name = blockName };
                              bt.Add(btr);
                              tr.AddNewlyCreatedDBObject(btr, true);

                              EnsureLayer(db, tr, STEEL_LAYER, Color.FromColorIndex(ColorMethod.ByAci, 7), "Continuous");
                              EnsureLayer(db, tr, GAGE_LAYER, Color.FromColorIndex(ColorMethod.ByAci, 1), "CENTER");

                              if (s.Type == "W") CreateWProfile(btr, tr, s);
                              else if (s.Type == "C") CreateCProfile(btr, tr, s);
                              else if (s.Type == "L") CreateLProfile(btr, tr, s);

                              CreatePlanView(btr, tr, s, length);
                              CreateSideView(btr, tr, s, length);
                }

                private static void CreateWProfile(BlockTableRecord btr, Transaction tr, SteelShape s)
                {
                              Polyline pl = new Polyline();
                              double d = s.Depth; double bf = s.Width; double tf = s.FlangeThickness; double tw = s.WebThickness;
                              double k = s.KDet; double r_t = s.ToeRadius;
                              double r_f = k - tf; // Internal fillet radius

                              // Start at top-left toe
                              pl.AddVertexAt(0, new Point2d(-bf/2 + r_t, d/2), 0, 0, 0); 
                              pl.AddVertexAt(1, new Point2d(bf/2 - r_t, d/2), -BULGE_90, 0, 0); // Top-right toe start
                              pl.AddVertexAt(2, new Point2d(bf/2, d/2 - r_t), 0, 0, 0);
                              pl.AddVertexAt(3, new Point2d(bf/2, d/2 - tf + r_t), -BULGE_90, 0, 0); // Top-right inner toe start
                              pl.AddVertexAt(4, new Point2d(bf/2 - r_t, d/2 - tf), 0, 0, 0);
                              pl.AddVertexAt(5, new Point2d(tw/2 + r_f, d/2 - tf), BULGE_90, 0, 0); // Top-right web fillet start
                              pl.AddVertexAt(6, new Point2d(tw/2, d/2 - tf - r_f), 0, 0, 0);
                              pl.AddVertexAt(7, new Point2d(tw/2, -d/2 + tf + r_f), BULGE_90, 0, 0); // Bot-right web fillet start
                              pl.AddVertexAt(8, new Point2d(tw/2 + r_f, -d/2 + tf), 0, 0, 0);
                              pl.AddVertexAt(9, new Point2d(bf/2 - r_t, -d/2 + tf), -BULGE_90, 0, 0); // Bot-right inner toe start
                              pl.AddVertexAt(10, new Point2d(bf/2, -d/2 + tf - r_t), 0, 0, 0);
                              pl.AddVertexAt(11, new Point2d(bf/2, -d/2 + r_t), -BULGE_90, 0, 0); // Bot-right toe start
                              pl.AddVertexAt(12, new Point2d(bf/2 - r_t, -d/2), 0, 0, 0);
                              pl.AddVertexAt(13, new Point2d(-bf/2 + r_t, -d/2), -BULGE_90, 0, 0); // Bot-left toe start
                              pl.AddVertexAt(14, new Point2d(-bf/2, -d/2 + r_t), 0, 0, 0);
                              pl.AddVertexAt(15, new Point2d(-bf/2, -d/2 + tf - r_t), -BULGE_90, 0, 0); // Bot-left inner toe start
                              pl.AddVertexAt(16, new Point2d(-bf/2 + r_t, -d/2 + tf), 0, 0, 0);
                              pl.AddVertexAt(17, new Point2d(-tw/2 - r_f, -d/2 + tf), BULGE_90, 0, 0); // Bot-left web fillet start
                              pl.AddVertexAt(18, new Point2d(-tw/2, -d/2 + tf + r_f), 0, 0, 0);
                              pl.AddVertexAt(19, new Point2d(-tw/2, d/2 - tf - r_f), BULGE_90, 0, 0); // Top-left web fillet start
                              pl.AddVertexAt(20, new Point2d(-tw/2 - r_f, d/2 - tf), 0, 0, 0);
                              pl.AddVertexAt(21, new Point2d(-bf/2 + r_t, d/2 - tf), -BULGE_90, 0, 0); // Top-left inner toe start
                              pl.AddVertexAt(22, new Point2d(-bf/2, d/2 - tf + r_t), 0, 0, 0);
                              pl.AddVertexAt(23, new Point2d(-bf/2, d/2 - r_t), -BULGE_90, 0, 0); // Top-left toe start

                              pl.Closed = true; pl.Layer = STEEL_LAYER;
                              btr.AppendEntity(pl); tr.AddNewlyCreatedDBObject(pl, true);
                              AddGageLine(btr, tr, new Point3d(-s.Gage/2, d/2, 0), new Point3d(-s.Gage/2, d/2-tf, 0));
                              AddGageLine(btr, tr, new Point3d(s.Gage/2, d/2, 0), new Point3d(s.Gage/2, d/2-tf, 0));
                }

                private static void CreateCProfile(BlockTableRecord btr, Transaction tr, SteelShape s)
                {
                              Polyline pl = new Polyline();
                              double d = s.Depth; double bf = s.Width; double tf = s.FlangeThickness; double tw = s.WebThickness;
                              double r_f = s.KDet - tw; double r_t = s.ToeRadius;

                              pl.AddVertexAt(0, new Point2d(0, d/2), 0, 0, 0); 
                              pl.AddVertexAt(1, new Point2d(bf - r_t, d/2), -BULGE_90, 0, 0);
                              pl.AddVertexAt(2, new Point2d(bf, d/2 - r_t), 0, 0, 0);
                              pl.AddVertexAt(3, new Point2d(bf, d/2 - tf + r_t), -BULGE_90, 0, 0);
                              pl.AddVertexAt(4, new Point2d(bf - r_t, d/2 - tf), 0, 0, 0);
                              pl.AddVertexAt(5, new Point2d(tw + r_f, d/2 - tf), BULGE_90, 0, 0);
                              pl.AddVertexAt(6, new Point2d(tw, d/2 - tf - r_f), 0, 0, 0);
                              pl.AddVertexAt(7, new Point2d(tw, -d/2 + tf + r_f), BULGE_90, 0, 0);
                              pl.AddVertexAt(8, new Point2d(tw + r_f, -d/2 + tf), 0, 0, 0);
                              pl.AddVertexAt(9, new Point2d(bf - r_t, -d/2 + tf), -BULGE_90, 0, 0);
                              pl.AddVertexAt(10, new Point2d(bf, -d/2 + tf - r_t), 0, 0, 0);
                              pl.AddVertexAt(11, new Point2d(bf, -d/2 + r_t), -BULGE_90, 0, 0);
                              pl.AddVertexAt(12, new Point2d(bf - r_t, -d/2), 0, 0, 0);
                              pl.AddVertexAt(13, new Point2d(0, -d/2), 0, 0, 0);

                              pl.Closed = true; pl.Layer = STEEL_LAYER;
                              btr.AppendEntity(pl); tr.AddNewlyCreatedDBObject(pl, true);
                              AddGageLine(btr, tr, new Point3d(s.Gage, d/2, 0), new Point3d(s.Gage, d/2-tf, 0));
                }

                private static void CreateLProfile(BlockTableRecord btr, Transaction tr, SteelShape s)
                {
                              Polyline pl = new Polyline();
                              double d = s.Depth; double w = s.Width; double t = s.WebThickness;
                              double k = s.KDet; double r_f = k - t; double r_t = s.ToeRadius;

                              pl.AddVertexAt(0, new Point2d(0, d), 0, 0, 0);
                              pl.AddVertexAt(1, new Point2d(t - r_t, d), -BULGE_90, 0, 0);
                              pl.AddVertexAt(2, new Point2d(t, d - r_t), 0, 0, 0);
                              pl.AddVertexAt(3, new Point2d(t, t + r_f), BULGE_90, 0, 0);
                              pl.AddVertexAt(4, new Point2d(t + r_f, t), 0, 0, 0);
                              pl.AddVertexAt(5, new Point2d(w - r_t, t), -BULGE_90, 0, 0);
                              pl.AddVertexAt(6, new Point2d(w, t - r_t), 0, 0, 0);
                              pl.AddVertexAt(7, new Point2d(w, 0), 0, 0, 0);
                              pl.AddVertexAt(8, new Point2d(0, 0), 0, 0, 0);

                              pl.Closed = true; pl.Layer = STEEL_LAYER;
                              btr.AppendEntity(pl); tr.AddNewlyCreatedDBObject(pl, true);
                              AddGageLine(btr, tr, new Point3d(s.Gage, 0, 0), new Point3d(s.Gage, t, 0));
                }

                private static void CreatePlanView(BlockTableRecord btr, Transaction tr, SteelShape s, double length)
                {
                              double w = s.Width;
                              Polyline pl = new Polyline(4);
                              pl.AddVertexAt(0, new Point2d(0, 20 + w), 0, 0, 0);
                              pl.AddVertexAt(1, new Point2d(length, 20 + w), 0, 0, 0);
                              pl.AddVertexAt(2, new Point2d(length, 20), 0, 0, 0);
                              pl.AddVertexAt(3, new Point2d(0, 20), 0, 0, 0);
                              pl.Closed = true; pl.Layer = STEEL_LAYER;
                              btr.AppendEntity(pl); tr.AddNewlyCreatedDBObject(pl, true);
                              AddGageLine(btr, tr, new Point3d(0, 20 + s.Gage, 0), new Point3d(length, 20 + s.Gage, 0));
                }

                private static void CreateSideView(BlockTableRecord btr, Transaction tr, SteelShape s, double length)
                {
                              double d = s.Depth;
                              Polyline pl = new Polyline(4);
                              pl.AddVertexAt(0, new Point2d(0, -20 - d), 0, 0, 0);
                              pl.AddVertexAt(1, new Point2d(length, -20 - d), 0, 0, 0);
                              pl.AddVertexAt(2, new Point2d(length, -20), 0, 0, 0);
                              pl.AddVertexAt(3, new Point2d(0, -20), 0, 0, 0);
                              pl.Closed = true; pl.Layer = STEEL_LAYER;
                              btr.AppendEntity(pl); tr.AddNewlyCreatedDBObject(pl, true);
                }

                private static void AddGageLine(BlockTableRecord btr, Transaction tr, Point3d p1, Point3d p2)
                {
                              Line l = new Line(p1, p2) { Layer = GAGE_LAYER };
                              btr.AppendEntity(l); tr.AddNewlyCreatedDBObject(l, true);
                }

                private static void EnsureLayer(Database db, Transaction tr, string name, Color color, string linetype)
                {
                              LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                              if (!lt.Has(name))
                              {
                                                lt.UpgradeOpen();
                                                LayerTableRecord ltr = new LayerTableRecord { Name = name, Color = color };
                                                LinetypeTable ltt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead);
                                                if (ltt.Has(linetype)) ltr.LinetypeObjectId = ltt[linetype];
                                                lt.Add(ltr); tr.AddNewlyCreatedDBObject(ltr, true);
                              }
                }
      }
}
