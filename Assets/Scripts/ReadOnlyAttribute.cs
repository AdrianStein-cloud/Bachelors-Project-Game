using System;
using UnityEngine;

/// <summary>
/// Read Only attribute.
/// Attribute is use only to mark ReadOnly properties.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class ReadOnlyAttribute : PropertyAttribute { }