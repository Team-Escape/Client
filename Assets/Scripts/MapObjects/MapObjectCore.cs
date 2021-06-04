using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TargetMode
{
    None,
    Move,
    YoyoMove,
    Rotate,
    YoyoRotate,
    YoyoMoveWithRotate,
    Transport,
    Trigger,
    ReverseActive,
    DelayTrigger,
}

public class MapObjectCore : MonoBehaviour
{

    public TargetMode mode;
    public bool testEvniorment = false;
    /* -------------------------------------------------------------------------- */
    /*                             Shared variables                               */
    /* -------------------------------------------------------------------------- */
    /// <summary>
    /// If Time Scale Effact.
    /// </summary>
    [HideInInspector]
    [SerializeField] bool unscaledTime = false;
    /// <summary>
    /// Delay sec to start.
    /// </summary>
    [HideInInspector]
    [SerializeField] float startTime = 1f;
    /// <summary>
    /// Finish in duration each round.
    /// </summary>
    [HideInInspector]
    [SerializeField] float duration = 1f;
    /// <summary>
    /// Loop times, -1 equals to infinite.
    /// </summary>
    [HideInInspector]
    [SerializeField] int loopTimes = -1;

    /* -------------------------------------------------------------------------- */
    /*                                  Move                                      */
    /* -------------------------------------------------------------------------- */
    [HideInInspector]
    [SerializeField] Vector2 movePos = Vector2.zero;

    /* -------------------------------------------------------------------------- */
    /*                                 Rotate                                     */
    /* -------------------------------------------------------------------------- */
    [HideInInspector]
    [SerializeField] float rotateAngle = 360;

    /* -------------------------------------------------------------------------- */
    /*                               Transport                                    */
    /* -------------------------------------------------------------------------- */
    [HideInInspector]
    [SerializeField] Transform destination = null;

    /* -------------------------------------------------------------------------- */
    /*                                 Trigger                                    */
    /* -------------------------------------------------------------------------- */
    [HideInInspector]
    [SerializeField] GameObject activeObject = null;
    [HideInInspector]
    [SerializeField] bool activeState = false;
    [HideInInspector]
    [SerializeField] UnityEvent activeFunction = null;


    Vector2 spawnPos = Vector2.zero;

    private void Awake()
    {
        if (testEvniorment)
        {
            Init();
        }
    }

    public void Init()
    {
        spawnPos = transform.position;
        switch (mode)
        {
            case TargetMode.Move:
                Invoke("DoMove", startTime);
                break;
            case TargetMode.YoyoMove:
                Invoke("DoYoyoMove", startTime);
                break;
            case TargetMode.Rotate:
                Invoke("DoRotate", startTime);
                break;
            case TargetMode.YoyoRotate:
                Invoke("DoYoyoRotate", startTime);
                break;
            case TargetMode.YoyoMoveWithRotate:
                DoYoyoMoveWithRotate();
                break;
            case TargetMode.ReverseActive:
                InvokeRepeating("DoReverseActive", startTime, duration);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            switch (mode)
            {
                case TargetMode.Transport:
                    DoTransport(other);
                    break;
                case TargetMode.Trigger:
                    DoTrigger();
                    break;
                case TargetMode.DelayTrigger:
                    Invoke("DoTrigger", startTime);
                    Invoke("DoReverseActive", startTime + duration);
                    break;
            }
        }
    }

