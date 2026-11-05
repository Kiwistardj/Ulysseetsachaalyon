using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Construit la scène du ch1 à partir de Assets/Data/ch1_layout.json.
// Menu : Ulysse > Reconstruire la scène du ch1
public static class Ch1SceneBuilder
{
    const string Art = "Assets/Art/Ch1/";
    const string ScenePath = "Assets/Scenes/Ch1_Perraliere.unity";

    [MenuItem("Ulysse/Reconstruire la scène du ch1")]
    public static string Build()
    {
        var L = JsonUtility.FromJson<Ch1Layout>(File.ReadAllText("Assets/Data/ch1_layout.json"));
        var log = new List<string>();
        SetupTopDownSorting(log);

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Sol
        var sol = new GameObject("Sol");
        foreach (var t in L.tiles)
            Put(sol, t.name, "decors/sol/" + t.name + ".jpg", t.x, t.y, order: -100);

        // Décor trié par les pieds
        var deco = new GameObject("Decor");
        foreach (var b in L.buildings) Put(deco, b.name, "decors/batiments/" + b.name + ".png", b.x + b.w / 2f, b.@base);
        foreach (var t in L.TREES) Put(deco, "Arbre", t.bloom ? "objets/arbre_fleuri.png" : "objets/arbre.png", t.x, t.y);
        foreach (var l in L.LAMPS) Put(deco, "Lampadaire", "objets/lampadaire.png", l.x, l.y);
        foreach (var p in L.PEDS) Put(deco, "FeuPieton", "objets/feu_pieton_rouge.png", p.x, p.y);
        Put(deco, "BoiteAuxLettres", "objets/boite_aux_lettres.png", L.MAILBOX.x, L.MAILBOX.y);
        Put(deco, "Araucaria", "objets/araucaria.png", L.ARAU.x, L.ARAU.y);
        Put(deco, "Fontaine", "objets/fontaine_jets.png", 1060, 352);

        // Quête et objets
        var quest = new GameObject("Quete");
        foreach (var r in L.RONCES) Put(quest, "Ronce_" + r.tag, "objets/ronce.png", r.x, r.y);
        Put(quest, "LocalVelos_Fente", "objets/fente_local_velos_avec_trottinette.png", L.BUSH.x, L.BUSH.y);
        var scooterSprite = new Dictionary<string, string> { { "t1", "trottinette_violette_iris" }, { "t2", "trottinette_orange_victorine" }, { "t3", "trottinette_rose_leonie" } };
        foreach (var it in L.ITEMS)
        {
            var go = Put(quest, "Trottinette_" + it.owner, "vehicules/" + scooterSprite[it.key] + ".png", it.x, it.y);
            if (it.hidden) go.SetActive(false);
        }
        foreach (var h in L.HURT)
            Put(quest, h.id == "herisson" ? "Herisson" : "RougeGorge", h.id == "herisson" ? "animaux/herisson_blesse.png" : "animaux/rougegorge_blesse.png", h.x, h.y, flip: h.face < 0);

        // PNJ
        var npcs = new GameObject("PNJ");
        foreach (var n in L.NPCS) Put(npcs, n.id, "personnages/" + n.id + ".png", n.x, n.y);

        // Voitures (voie 0 vers la gauche, voie 1 vers la droite)
        var cars = new GameObject("Voitures");
        var carSprite = new Dictionary<string, string> { { "#4f8fd8", "voiture_bleue" }, { "#f2b440", "voiture_jaune" }, { "#6cc36a", "voiture_verte" }, { "#e04a3a", "voiture_pompiers" } };
        foreach (var c in L.CARS)
            Put(cars, carSprite[c.color], "vehicules/" + carSprite[c.color] + ".png", c.x, L.LANES[c.lane] + 26, flip: c.lane == 0);

        // Famille au départ
        var fam = new GameObject("Famille");
        var ulysse = Put(fam, "Ulysse", "personnages/ulysse.png", L.P.x, L.P.y);
        Put(fam, "Sacha_sur_Ninou", "animaux/sacha_sur_ninou.png", L.F.sacha.x, L.F.sacha.y);
        Put(fam, "Didou", "animaux/didou.png", L.F.didou.x, L.F.didou.y);
        Put(fam, "Bibou", "animaux/bibou.png", L.F.bibou.x, L.F.bibou.y);
        Put(fam, "Papa", "personnages/papa.png", L.F.papa.x, L.F.papa.y);
        Put(fam, "Maman", "personnages/maman.png", L.F.maman.x, L.F.maman.y);

        // Collisions statiques
        var col = new GameObject("Collisions");
        foreach (var r in L.RECTS)
        {
            var bc = col.AddComponent<BoxCollider2D>();
            bc.offset = World.ToUnity(r.x + r.w / 2f, r.y + r.h / 2f);
            bc.size = new Vector2(r.w / World.Scale, r.h / World.Scale);
        }
        foreach (var p in L.POSTS)
        {
            var cc = col.AddComponent<CircleCollider2D>();
            cc.offset = World.ToUnity(p.x, p.y);
            cc.radius = p.r / World.Scale;
        }

        // Caméra paysage, cadrage proche du HTML (environ 860 x 600 unités HTML visibles)
        var camGo = new GameObject("Main Camera");
        camGo.tag = "MainCamera";
        var cam = camGo.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 300f / World.Scale;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color32(0xa9, 0xd6, 0x90, 0xff); // herbe de la carte
        camGo.transform.position = ulysse.transform.position + new Vector3(0, 0.6f, -10f);
        var cd = camGo.AddComponent<UniversalAdditionalCameraData>();
        cd.renderPostProcessing = true;

        var lightGo = new GameObject("Global Light 2D");
        var l2d = lightGo.AddComponent<Light2D>();
        l2d.lightType = Light2D.LightType.Global;
        l2d.intensity = 1f;

        var volGo = new GameObject("Global Volume");
        var vol = volGo.AddComponent<Volume>();
        vol.isGlobal = true;
        vol.sharedProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>("Assets/Settings/Ch1_Volume.asset");

        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        log.Add("scène enregistrée : " + ScenePath);
        if (missing.Count > 0) log.Add("images manquantes : " + string.Join(", ", missing));
        missing.Clear();
        return string.Join(" | ", log);
    }

    static readonly List<string> missing = new List<string>();

    static GameObject Put(GameObject parent, string name, string sprite, float x, float y, int order = 0, bool flip = false)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform);
        go.transform.position = World.ToUnity(x, y);
        var r = go.AddComponent<SpriteRenderer>();
        r.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Art + sprite);
        if (r.sprite == null) missing.Add(sprite);
        r.sortingOrder = order;
        r.spriteSortPoint = SpriteSortPoint.Pivot;
        r.flipX = flip;
        return go;
    }

    // Vue de dessus : ce qui est plus bas à l'écran passe devant.
    static void SetupTopDownSorting(List<string> log)
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0f, 1f, 0f);
        var guids = AssetDatabase.FindAssets("t:Renderer2DData");
        foreach (var g in guids)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(g));
            var so = new SerializedObject(asset);
            var mode = so.FindProperty("m_TransparencySortMode");
            var axis = so.FindProperty("m_TransparencySortAxis");
            if (mode != null && axis != null)
            {
                mode.enumValueIndex = (int)TransparencySortMode.CustomAxis;
                axis.vector3Value = new Vector3(0f, 1f, 0f);
                so.ApplyModifiedProperties();
                log.Add("tri par axe Y : " + asset.name);
            }
            else log.Add("tri : propriétés introuvables sur " + asset.name);
        }
        AssetDatabase.SaveAssets();
    }
}
