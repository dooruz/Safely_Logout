using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class game_mainStory_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/YooJung/ExcelData/game/game_mainStory.xlsx";
	private static readonly string exportPath = "Assets/Resources/YooJung/ExcelData/game/game_mainStory.asset";
	private static readonly string[] sheetNames = { "main_g", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			main_g data = (main_g)AssetDatabase.LoadAssetAtPath (exportPath, typeof(main_g));
			if (data == null) {
				data = ScriptableObject.CreateInstance<main_g> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					main_g.Sheet s = new main_g.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						main_g.Param p = new main_g.Param ();
						
					cell = row.GetCell(0); p.index = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.MyTurn = (cell == null ? false : cell.BooleanCellValue);
					cell = row.GetCell(2); p.Script = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.Who = (cell == null ? "" : cell.StringCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
