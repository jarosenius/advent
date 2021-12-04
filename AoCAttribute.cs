using System;

[AttributeUsage(AttributeTargets.All)]
public class AoCAttribute : System.Attribute 
{
    public readonly int Year;

    public AoCAttribute(int year)
    {
        this.Year = year;
    }
}
