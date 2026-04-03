using System;
using System.Linq;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using Autodesk.AutoCAD.ApplicationServices;

namespace SteelShapesLibrary
{
      public class RibbonManager : IExtensionApplication
      {
                private static SteelShape? _selectedShape;
                public static SteelShape? SelectedShape => _selectedShape;

                public void Initialize()
                {
                              if (ComponentManager.Ribbon == null)
                                                ComponentManager.ItemInitialized += ComponentManager_ItemInitialized;
                              else
                                                CreateRibbon();
                          }

                private void ComponentManager_ItemInitialized(object? sender, RibbonItemEventArgs e)
                {
                              if (ComponentManager.Ribbon != null)
                                            {
                                                ComponentManager.ItemInitialized -= ComponentManager_ItemInitialized;
                                                CreateRibbon();
                                            }
                          }

                private void CreateRibbon()
                {
                              RibbonControl rc = ComponentManager.Ribbon;
                              RibbonTab? tab = rc.Tabs.FirstOrDefault(t => t.Title == "STEEL LIBRARY");
                              if (tab == null)
                                            {
                                                tab = new RibbonTab { Title = "STEEL LIBRARY", Id = "STEEL_LIBRARY_TAB" };
                                                rc.Tabs.Add(tab);
                                            }

                              RibbonPanelSource source = new RibbonPanelSource { Title = "AISC SHAPES" };
                              RibbonPanel panel = new RibbonPanel { Source = source };
                              tab.Panels.Add(panel);

                              // High-Visibility Search/Select Button
                              RibbonButton selectBtn = new RibbonButton
                              {
                                                Text = "Select\nSteel Shape",
                                                ShowText = true,
                                                ShowImage = true,
                                                CommandParameter = "STEELSELECT ",
                                                CommandHandler = new RelayCommand()
                                            };
                              source.Items.Add(selectBtn);

                              // Quick Insertion Button (recycles the selection)
                              RibbonButton placeBtn = new RibbonButton
                              {
                                                Text = "Place\nSelected",
                                                ShowText = true,
                                                ShowImage = true,
                                                CommandParameter = "STEELINSERT ",
                                                CommandHandler = new RelayCommand()
                                            };
                              source.Items.Add(new RibbonSeparator());
                              source.Items.Add(placeBtn);

                              tab.IsActive = true;
                          }

                [CommandMethod("STEELSELECT")]
                public void LaunchSelectionWindow()
                {
                              using (var form = new ShapeSelectionForm())
                              {
                                                var result = Application.ShowModalDialog(form);
                                                if (result == System.Windows.Forms.DialogResult.OK)
                                                                  {
                                                                      _selectedShape = form.SelectedShape;
                                                                      if (_selectedShape != null)
                                                                                            {
                                                                                                var doc = Application.DocumentManager.MdiActiveDocument;
                                                                                                if (doc != null)
                                                                                                                              doc.Editor.WriteMessage($"\nActive Shape: {_selectedShape.Name}");
                                                                                            }
                                                                  }
                                            }
                          }

                public void Terminate() { }
            }

      public class RelayCommand : System.Windows.Input.ICommand
      {
                public bool CanExecute(object? parameter) => true;
                public event EventHandler? CanExecuteChanged;
                public void Execute(object? parameter)
                {
                              if (parameter is RibbonButton btn)
                                            {
                                                var doc = Application.DocumentManager.MdiActiveDocument;
                                                if (doc != null && btn.CommandParameter != null)
                                                                  {
                                                                      doc.SendStringToExecute((string)btn.CommandParameter, true, false, true);
                                                                  }
                                            }
                          }
            }
  }