    public void DoMove()
    {
        float _dur = duration;
        transform.DOMove(movePos, _dur).SetLoops(loopTimes, LoopType.Restart).SetUpdate(unscaledTime).SetEase(Ease.Linear).SetRelative();
    }
    public void DoYoyoMove()
    {
        float _dur = (duration) / 2;
        transform.DOMove(movePos, _dur).SetLoops(loopTimes, LoopType.Yoyo).SetUpdate(unscaledTime).SetEase(Ease.Linear).SetRelative();
    }
    public void DoRotate()
    {
        float _dur = duration;
        Vector3 _rot = new Vector3(0, 0, rotateAngle);
        transform.DORotate(_rot, _dur).SetEase(Ease.Linear).SetRelative().SetLoops(loopTimes, LoopType.Restart).SetUpdate(unscaledTime);
    }
    public void DoYoyoRotate()
    {
        float _dur = duration / 2;
        transform.DORotate(new Vector3(0, 0, rotateAngle), _dur).SetEase(Ease.Linear).SetUpdate(unscaledTime).SetLoops(loopTimes, LoopType.Yoyo);
    }
    public void DoYoyoMoveWithRotate()
    {
        Invoke("DoYoyoMove", startTime);
        Invoke("DoRotate", startTime);
    }
    public void DoTransport(Collider2D other)
    {
        other.transform.position = destination.position;
    }
    public void DoTrigger()
    {
        if (activeObject != null)
        {
            activeObject.SetActive(activeState);
        }
        else if (activeFunction != null)
        {
            activeFunction.Invoke();
        }
    }
    public void DoReverseActive()
    {
        if (activeObject != null)
        {
            activeObject.SetActive(!activeObject.activeSelf);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MapObjectCore)), CanEditMultipleObjects]
public class MapObjectCoreEditor : Editor
{
    MapObjectCore mapObjectCore;
    public SerializedProperty unscaledTime, startTime, duration, loopTimes;
    public SerializedProperty movePos;
    public SerializedProperty rotateAngle;
    public SerializedProperty destination;
    public SerializedProperty activeObject, activeState, activeFunction;

    void OnEnable()
    {
        mapObjectCore = (MapObjectCore)target;

        unscaledTime = serializedObject.FindProperty("unscaledTime");
        startTime = serializedObject.FindProperty("startTime");
        duration = serializedObject.FindProperty("duration");
        loopTimes = serializedObject.FindProperty("loopTimes");

        movePos = serializedObject.FindProperty("movePos");

        rotateAngle = serializedObject.FindProperty("rotateAngle");

        destination = serializedObject.FindProperty("destination");

        activeObject = serializedObject.FindProperty("activeObject");
        activeState = serializedObject.FindProperty("activeState");
        activeFunction = serializedObject.FindProperty("activeFunction");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.Space();

        if (mapObjectCore.mode != TargetMode.None)
        {
            DrawGlobal();
            EditorGUILayout.Space();
        }

        switch (mapObjectCore.mode)
        {
            case TargetMode.Move:
            case TargetMode.YoyoMove:
                EditorGUILayout.LabelField(mapObjectCore.mode.ToString());
                DrawMove();
                break;
            case TargetMode.Rotate:
            case TargetMode.YoyoRotate:
                EditorGUILayout.LabelField(mapObjectCore.mode.ToString());
                DrawRotate();
                break;
            case TargetMode.YoyoMoveWithRotate:
                EditorGUILayout.LabelField("Yoyo Move & Rotate");
                DrawMove();
                DrawRotate();
                break;
            case TargetMode.Transport:
                EditorGUILayout.LabelField("Transport");
                DrawTransport();
                break;
            case TargetMode.Trigger:
                EditorGUILayout.LabelField("Trigger");
                DrawTrigger();
                break;
            case TargetMode.ReverseActive:
                EditorGUILayout.LabelField("Trigger");
                DrawTrigger();
                break;
            case TargetMode.DelayTrigger:
                EditorGUILayout.LabelField("Trigger");
                DrawTrigger();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DrawGlobal()
    {
        EditorGUILayout.PropertyField(unscaledTime, new GUIContent("Unscaled Time"));
        EditorGUILayout.PropertyField(startTime, new GUIContent("Start Time"));
        EditorGUILayout.PropertyField(duration, new GUIContent("Duration"));
        EditorGUILayout.PropertyField(loopTimes, new GUIContent("Loop Times"));
    }
    void DrawMove()
    {
        EditorGUILayout.PropertyField(movePos, new GUIContent("Move Pos"));
    }

    void DrawRotate()
    {
        EditorGUILayout.PropertyField(rotateAngle, new GUIContent("Rotate Angle"));
    }

    void DrawTransport()
    {
        EditorGUILayout.PropertyField(destination, new GUIContent("Destination"));
    }

    void DrawTrigger()
    {
        EditorGUILayout.PropertyField(activeObject, new GUIContent("Active Object"));
        EditorGUILayout.PropertyField(activeState, new GUIContent("Active State"));
        EditorGUILayout.PropertyField(activeFunction, new GUIContent("Active Function"));
    }
}
#endif