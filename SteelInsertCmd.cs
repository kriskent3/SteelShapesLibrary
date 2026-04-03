using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace SteelShapesLibrary
{
      public class SteelInsertCmd
      {
                [CommandMethod("STEELINSERT")]
                public void InsertSteelShape()
                {
                              Document doc = Application.DocumentManager.MdiActiveDocument;
                              Database db = doc.Database;
                              Editor ed = doc.Editor;

                              SteelShape? shape = RibbonManager.SelectedShape;
                              if (shape == null)
                              {
                                                ed.WriteMessage("\nPlease select a steel shape from the Ribbon dropdown first.");
                                                return;
                              }

                              // 1. Prompt for Insertion Point
                              PromptPointOptions ppo = new PromptPointOptions("\nPick insertion point: ");
                              PromptPointResult ppr = ed.GetPoint(ppo);
                              if (ppr.Status != PromptStatus.OK) return;
                              Point3d insertPt = ppr.Value;

                              // 2. Prompt for Length
                              PromptDoubleOptions pdo = new PromptDoubleOptions("\nEnter length for Plan/Side views (inches): ");
                              pdo.DefaultValue = 12.0;
                              pdo.UseDefaultValue = true;
                              PromptDoubleResult pdr = ed.GetDouble(pdo);
                              if (pdr.Status != PromptStatus.OK) return;
                              double length = pdr.Value;

                              using (Transaction tr = db.TransactionManager.StartTransaction())
                              {
                                                BlockTableRecord ms = (BlockTableRecord)tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite);

                                                // Create Block Definition
                                                GeometryHdlr.CreateShapeBlock(db, tr, shape, length);

                                                // Insert Block Reference
                                                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                                                string blockName = $"{shape.Name}_{length}";
                                                if (bt.Has(blockName))
                                                {
                                                                      BlockReference br = new BlockReference(insertPt, bt[blockName]);
                                                                      ms.AppendEntity(br);
                                                                      tr.AddNewlyCreatedDBObject(br, true);
                                                }

                                                tr.Commit();
                                                ed.WriteMessage($"\nPlaced {shape.Name} at length {length}\".");
                              }
                }
      }
}
