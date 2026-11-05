using System.IO;
using UnityEditor;
using UnityEngine;

// Réglages d'import des images exportées du ch1 HTML (voir docs/ch1-export/LISEZMOI.md).
// Échelle : 50 unités HTML = 1 unité Unity.
//  - personnages, animaux, objets : 4 px par unité HTML -> 200 PPU, pivot aux pieds
//  - sol et bâtiments : 2 px par unité HTML -> 100 PPU
//  - bâtiments : pivot au pied de la façade (base), lu dans Assets/Data/ch1_layout.json
public class Ch1SpriteImport : AssetPostprocessor
{
    const string Root = "Assets/Art/Ch1/";
    const string LayoutPath = "Assets/Data/ch1_layout.json";

    void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith(Root)) return;
        var ti = (TextureImporter)assetImporter;
        ti.textureType = TextureImporterType.Sprite;
        ti.spriteImportMode = SpriteImportMode.Single;
        ti.mipmapEnabled = false;
        ti.alphaIsTransparency = true;
        ti.filterMode = FilterMode.Bilinear;
        ti.maxTextureSize = 2048;
        ti.wrapMode = TextureWrapMode.Clamp;

        var s = new TextureImporterSettings();
        ti.ReadTextureSettings(s);
        s.spriteMeshType = SpriteMeshType.FullRect;

        if (assetPath.Contains("/decors/sol/"))
        {
            s.spritePixelsPerUnit = 100f;
            s.spriteAlignment = (int)SpriteAlignment.TopLeft;
        }
        else if (assetPath.Contains("/decors/batiments/"))
        {
            s.spritePixelsPerUnit = 100f;
            s.spriteAlignment = (int)SpriteAlignment.Custom;
            s.spritePivot = BuildingPivot(Path.GetFileNameWithoutExtension(assetPath));
        }
        else if (assetPath.Contains("/ui/"))
        {
            s.spritePixelsPerUnit = 200f;
            s.spriteAlignment = (int)SpriteAlignment.Center;
        }
        else if (assetPath.Contains("/portraits/") || assetPath.Contains("/scenes/"))
        {
            s.spritePixelsPerUnit = 100f;
            s.spriteAlignment = (int)SpriteAlignment.Center;
        }
        else
        {
            s.spritePixelsPerUnit = 200f;
            s.spriteAlignment = (int)SpriteAlignment.BottomCenter;
        }
        ti.SetTextureSettings(s);
    }

    static Vector2 BuildingPivot(string name)
    {
        if (!File.Exists(LayoutPath)) return new Vector2(0.5f, 0f);
        var layout = JsonUtility.FromJson<Ch1Layout>(File.ReadAllText(LayoutPath));
        foreach (var b in layout.buildings)
            if (b.name == name)
                return new Vector2(0.5f, Mathf.Clamp01((b.y + b.h - b.@base) / b.h));
        return new Vector2(0.5f, 0f);
    }
}
