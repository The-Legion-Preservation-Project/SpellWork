using System;

namespace SpellWork.Extensions;

[AttributeUsage(AttributeTargets.Field)]
public class FullNameAttribute : Attribute
{
    public string FullName { get; }

    public FullNameAttribute(string fullName)
    {
        FullName = fullName;
    }
}
