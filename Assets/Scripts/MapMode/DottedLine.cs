using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    #region Variables

    private static DottedLine instance;

    public static DottedLine Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DottedLine>();

            return instance;
        }
    }

    private float size = 0.3f;
    private float delta = 0.5f;

    public Sprite Dot;

    private List<Vector2> positions = new List<Vector2>();
    private List<GameObject> dots = new List<GameObject>();

    #endregion

    #region MonoBehaviour

    void FixedUpdate()
    {
        if (positions.Count > 0)
        {
            DestroyAllDots();
            positions.Clear();
        }
    }

    #endregion

    #region Component

    public void DrawDottedLine(Vector2 start, Vector2 end)
    {
        DestroyAllDots();

        Vector2 point = start;
        Vector2 direction = (end - start).normalized;

        while ((end - start).magnitude > (point - start).magnitude)
        {
            positions.Add(point);
            point += (direction * transform.localScale.x * delta);
        }

        Render();
    }

    private void Render()
    {
        foreach (var position in positions)
        {
            var g = GetOneDot();
            g.transform.position = position;
            dots.Add(g);
        }
    }

    private GameObject GetOneDot()
    {
        var gameObject = new GameObject();
        gameObject.transform.localScale = transform.localScale * size;
        gameObject.transform.parent = transform;

        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Dot;
        return gameObject;
    }

    private void DestroyAllDots()
    {
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
    }

    #endregion
}
