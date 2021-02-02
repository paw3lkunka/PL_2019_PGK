using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class SaveRenderTextureToFile
{
    [MenuItem("Assets/Save RenderTexture to file")]
    public static void SaveRTToFile()
    {
        Camera cam = Selection.activeGameObject.GetComponent<Camera>();
        RenderTexture rt = cam.targetTexture;

        RenderTexture newRT = new RenderTexture(rt.width, rt.height, rt.depth, rt.format, RenderTextureReadWrite.sRGB);
        cam.targetTexture = newRT;

        cam.Render();

        RenderTexture.active = newRT;
        Texture2D tex = new Texture2D(newRT.width, newRT.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, newRT.width, newRT.height), 0, 0);
        RenderTexture.active = null;

        cam.targetTexture = rt;

        byte[] bytes;
        bytes = tex.EncodeToPNG();

        string path = "../rendertexture.png";
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path);
        Debug.Log("Saved to " + path);
    }

    [MenuItem("Assets/Save transparent RenderTexture to file")]
    public static void SaveTransparentRTToFile()
    {
        Camera cam = Selection.activeGameObject.GetComponent<Camera>();
        RenderTexture rt = cam.targetTexture;

        RenderTexture newRT = new RenderTexture(rt.width, rt.height, rt.depth, rt.format, RenderTextureReadWrite.sRGB);
        cam.targetTexture = newRT;

        cam.Render();

        RenderTexture.active = newRT;
        Texture2D tex = new Texture2D(newRT.width, newRT.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, newRT.width, newRT.height), 0, 0);
        RenderTexture.active = null;

        cam.targetTexture = rt;

        byte[] bytes;
        bytes = tex.EncodeToPNG();

        string path = "../rendertexture.png";
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path);
        Debug.Log("Saved to " + path);
    }

    [MenuItem("Assets/Save RenderTexture to file", true)]
    public static bool SaveRTToFileValidation()
    {
        return Selection.activeGameObject.GetComponent<Camera>() is Camera;
    }
}
#endif