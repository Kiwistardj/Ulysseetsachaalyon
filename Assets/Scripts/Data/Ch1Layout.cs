using System;
using UnityEngine;

// Positions du ch1, extraites du code HTML v2 (tools/export-ch1).
// Unités HTML : x vers la droite, y vers le bas. Conversion : World.ToUnity.
[Serializable] public class Ch1Layout
{
    public float WW, WH;
    public Rect2[] RECTS;
    public Circle[] POSTS;
    public Pt[] PEDS, LAMPS, RAMPS;
    public Pt ARAU, METRO, MAILBOX, P;
    public Shop[] SHOPS;
    public Tree[] TREES;
    public Car[] CARS;
    public float[] LANES;
    public Ronce[] RONCES;
    public Circle BUSH;
    public Item[] ITEMS;
    public Hurt[] HURT;
    public Npc[] NPCS;
    public Family F;
    public Tile[] tiles;
    public Building[] buildings;

    [Serializable] public class Pt { public float x, y; }
    [Serializable] public class Rect2 { public float x, y, w, h; }
    [Serializable] public class Circle { public float x, y, r; }
    [Serializable] public class Shop { public float x, y; public string text; }
    [Serializable] public class Tree { public float x, y, seed; public bool bloom; }
    [Serializable] public class Car { public float x, v; public int lane; public string color; public bool fire; }
    [Serializable] public class Ronce { public float x, y, r; public string tag; }
    [Serializable] public class Item { public string key, color, owner, ownerId; public float x, y; public int slot; public bool hidden; }
    [Serializable] public class Hurt { public string id; public float x, y, r, face; public Pt perch; }
    [Serializable] public class Npc { public string id, dir, first, again; public float x, y; }
    [Serializable] public class Member { public float x, y; public int idx; }
    [Serializable] public class Family { public Member sacha, didou, bibou, ninou, papa, maman; }
    [Serializable] public class Tile { public string name; public float x, y, w, h; }
    [Serializable] public class Building { public string name; public float x, y, w, h, @base; }
}

public static class World
{
    // 50 unités HTML = 1 unité Unity ; y inversé.
    public const float Scale = 50f;
    public static Vector3 ToUnity(float x, float y) => new Vector3(x / Scale, -y / Scale, 0f);
    public static Vector2 ToHtml(Vector3 p) => new Vector2(p.x * Scale, -p.y * Scale);
}
