using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(BlockStructure))]
public class BlockStructureSave : Editor
{
    private BlockStructure blockStructure;

    private void OnEnable()
    {
        blockStructure = (BlockStructure)target;
    }

    public override void OnInspectorGUI()
    {
        blockStructure.savedStructure.directory = EditorGUILayout.TextField("Directory", blockStructure.savedStructure.directory);
        blockStructure.savedStructure.configurationData.type =(ConfigurationType) EditorGUILayout.EnumPopup( "Configuration Type", blockStructure.savedStructure.configurationData.type);
        blockStructure.savedStructure.configurationData.distance = EditorGUILayout.IntField(  "Distance",blockStructure.savedStructure.configurationData.distance);
        blockStructure.savedStructure.configurationData.padding = EditorGUILayout.IntField("Padding", blockStructure.savedStructure.configurationData.padding );
        blockStructure.savedStructure.configurationData.height = EditorGUILayout.IntField("Height", blockStructure.savedStructure.configurationData.height );
        blockStructure.savedStructure.configurationData.isTwoDirection = EditorGUILayout.Toggle("Is Two Direction", blockStructure.savedStructure.configurationData.isTwoDirection);

        if (GUILayout.Button("Save"))
        {
            SaveJson();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(blockStructure);
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }
    }

    private void SaveJson()
    {
        string path = Application.dataPath+ blockStructure.savedStructure.directory ;

        if (!Directory.Exists(path))
        {
            DirectoryInfo did = Directory.CreateDirectory(path);
           //File.SetAttributes(path, FileAttributes.Directory);
        }

        File.SetAttributes(path, FileAttributes.Directory);
        path += blockStructure.gameObject.name + ".json";

        Debug.Log(path);

        File.WriteAllText(path , WriteToJson());
        //File.SetAttributes(path, FileAttributes.Directory);
    }

    private string WriteToJson()
    {
        blockStructure.savedStructure.blocks = new List<PositionType>();

        for(int i =0; i< blockStructure.transform.childCount; i++)
        {
            Transform child = blockStructure.transform.GetChild(i);
            if (child.GetComponent<BlockPrefab>())
            {
                GetBlockPrefab(child);
            }
            else
            {
                for (int j = 0; j < child.childCount; j++)
                {
                    Transform child2 = child.GetChild(j);
                    if (child2.GetComponent<BlockPrefab>())
                    {
                        GetBlockPrefab(child2);
                    }
                }
            }
        }

        Debug.Log(JsonUtility.ToJson(blockStructure.savedStructure));

        return JsonUtility.ToJson(blockStructure.savedStructure);
    }

    private void GetBlockPrefab(Transform child)
    {
        Vector3Int position = MyMath.GetGridCoordinate(child.position);
        PositionType block = new PositionType()
        {
            position = position,
            blockType = child.GetComponent<BlockPrefab>().blockType
        };
        blockStructure.savedStructure.blocks.Add(block);
    }
}
