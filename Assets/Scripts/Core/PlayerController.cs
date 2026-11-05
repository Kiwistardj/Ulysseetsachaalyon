using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

// Ulysse : toucher (ou clic) pour marcher avec contournement, clavier (flèches, ZQSD/WASD), manette.
// Vitesse et rayon repris du HTML : 235 unités/s, rayon 17 (50 unités HTML = 1 unité Unity).
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 235f / World.Scale;
    public Transform visual;          // le sprite (enfant), pour le balancement de marche
    public SpriteRenderer tapMarker;  // rond qui montre où on va

    public static PlayerController Instance { get; private set; }
    public bool CanMove { get; set; } = true;
    public bool Moving { get; private set; }
    public Vector2 Facing { get; private set; } = Vector2.down;

    // Trace des positions, pour les compagnons qui suivent en file (comme P.trail dans le HTML)
    public readonly List<Vector2> Trail = new List<Vector2>();
    const float TrailStep = 3f / World.Scale;
    const int TrailMax = 260;

    Rigidbody2D rb;
    InputAction move;
    List<Vector2> path;
    int pathIndex;
    float stuck, phase, markerT;
    Vector2 lastPos;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        move = new InputAction("Move", InputActionType.Value);
        move.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow").With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow");
        move.AddCompositeBinding("2DVector") // position physique : ZQSD sur clavier français
            .With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
        move.AddBinding("<Gamepad>/leftStick").WithProcessor("stickDeadzone(min=0.28)");
        move.AddBinding("<Gamepad>/dpad");
        move.Enable();
        for (int i = 0; i < 240; i++) Trail.Add((Vector2)transform.position + Vector2.down * i * TrailStep);
        lastPos = rb.position;
    }

    void OnDestroy() { move?.Dispose(); }

    void Update()
    {
        if (CanMove) ReadTap();
        if (tapMarker != null)
        {
            markerT -= Time.deltaTime;
            var c = tapMarker.color; c.a = Mathf.Clamp01(markerT / 0.4f) * 0.8f; tapMarker.color = c;
            tapMarker.transform.localScale = Vector3.one * (1f + 0.15f * Mathf.Sin(Time.time * 8f));
        }
    }

    void ReadTap()
    {
        var p = Pointer.current;
        if (p == null || !p.press.wasPressedThisFrame) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        var world = (Vector2)Camera.main.ScreenToWorldPoint(p.position.ReadValue());
        GoTo(world);
    }

    public void GoTo(Vector2 target)
    {
        path = PathGrid.Instance != null ? PathGrid.Instance.FindPath(rb.position, target) : new List<Vector2> { target };
        pathIndex = 0; stuck = 0;
        if (path == null || path.Count == 0) { path = null; return; }
        if (tapMarker != null) { tapMarker.transform.position = path[path.Count - 1]; markerT = 1.2f; }
    }

    public void Stop() { path = null; rb.linearVelocity = Vector2.zero; }

    void FixedUpdate()
    {
        Vector2 dir = Vector2.zero;
        if (CanMove)
        {
            var input = move.ReadValue<Vector2>();
            if (input.sqrMagnitude > 0.0025f) { path = null; dir = Vector2.ClampMagnitude(input, 1f); }
            else if (path != null)
            {
                var target = path[pathIndex];
                var to = target - rb.position;
                float reach = pathIndex == path.Count - 1 ? 14f / World.Scale : 10f / World.Scale;
                if (to.magnitude < reach)
                {
                    pathIndex++;
                    if (pathIndex >= path.Count) path = null;
                }
                else dir = to.normalized;
            }
        }
        else path = null;

        rb.linearVelocity = dir * speed;
        float moved = (rb.position - lastPos).magnitude;
        // bloqué contre un obstacle en suivant un chemin : on abandonne (comme S.tap.stuck)
        if (path != null && dir != Vector2.zero && moved < speed * Time.fixedDeltaTime * 0.25f)
        {
            stuck += Time.fixedDeltaTime;
            if (stuck > 0.35f) path = null;
        }
        else stuck = 0;
        lastPos = rb.position;

        Moving = dir != Vector2.zero && moved > 0.2f / World.Scale;
        if (dir != Vector2.zero) Facing = dir;

        if (Trail.Count == 0 || (rb.position - Trail[0]).magnitude >= TrailStep)
        {
            Trail.Insert(0, rb.position);
            if (Trail.Count > TrailMax) Trail.RemoveAt(Trail.Count - 1);
        }
    }

    void LateUpdate()
    {
        if (visual == null) return;
        if (Moving) phase += Time.deltaTime * 11f;
        float bob = Moving ? Mathf.Abs(Mathf.Sin(phase)) * 3f / World.Scale : 0f;
        visual.localPosition = new Vector3(0f, bob, 0f);
        if (Mathf.Abs(Facing.x) > 0.3f)
        {
            var sr = visual.GetComponent<SpriteRenderer>();
            if (sr != null) sr.flipX = Facing.x < 0f;
        }
    }

    public Vector2 TrailAt(int i) => Trail.Count == 0 ? (Vector2)transform.position : Trail[Mathf.Min(i, Trail.Count - 1)];
}
