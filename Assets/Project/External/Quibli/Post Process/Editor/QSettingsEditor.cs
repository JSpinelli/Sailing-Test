using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.PostProcessing;
using UnityEditor;
using UnityEditorInternal;

namespace UnityEditor.Rendering.Universal.PostProcessing {
/// <summary>
/// A custom property drawer for the settings for custom post-processing feature
/// </summary>
[CustomPropertyDrawer(typeof(QuibliPostProcess.Settings), true)]
internal class QSettingsEditor : PropertyDrawer {
    /// <summary>
    /// This will contain a list of all available renderers for each injection point.
    /// </summary>
    private Dictionary<InjectionPoint, List<Type>> _availableRenderers;

    /// <summary>
    /// Contains 3 Reorderable list for each settings property.
    /// </summary>
    private struct DrawerState {
        public ReorderableList listAfterOpaqueAndSky;
        public ReorderableList listBeforePostProcess;
        public ReorderableList listAfterPostProcess;
    }

    /// <summary>
    /// Since the drawer is shared for multiple properties, we need to store the reorderable lists for each property by path.
    /// </summary>
    private Dictionary<string, DrawerState> propertyStates = new Dictionary<string, DrawerState>();

    /// <summary>
    /// Get the renderer name from the attached custom post-process attribute.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private string GetName(Type type) {
        return CompoundRendererFeatureAttribute.GetAttribute(type)?.Name ?? type?.Name;
    }

    // This code is mostly copied from Unity's HDRP repository
    /// <summary>
    /// Intialize a reoderable list
    /// </summary>
    void InitList(ref ReorderableList reorderableList, List<string> elements, string headerName,
                  InjectionPoint injectionPoint, QuibliPostProcess feature) {
        reorderableList = new ReorderableList(elements, typeof(string), true, true, true, true);

        reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, headerName, EditorStyles.boldLabel);

        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
            rect.height = EditorGUIUtility.singleLineHeight;
            var elemType = Type.GetType(elements[index]);
            EditorGUI.LabelField(rect, GetName(elemType), EditorStyles.boldLabel);
        };

        reorderableList.onAddCallback = (list) => {
            var menu = new GenericMenu();

            foreach (var type in _availableRenderers[injectionPoint]) {
                if (!elements.Contains(type.AssemblyQualifiedName))
                    menu.AddItem(new GUIContent(GetName(type)), false, () => {
                        Undo.RegisterCompleteObjectUndo(feature, $"Added {type.ToString()} Custom Post Process");
                        elements.Add(type.AssemblyQualifiedName);
                        forceRecreate(feature); // This is done since OnValidate doesn't get called.
                    });
            }

            if (menu.GetItemCount() == 0) menu.AddDisabledItem(new GUIContent("No Custom Post Process Availble"));

            menu.ShowAsContext();
            EditorUtility.SetDirty(feature);
        };
        reorderableList.onRemoveCallback = (list) => {
            Undo.RegisterCompleteObjectUndo(feature, $"Removed {list.list[list.index].ToString()} Custom Post Process");
            elements.RemoveAt(list.index);
            EditorUtility.SetDirty(feature);
            forceRecreate(feature); // This is done since OnValidate doesn't get called.
        };
        reorderableList.elementHeightCallback =
            _ => EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        reorderableList.onReorderCallback = (list) => {
            EditorUtility.SetDirty(feature);
            forceRecreate(feature); // This is done since OnValidate doesn't get called.
        };
    }

    /// <summary>
    /// Initialize a drawer state for the giver property if none already exists.
    /// </summary>
    /// <param name="property">The property that will be edited/displayed</param>
    private void Init(SerializedProperty property) {
        var path = property.propertyPath;
        if (!propertyStates.ContainsKey(path)) {
            var state = new DrawerState();
            var feature = property.serializedObject.targetObject as QuibliPostProcess;
            InitList(ref state.listAfterOpaqueAndSky, feature.settings.renderersAfterOpaqueAndSky,
                     "After Opaque and Sky", InjectionPoint.AfterOpaqueAndSky, feature);
            InitList(ref state.listBeforePostProcess, feature.settings.renderersBeforePostProcess,
                     "Before Post Process", InjectionPoint.BeforePostProcess, feature);
            // "After Post Process"
            InitList(ref state.listAfterPostProcess, feature.settings.renderersAfterPostProcess, "Effects",
                     InjectionPoint.AfterPostProcess, feature);
            propertyStates.Add(path, state);
        }
    }

    /// <summary>
    /// Present the property on the Editor GUI
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        populateRenderers();
        EditorGUI.BeginProperty(position, label, property);
        Init(property);
        DrawerState state = propertyStates[property.propertyPath];
        EditorGUI.BeginChangeCheck();
#if Q_AFTER_OPAQUE_AND_SKY
        state.listAfterOpaqueAndSky.DoLayoutList();
        EditorGUILayout.Space();
#endif
#if Q_BEFORE_POST_PROCESS
        state.listBeforePostProcess.DoLayoutList();
        EditorGUILayout.Space();
#endif
        state.listAfterPostProcess.DoLayoutList();
        if (EditorGUI.EndChangeCheck()) {
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
        EditorUtility.SetDirty(property.serializedObject.targetObject);
    }

    /// <summary>
    /// Force recreating the render feature
    /// </summary>
    /// <param name="feature">The render feature to recreate</param>
    private void forceRecreate(QuibliPostProcess feature) {
        feature.Create();
    }

    /// <summary>
    /// Finds all the custom post-processing renderer classes and categorizes them by injection point
    /// </summary>
    private void populateRenderers() {
        if (_availableRenderers != null) return;
        _availableRenderers = new Dictionary<InjectionPoint, List<Type>>() {
            { InjectionPoint.AfterOpaqueAndSky, new List<Type>() },
            { InjectionPoint.BeforePostProcess, new List<Type>() },
            { InjectionPoint.AfterPostProcess, new List<Type>() }
        };
        foreach (var type in TypeCache.GetTypesDerivedFrom<CompoundRenderer>()) {
            if (type.IsAbstract) continue;
            var attributes = type.GetCustomAttributes(typeof(CompoundRendererFeatureAttribute), false);
            if (attributes.Length != 1) continue;
            CompoundRendererFeatureAttribute attribute = attributes[0] as CompoundRendererFeatureAttribute;
            if (attribute.InjectionPoint.HasFlag(InjectionPoint.AfterOpaqueAndSky))
                _availableRenderers[InjectionPoint.AfterOpaqueAndSky].Add(type);
            if (attribute.InjectionPoint.HasFlag(InjectionPoint.BeforePostProcess))
                _availableRenderers[InjectionPoint.BeforePostProcess].Add(type);
            if (attribute.InjectionPoint.HasFlag(InjectionPoint.AfterPostProcess))
                _availableRenderers[InjectionPoint.AfterPostProcess].Add(type);
        }
    }
}
}