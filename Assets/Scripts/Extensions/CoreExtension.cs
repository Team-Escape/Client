using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoreExtension
{
    /// <param name="Camera">
    /// Resize all camera rect and field from 1 to 4 players.
    ///</param>
    public static void Resize(this List<Camera> source)
    {
        switch (source.Count)
        {
            case 1:
                source[0].UpdateRectAndField(0, 0, 1, 1, 60);
                break;
            case 2:
                source[0].UpdateRectAndField(0, 0, 1, 0.5f, 65);
                source[1].UpdateRectAndField(0, 0.5f, 1, 0.5f, 65);
                break;
            case 3:
                source[0].UpdateRectAndField(0, 0, 0.5f, 0.5f, 67);
                source[1].UpdateRectAndField(0.5f, 0, 0.5f, 0.5f, 67);
                source[2].UpdateRectAndField(0f, 0.5f, 1f, 0.5f, 67);
                break;
            case 4:
                source[0].UpdateRectAndField(0, 0, 0.5f, 0.5f, 70);
                source[1].UpdateRectAndField(0.5f, 0, 0.5f, 0.5f, 70);
                source[2].UpdateRectAndField(0f, 0.5f, 0.5f, 0.5f, 70);
                source[3].UpdateRectAndField(0.5f, 0.5f, 0.5f, 0.5f, 70);
                break;
        }
    }

    public static void UpdateRectAndField<T>(this T soruce, float x, float y, float w, float h, float field) where T : Component
    {
        Camera cam = soruce.transform.parent.gameObject.GetComponentInChildren<Camera>();

        cam.rect = new Rect(x, y, w, h);

        cam.fieldOfView = field;
    }

    /// <param name="Function Delay">
    /// Delay func secs.
    /// </param>
    public static void AbleToDo<T>(this T tweener, float sec, System.Action callback) where T : MonoBehaviour
    {
        tweener.StartCoroutine(DelaySec(sec, callback));
    }
    public static IEnumerator DelaySec(float sec, System.Action callback)
    {
        yield return new WaitForSeconds(sec);
        callback();
    }
}
