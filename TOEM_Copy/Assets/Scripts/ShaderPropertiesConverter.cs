using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

public enum PropertyType{Texture, Float, Vector, Color}

public class PropertyData
{
    public string source;
    public string dest;
    public int selectionIndex;
    public PropertyType type;
}

public class ShaderPropertiesConverter : EditorWindow
{
    Shader shaderSource;
    Shader shaderDest;

    static List<PropertyData> propertyData = new List<PropertyData>();
    List<string> shaderDestPro = new List<string>();

    [MenuItem("Tools/Shader Properties Converter")]
    static void Init()
    {
        ShaderPropertiesConverter window = (ShaderPropertiesConverter)EditorWindow.GetWindow(typeof(ShaderPropertiesConverter));
        window.Show();
    }

    void OnEnable()
    {
        propertyData.Clear();
        shaderDestPro.Add("None");
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        shaderSource = (Shader)EditorGUILayout.ObjectField("Shader From", shaderSource, typeof(Shader), false);
        shaderDest = (Shader)EditorGUILayout.ObjectField("Shader To", shaderDest, typeof(Shader), false);
        if (EditorGUI.EndChangeCheck())
        {
            propertyData.Clear();
            if (shaderSource == null || shaderDest == null)
                return;

            for (int i = 0; i < shaderSource.GetPropertyCount(); i++)
            {
                propertyData.Add(new PropertyData());
            }

            for (int i = 0; i < shaderDest.GetPropertyCount(); i++)
            {
                shaderDestPro.Add(shaderDest.GetPropertyName(i));
            }
        }

        if (shaderSource == null || shaderDest == null)
            return;

        if (propertyData.Count == shaderSource.GetPropertyCount())
        {
            for (int i = 0; i < shaderSource.GetPropertyCount(); i++)
            {
                EditorGUILayout.BeginHorizontal();
                propertyData[i].source = shaderSource.GetPropertyName(i);
                propertyData[i].selectionIndex = EditorGUILayout.Popup(propertyData[i].source, propertyData[i].selectionIndex, shaderDestPro.ToArray());
                propertyData[i].dest = shaderDestPro[propertyData[i].selectionIndex];
                propertyData[i].type = (PropertyType)EditorGUILayout.EnumPopup(propertyData[i].type, GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Change Shader"))
        {
            UpgradeProjectFolder(propertyData, shaderDest.name);
            var materialUpgrader = new MaterialUpgrader();
            materialUpgrader.RenameShader(shaderSource.name, shaderDest.name);
        }
    }

    static bool IsMaterialPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(nameof(path));
        }
        return path.EndsWith(".mat", StringComparison.OrdinalIgnoreCase);
    }

    static bool ShouldUpgradeShader(Material material, string shaderNamesToIgnore)
    {
        if (material == null)
            return false;

        if (material.shader == null)
            return false;

        return !shaderNamesToIgnore.Contains(material.shader.name);
    }

    public void UpgradeProjectFolder(List<PropertyData> properties, string newShader)
    {
        Debug.Log("Changing");
        if ((!Application.isBatchMode) && (!EditorUtility.DisplayDialog(DialogText.title, "The upgrade will overwrite materials in your project. " + DialogText.projectBackMessage, DialogText.proceed, DialogText.cancel)))
            return;

        int totalMaterialCount = 0;
        foreach (string s in UnityEditor.AssetDatabase.GetAllAssetPaths())
        {
            if (IsMaterialPath(s))
                totalMaterialCount++;
        }

        int materialIndex = 0;
        foreach (string path in UnityEditor.AssetDatabase.GetAllAssetPaths())
        {
            if (IsMaterialPath(path))
            {
                materialIndex++;
                if (UnityEditor.EditorUtility.DisplayCancelableProgressBar("Updating", string.Format("({0} of {1}) {2}", materialIndex, totalMaterialCount, path), (float)materialIndex / (float)totalMaterialCount))
                    break;

                Material m = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path) as Material;

                if (!ShouldUpgradeShader(m, newShader))
                    continue;

                Upgrade(m, newShader, properties);

                //SaveAssetsAndFreeMemory();
            }
        }
    }

    internal void Upgrade(Material material, string newShader, List<PropertyData> properties)
    {
        Material newMaterial;
        newMaterial = new Material(Shader.Find(newShader));

        Convert(material, newMaterial, properties);

        material.shader = Shader.Find(newShader);
        material.CopyPropertiesFromMaterial(newMaterial);
        UnityEngine.Object.DestroyImmediate(newMaterial);
    }

    public virtual void Convert(Material srcMaterial, Material dstMaterial, List<PropertyData> properties)
    {
        foreach (var property in properties)
        {
            if (property.selectionIndex == 0)
                return;
            
            switch (property.type)
            {
                case PropertyType.Texture: RenameTexture(property);
                    break;
                case PropertyType.Float: RenameFloat(property);
                    break;
                case PropertyType.Vector: RenameVector(property);
                    break;
                case PropertyType.Color: RenameColor(property);
                    break;
            }
        }

        void RenameTexture(PropertyData propertyData)
        {
            dstMaterial.SetTextureScale(propertyData.dest, srcMaterial.GetTextureScale(propertyData.source));
            dstMaterial.SetTextureOffset(propertyData.dest, srcMaterial.GetTextureOffset(propertyData.source));
            dstMaterial.SetTexture(propertyData.dest, srcMaterial.GetTexture(propertyData.source));
        }

        void RenameFloat(PropertyData propertyData)
        {
            dstMaterial.SetFloat(propertyData.dest, srcMaterial.GetFloat(propertyData.source));
        }

        void RenameVector(PropertyData propertyData)
        {
            dstMaterial.SetVector(propertyData.dest, srcMaterial.GetVector(propertyData.source));
        }

        void RenameColor(PropertyData propertyData)
        {
            dstMaterial.SetColor(propertyData.dest, srcMaterial.GetColor(propertyData.source));
        }
    }
}

class CustomUpgrader : MaterialUpgrader
{
    public CustomUpgrader(string sourceShaderName, string destShaderName, MaterialFinalizer finalizer = null)
    {
        RenameShader(sourceShaderName, destShaderName, finalizer);
    }
}