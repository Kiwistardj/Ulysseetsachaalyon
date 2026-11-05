using System.Collections.Generic;
using UnityEngine;

// Grille de marche (A*) calculée depuis les collisions statiques de la scène.
// Une case est libre si un cercle de la taille d'Ulysse y tient sans toucher d'obstacle.
public class PathGrid : MonoBehaviour
{
    [Tooltip("Taille d'une case en unités Unity (20 unités HTML)")]
    public float cell = 0.4f;
    [Tooltip("Rayon d'Ulysse en unités Unity (17 unités HTML)")]
    public float agentRadius = 0.34f;
    public Rect bounds; // zone de la carte, en unités Unity

    int w, h;
    bool[] free;
    static readonly Collider2D[] hits = new Collider2D[16];

    public static PathGrid Instance { get; private set; }
    void Awake() { Instance = this; Rebuild(); }

    public void Rebuild()
    {
        w = Mathf.CeilToInt(bounds.width / cell);
        h = Mathf.CeilToInt(bounds.height / cell);
        free = new bool[w * h];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                free[y * w + x] = !Blocked(CellCenter(x, y));
    }

    // Recalcule une zone (ex. : ronces brûlées)
    public void RebuildAround(Vector2 p, float radius)
    {
        var a = ToCell(p - Vector2.one * radius); var b = ToCell(p + Vector2.one * radius);
        for (int y = Mathf.Max(0, a.y); y <= Mathf.Min(h - 1, b.y); y++)
            for (int x = Mathf.Max(0, a.x); x <= Mathf.Min(w - 1, b.x); x++)
                free[y * w + x] = !Blocked(CellCenter(x, y));
    }

    bool Blocked(Vector2 p)
    {
        int n = Physics2D.OverlapCircle(p, agentRadius, ContactFilter2D.noFilter, hits);
        for (int i = 0; i < n; i++)
        {
            var c = hits[i];
            if (c.isTrigger) continue;
            if (c.attachedRigidbody != null && c.attachedRigidbody.bodyType == RigidbodyType2D.Dynamic) continue; // personnages
            return true;
        }
        return false;
    }

    Vector2 CellCenter(int x, int y) => new Vector2(bounds.xMin + (x + 0.5f) * cell, bounds.yMin + (y + 0.5f) * cell);
    Vector2Int ToCell(Vector2 p) => new Vector2Int(Mathf.FloorToInt((p.x - bounds.xMin) / cell), Mathf.FloorToInt((p.y - bounds.yMin) / cell));
    bool In(Vector2Int c) => c.x >= 0 && c.y >= 0 && c.x < w && c.y < h;
    bool Free(Vector2Int c) => In(c) && free[c.y * w + c.x];

    // Case libre la plus proche (on peut toucher un arbre : on va juste à côté)
    Vector2Int NearestFree(Vector2Int c, int maxRing = 12)
    {
        if (Free(c)) return c;
        for (int r = 1; r <= maxRing; r++)
            for (int dy = -r; dy <= r; dy++)
                for (int dx = -r; dx <= r; dx++)
                {
                    if (Mathf.Abs(dx) != r && Mathf.Abs(dy) != r) continue;
                    var n = new Vector2Int(c.x + dx, c.y + dy);
                    if (Free(n)) return n;
                }
        return new Vector2Int(-1, -1);
    }

    static readonly Vector2Int[] Dirs = {
        new Vector2Int(1,0), new Vector2Int(-1,0), new Vector2Int(0,1), new Vector2Int(0,-1),
        new Vector2Int(1,1), new Vector2Int(1,-1), new Vector2Int(-1,1), new Vector2Int(-1,-1) };

    public List<Vector2> FindPath(Vector2 from, Vector2 to)
    {
        var s = NearestFree(ToCell(from), 3); var g = NearestFree(ToCell(to));
        if (!In(s) || !In(g)) return null;
        int N = w * h, start = s.y * w + s.x, goal = g.y * w + g.x;
        var gScore = new float[N]; var came = new int[N]; var closed = new bool[N];
        for (int i = 0; i < N; i++) { gScore[i] = float.MaxValue; came[i] = -1; }
        gScore[start] = 0;
        var open = new SortedSet<(float, int)>();
        open.Add((H(s, g), start));
        while (open.Count > 0)
        {
            var cur = open.Min; open.Remove(cur);
            int ci = cur.Item2;
            if (ci == goal) break;
            if (closed[ci]) continue;
            closed[ci] = true;
            var c = new Vector2Int(ci % w, ci / w);
            for (int k = 0; k < 8; k++)
            {
                var d = Dirs[k]; var n = c + d;
                if (!Free(n)) continue;
                if (k >= 4 && (!Free(new Vector2Int(c.x + d.x, c.y)) || !Free(new Vector2Int(c.x, c.y + d.y)))) continue; // pas de coin coupé
                int ni = n.y * w + n.x;
                float ng = gScore[ci] + (k >= 4 ? 1.4142f : 1f);
                if (ng < gScore[ni]) { gScore[ni] = ng; came[ni] = ci; open.Add((ng + H(n, g), ni)); }
            }
        }
        if (came[goal] == -1 && goal != start) return null;
        var cells = new List<Vector2Int>();
        for (int i = goal; i != -1; i = came[i]) cells.Add(new Vector2Int(i % w, i / w));
        cells.Reverse();
        // Lissage : on saute les cases visibles en ligne droite
        var pts = new List<Vector2>();
        int a = 0;
        while (a < cells.Count - 1)
        {
            int b = cells.Count - 1;
            while (b > a + 1 && !Line(cells[a], cells[b])) b--;
            pts.Add(CellCenter(cells[b].x, cells[b].y));
            a = b;
        }
        if (pts.Count > 0 && ToCell(to) == g) pts[pts.Count - 1] = to;
        return pts;
    }

    static float H(Vector2Int a, Vector2Int b) { int dx = Mathf.Abs(a.x - b.x), dy = Mathf.Abs(a.y - b.y); return (dx + dy) + (1.4142f - 2f) * Mathf.Min(dx, dy); }

    bool Line(Vector2Int a, Vector2Int b)
    {
        int steps = Mathf.Max(Mathf.Abs(b.x - a.x), Mathf.Abs(b.y - a.y)) * 2;
        for (int i = 0; i <= steps; i++)
        {
            float t = steps == 0 ? 0 : i / (float)steps;
            var p = new Vector2(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t));
            // vérifie les 4 cases autour du point pour garder la marge du rayon
            if (!Free(new Vector2Int(Mathf.FloorToInt(p.x + .5f), Mathf.FloorToInt(p.y + .5f)))) return false;
            if (!Free(new Vector2Int(Mathf.FloorToInt(p.x), Mathf.FloorToInt(p.y)))) return false;
            if (!Free(new Vector2Int(Mathf.CeilToInt(p.x), Mathf.CeilToInt(p.y)))) return false;
        }
        return true;
    }
}
