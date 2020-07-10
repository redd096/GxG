/*
 * from this
 * https://medium.com/@ProGM/show-a-draggable-point-into-the-scene-linked-to-a-vector3-field-using-the-handle-api-in-unity-bffc1a98271d
 * 
 * git
 * https://gist.github.com/ProGM/226204b2a7f99998d84d755ffa1fb39a
 * 
 * maybe better use property drawer
 * https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
 * 
 */

using System;
using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

//[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
/// <summary>
/// drag vector3 or array of vector3 - relative to the parent
/// </summary>
public class DraggableLocalPointAttribute : Attribute { }

#if UNITY_EDITOR

//pass true as a second parameter to CustomEditor
//This says to unity to instantiate this Editor script to all classes that inherit from MonoBehavior
[CustomEditor(typeof(MonoBehaviour), true)]
public class DraggableLocalPointDrawer : Editor
{

    readonly GUIStyle style = new GUIStyle();

    void OnEnable()
    {
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
    }

    public void OnSceneGUI()
    {
        //get first serialized property
        SerializedProperty property = serializedObject.GetIterator();

        //iterate on every property of the script - true to enter children too
        while (property.Next(true))
        {
            //if is a vector3
            if (property.propertyType == SerializedPropertyType.Vector3)
            {
                //handle vector3
                Vector3Handle(property);
            }
            //if is array
            else if (property.isArray)
            {
                //foreach point
                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(i);

                    //if not an array of vector3, break
                    if (element.propertyType != SerializedPropertyType.Vector3)
                    {
                        break;
                    }

                    //handle vector3 (element is this vector3, property is the parent, i is the index)
                    Vector3InArrayHandle(element, property, i);
                }
            }
        }
    }

    /// <summary>
    /// handle vector3 point
    /// </summary>
    void Vector3Handle(SerializedProperty property)
    {
        //use System.Reflection to check for any PropertyAttribute of type DraggablePoint defined in the current object property.

        //if field == null do nothing
        FieldInfo field = serializedObject.targetObject.GetType().GetField(property.name);
        if (field == null)
        {
            return;
        }

        //get draggable points
        object[] draggablePoints = field.GetCustomAttributes(typeof(DraggableLocalPointAttribute), false);
        if (draggablePoints.Length > 0)
        {
            //show the name (position relative to parent)
            Handles.Label(property.vector3Value + ((MonoBehaviour)target).transform.position, property.name);

            //make this vector3 a position handle (position relative to parent)
            property.vector3Value = Handles.PositionHandle(property.vector3Value + ((MonoBehaviour)target).transform.position, Quaternion.identity) - ((MonoBehaviour)target).transform.position;

            //use ApplyModifiedProperties to apply the movement done by the handle. 
            //it also creates a history entry, so you can use ctrl+z to rollback the movement
            serializedObject.ApplyModifiedProperties();
        }
    }

    /// <summary>
    /// handle vector3 point inside an array
    /// </summary>
    void Vector3InArrayHandle(SerializedProperty property, SerializedProperty parent, int index)
    {
        //check parent - if field == null do nothing
        FieldInfo parentfield = serializedObject.targetObject.GetType().GetField(parent.name);
        if (parentfield == null)
        {
            return;
        }

        //get draggable points
        object[] draggablePoints = parentfield.GetCustomAttributes(typeof(DraggableLocalPointAttribute), false);
        if (draggablePoints.Length > 0)
        {
            //show parent name + [index] (position relative to parent)
            Handles.Label(property.vector3Value + ((MonoBehaviour)target).transform.position, parent.name + "[" + index + "]");

            //make this vector3 a position handle (position relative to parent)
            property.vector3Value = Handles.PositionHandle(property.vector3Value + ((MonoBehaviour)target).transform.position, Quaternion.identity) - ((MonoBehaviour)target).transform.position;

            //use ApplyModifiedProperties to apply the movement done by the handle. 
            //it also creates a history entry, so you can use ctrl+z to rollback the movement
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif