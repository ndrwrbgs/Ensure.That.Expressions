namespace Ensure.That.Expressions
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    using EnsureThat;

    using JetBrains.Annotations;

    [PublicAPI]
    public static class EnsureExpression
    {
        [Pure]
        private static Param<TContainer> That<TContainer, TProperty>(
            [NotNull] Expression<Func<TContainer, TProperty>> expression,
            TContainer instance,
            [CanBeNull] OptsFn optsFn = null,
            // Used for smart caching
            [CallerFilePath] [NotNull] string filePath = null,
            [CallerMemberName] [NotNull] string memberName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Note that whatever you're passing into <paramref name="expression"/> will
        /// likely result in an object allocation for a <see cref="Func{T, TResult}"/>
        /// in order to capture the closure you're gathering
        /// </summary>
        [Pure]
        public static Param<T> That<T>(
            [NotNull] Expression<Func<T>> expression,
            [CanBeNull] OptsFn optsFn = null,
            // Used for smart caching
            [CallerFilePath] [NotNull] string filePath = null,
            [CallerMemberName] [NotNull] string memberName = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            // TODO: Should cache the compiled expression
            T value = GetValue(expression, filePath, memberName, lineNumber);
            // TODO: Should cache the name
            string name = GetName(expression, filePath, memberName, lineNumber);

            return new Param<T>(name, value, optsFn);
        }

        [NotNull]
        private static string GetName<T>(
            [NotNull] Expression<Func<T>> expression,
            string filePath,
            string memberName,
            int lineNumber)
        {
            var body = expression.Body;
            var memberExpression = body as MemberExpression;
            if (memberExpression == null)
                // TODO: Factor out exceptions for speed/inlining
            {
                throw new NotSupportedException("Whatever you gave us, we don't understand it");
            }

            return memberExpression.Member.Name;
        }

        private static T GetValue<T>(
            [NotNull] Expression<Func<T>> expression,
            string filePath,
            string memberName,
            int lineNumber)
        {
            return expression.Compile()();
        }
    }
}