using UnityEditor;
using UnityEngine;

// Réglages d'import des images exportées du ch1 HTML (voir docs/ch1-export/LISEZMOI.md).
// Échelle commune : 4 px par unité du jeu HTML, 50 unités HTML = 1 unité Unity.
public class Ch1SpriteImport : AssetPostprocessor
{
    const string Root = "Assets/Art/Ch1/";

    void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith(Root)) return;
        var ti = (TextureImporter)assetImporter;
        ti.textureType = TextureImporterType.Sprite;
        ti.spriteImportMode = SpriteImportMode.Single;
        ti.mipmapEnabled = false;
        ti.alphaIsTransparency = true;
        ti.filterMode = FilterMode.Bilinear;
        ti.maxTextureSize = 4096;

        var s = new TextureImporterSettings();
        ti.ReadTextureSettings(s);
        s.spriteMeshType = SpriteMeshType.FullRect;

        if (assetPath.Contains("/decors/"))
        {
            s.spritePixelsPerUnit = 50f;           // 1 px par unité HTML
            s.spriteAlignment = (int)SpriteAlignment.BottomLeft;
        }
        else if (assetPath.Contains("/portraits/") || assetPath.Contains("/scenes/"))
        {
            s.spritePixelsPerUnit = 100f;
            s.spriteAlignment = (int)SpriteAlignment.Center;
        }
        else
        {
            s.spritePixelsPerUnit = 200f;          // 4 px par unité HTML
            s.spriteAlignment = (int)SpriteAlignment.BottomCenter; // les pieds
        }
        ti.SetTextureSettings(s);
    }
}
