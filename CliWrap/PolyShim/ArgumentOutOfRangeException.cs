#pragma warning disable CS0436

using System;
using System.Runtime.CompilerServices;

internal static class MemberPolyfills_Net80_ArgumentOutOfRangeException
{
    extension(ArgumentOutOfRangeException)
    {
        // https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifnegative
        // Note: the original API constrains T to INumber<T> (numeric types only). Because
        // INumber<T> is not available on older TFMs, we use 'struct, IComparable<T>' instead.
        // This accepts any value type (including non-numeric structs such as enums) but still
        // prevents null and correctly compares against default(T) which is zero for all
        // built-in numeric types.
        public static void ThrowIfNegative<T>(
            T value,
            [CallerArgumentExpression(nameof(value))] string? paramName = null
        )
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default) < 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName,
                    value,
                    $"'{paramName}' must be a non-negative value."
                );
            }
        }

    }
}