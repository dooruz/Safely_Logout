using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeleteSaveEditor : EditorWindow
{
    [MenuItem("MyMenu/DeleteSaveFiles")]
    static void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("���̺� ���� �� ����");
    }
}