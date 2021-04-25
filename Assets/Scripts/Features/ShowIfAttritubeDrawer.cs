﻿using System.Reflection;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
public class ShowIfAttributeDrawer : PropertyDrawer
{

    #region Reflection helpers.
    private static MethodInfo GetMethod(object target, string methodName)
    {
        return GetAllMethods(target, m => m.Name.Equals(methodName,
                  StringComparison.InvariantCulture)).FirstOrDefault();
    }

    private static FieldInfo GetField(object target, string fieldName)
    {
        return GetAllFields(target, f => f.Name.Equals(fieldName,
              StringComparison.InvariantCulture)).FirstOrDefault();
    }
    private static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo,
            bool> predicate)
    {
        List<Type> types = new List<Type>()
            {
                target.GetType()
            };

        while (types.Last().BaseType != null)
        {
            types.Add(types.Last().BaseType);
        }

        for (int i = types.Count - 1; i >= 0; i--)
        {
            IEnumerable<FieldInfo> fieldInfos = types[i]
                .GetFields(BindingFlags.Instance | BindingFlags.Static |
   BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(predicate);

            foreach (var fieldInfo in fieldInfos)
            {
                yield return fieldInfo;
            }
        }
    }
    private static IEnumerable<MethodInfo> GetAllMethods(object target,
  Func<MethodInfo, bool> predicate)
    {
        IEnumerable<MethodInfo> methodInfos = target.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static |
  BindingFlags.NonPublic | BindingFlags.Public)
            .Where(predicate);

        return methodInfos;
    }
    #endregion

    private bool MeetsConditions(SerializedProperty property)
    {
        var showIfAttribute = this.attribute as ShowIfAttribute;
        var target = property.serializedObject.targetObject;
        List<bool> conditionValues = new List<bool>();

        foreach (var condition in showIfAttribute.Conditions)
        {
            FieldInfo conditionField = GetField(target, condition);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
            {
                conditionValues.Add((bool)conditionField.GetValue(target));
            }

            MethodInfo conditionMethod = GetMethod(target, condition);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
            {
                conditionValues.Add((bool)conditionMethod.Invoke(target, null));
            }
        }

        if (conditionValues.Count > 0)
        {
            bool met;
            if (showIfAttribute.Operator == ConditionOperator.And)
            {
                met = true;
                foreach (var value in conditionValues)
                {
                    met = met && value;
                }
            }
            else
            {
                met = false;
                foreach (var value in conditionValues)
                {
                    met = met || value;
                }
            }
            return met;
        }
        else
        {
            Debug.LogError("Invalid boolean condition fields or methods used!");
            return true;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent
                 label)
    {
        // Calcluate the property height, if we don't meet the condition and the draw mode is DontDraw, then height will be 0.
        bool meetsCondition = MeetsConditions(property);
        var showIfAttribute = this.attribute as ShowIfAttribute;

        if (!meetsCondition && showIfAttribute.Action ==
                                       ActionOnConditionFail.DontDraw)
            return 0;
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent
           label)
    {
        bool meetsCondition = MeetsConditions(property);
        // Early out, if conditions met, draw and go.
        if (meetsCondition)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        var showIfAttribute = this.attribute as ShowIfAttribute;
        if (showIfAttribute.Action == ActionOnConditionFail.DontDraw)
        {
            return;
        }
        else if (showIfAttribute.Action == ActionOnConditionFail.JustDisable)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();
        }

    }
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ShowIfAttribute : PropertyAttribute
{
    public ActionOnConditionFail Action { get; private set; }
    public ConditionOperator Operator { get; private set; }
    public string[] Conditions { get; private set; }

    public ShowIfAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator, params string[] conditions)
    {
        Action = action;
        Operator = conditionOperator;
        Conditions = conditions;
    }
}

public enum ActionOnConditionFail
{
    // If condition(s) are false, don't draw the field at all.
    DontDraw,
    // If condition(s) are false, just set the field as disabled.
    JustDisable,
}

public enum ConditionOperator
{
    // A field is visible/enabled only if all conditions are true.
    And,
    // A field is visible/enabled if at least ONE condition is true.
    Or,
}