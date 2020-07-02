using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface 'forecing' to inherit from MonoBehaviour, to allow to use MonoBehaviour function for interfaces instances.
/// Contains all non-obsolete functions of MonoBehaviour class.
/// </summary>
public interface IMonoBehaviour
{
    // MonoBehaviour

#pragma warning disable IDE1006 // Naming Styles
    bool useGUILayout { get; set; }
    bool runInEditMode { get; set; }
    void CancelInvoke(string methodName);
    void CancelInvoke();
    void Invoke(string methodName, float time);
    void InvokeRepeating(string methodName, float time, float repeatRate);
    bool IsInvoking(string methodName);
    bool IsInvoking();
    Coroutine StartCoroutine(string methodName);
    Coroutine StartCoroutine(IEnumerator routine);
    Coroutine StartCoroutine(string methodName, object value);
    void StopAllCoroutines();
    void StopCoroutine(IEnumerator routine);
    void StopCoroutine(Coroutine routine);
    void StopCoroutine(string methodName);

    // Behaviour

    bool enabled { get; set; }
    bool isActiveAndEnabled { get; }

    // Component

    Transform transform { get; }
    GameObject gameObject { get; }
    string tag { get; set; }
    void BroadcastMessage(string methodName, SendMessageOptions options);
    void BroadcastMessage(string methodName);
    void BroadcastMessage(string methodName, object parameter);
    void BroadcastMessage(string methodName, object parameter, SendMessageOptions options);
    bool CompareTag(string tag);
    Component GetComponent(System.Type type);
    T GetComponent<T>();
    Component GetComponent(string type);
    Component GetComponentInChildren(System.Type t, bool includeInactive);
    Component GetComponentInChildren(System.Type t);
    T GetComponentInChildren<T>(bool includeInactive);
    T GetComponentInChildren<T>();
    Component GetComponentInParent(System.Type t);
    T GetComponentInParent<T>();
    Component[] GetComponents(System.Type type);
    void GetComponents(System.Type type, List<Component> results);
    void GetComponents<T>(List<T> results);
    T[] GetComponents<T>();
    Component[] GetComponentsInChildren(System.Type t);
    void GetComponentsInChildren<T>(List<T> results);
    T[] GetComponentsInChildren<T>();
    void GetComponentsInChildren<T>(bool includeInactive, List<T> result);
    T[] GetComponentsInChildren<T>(bool includeInactive);
    Component[] GetComponentsInChildren(System.Type t, bool includeInactive);
    T[] GetComponentsInParent<T>(bool includeInactive);
    Component[] GetComponentsInParent(System.Type t);
    Component[] GetComponentsInParent(System.Type t, bool includeInactive);
    T[] GetComponentsInParent<T>();
    void GetComponentsInParent<T>(bool includeInactive, List<T> results);
    void SendMessage(string methodName, object value, SendMessageOptions options);
    void SendMessage(string methodName);
    void SendMessage(string methodName, object value);
    void SendMessage(string methodName, SendMessageOptions options);
    void SendMessageUpwards(string methodName, object value);
    void SendMessageUpwards(string methodName, SendMessageOptions options);
    void SendMessageUpwards(string methodName);
    void SendMessageUpwards(string methodName, object value, SendMessageOptions options);
    bool TryGetComponent<T>(out T component);
    bool TryGetComponent(System.Type type, out Component component);

#pragma warning restore IDE1006 // Naming Styles
}
