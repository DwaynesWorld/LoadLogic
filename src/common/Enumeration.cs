using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LoadLogic.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Implementation based on
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
    /// </remarks>
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Enumeration otherValue))
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            return Math.Abs(firstValue.Id - secondValue.Id);
        }

        public static T FromValue<T>(int value) where T : Enumeration
        {
            return Parse<T, int>(value, "value", item => item.Id == value);
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            return Parse<T, string>(displayName, "display name", item => item.Name == displayName);
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
            }

            return matchingItem;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}
