using System;
using UnityEngine;

public class NestedEnumAttribute : PropertyAttribute
{

}

[AttributeUsage(AttributeTargets.Field)]
public class NestedEnumSubMenuAttribute : Attribute
{
    public string SubMenuPath;

    public NestedEnumSubMenuAttribute(string subMenu) => SubMenuPath = subMenu;
}