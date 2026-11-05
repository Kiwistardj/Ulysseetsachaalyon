using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Builds du jeu. Menu : Ulysse > Build web / Build Android
public static class BuildTools
{
    static readonly string[] Scenes = { "Assets/Scenes/Ch1_Perraliere.unity" };

    static void CommonSettings()
    {
        PlayerSettings.companyName = "Kiwistar";
        PlayerSettings.productName = "Ulysse, Sacha et Bibou";
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
    }

    [MenuItem("Ulysse/Build web")]
    public static string BuildWeb()
    {
        CommonSettings();
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        // Téléphone Android d'abord : textures ASTC (décompressées à la volée sur PC)
        EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.ASTC;
        // Hébergement sans en-têtes spéciaux : gzip + décompression par le chargeur
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.dataCaching = false;
        PlayerSettings.WebGL.template = "APPLICATION:Minimal";
        var r = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = Scenes, locationPathName = "Builds/Web", target = BuildTarget.WebGL, options = BuildOptions.None
        });
        return Report(r);
    }

    [MenuItem("Ulysse/Build Android")]
    public static string BuildAndroid()
    {
        CommonSettings();
        PlayerSettings.SetApplicationIdentifier(UnityEditor.Build.NamedBuildTarget.Android, "fr.kiwistar.ulyssesachabibou");
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        var r = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = Scenes, locationPathName = "Builds/Android/UlysseSachaBibou.apk", target = BuildTarget.Android, options = BuildOptions.None
        });
        return Report(r);
    }

    static string Report(BuildReport r)
    {
        var s = r.summary;
        return $"{s.result} | {s.totalSize / 1048576f:0.0} Mo | {s.totalTime.TotalSeconds:0} s | erreurs {s.totalErrors} | avertissements {s.totalWarnings}";
    }
}
