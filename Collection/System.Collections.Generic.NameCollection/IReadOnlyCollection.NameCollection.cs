using Cactus.Blade.Collection;
using System;
using System.Collections.Generic;

public static partial class Extension
{
    /// <summary>
    /// Creates a <see cref="NameCollection{T}"/> from an <see cref="IEnumerable{T}"/>
    /// according to the specified <paramref name="name"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="values"/>.</typeparam>
    /// <param name="values">The values that make up the collection.</param>
    /// <param name="name">A function that gets the name of a value.</param>
    /// <param name="stringComparer">
    /// The equality comparer used to determine if names are equal. The default value is
    /// <see cref="StringComparer.OrdinalIgnoreCase"/>.
    /// </param>
    /// <param name="defaultName">
    /// The name that indicates an item should be used as the <see cref="NameCollection{T}.DefaultValue"/>
    /// for the <see cref="NameCollection{T}"/>.
    /// </param>
    /// <param name="strict">
    /// Whether to throw an exception if <paramref name="values"/> contains more than one
    /// default value or more than one value with the same name. If <see langword="false"/>,
    /// when there are duplicates, then the last value in the collection wins.
    /// </param>
    /// <returns>
    /// A <see cref="NameCollection{T}"/> that contains values of type <typeparamref name="T"/>
    /// selected from the input sequence.
    /// </returns>
    public static NameCollection<T> ToNameCollection<T>(this IEnumerable<T> values, Func<T, string> name,
        IEqualityComparer<string> stringComparer = null, string defaultName = "default", bool strict = true)
    {
        return new NameCollection<T>(values, name, stringComparer, defaultName, strict);
    }
}
