using System;
using System.Collections.Generic;
using Mapster;
using NFluent;
using NFluent.Extensibility;

namespace Moonlight.Tests.Extensions
{
    public static class NFluentExtension
    {
        public static void Is<T>(this ICheck<T> check, T value)
        {
            check.IsEqualTo(value);
        }

        public static void IsNotNullOrEmpty(this ICheck<string> check)
        {
            check.Not.IsNullOrEmpty();
        }

        public static void WhichIs<T>(this ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> check, T value)
        {
            check.Which.Is(value);
        }

        public static void Match<T>(this ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> check, Predicate<T> value)
        {
            ExtensibilityHelper.BeginCheck(check.Which)            
                .OnNegate("The {0} is false, whereas it must not.")
                .FailWhen(x => !value.Invoke(x), "The {0} is not true")
                .EndCheck();
        }

        public static void WithElementAt<T>(this ICheck<T[]> check, int index, T value)
        {
            check.HasElementAt(index).Which.Is(value);
        }
    }
}