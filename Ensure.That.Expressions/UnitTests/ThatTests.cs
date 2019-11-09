namespace UnitTests
{
    using System;

    using Ensure.That.Expressions;

    using EnsureThat;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ThatTests
    {
        [TestMethod]
        public void LocalVariableName()
        {
            string variable = null;

            Action action = () =>
            {
                EnsureExpression
                    // WILL allocate a local func to capture your variable
                    .That(() => variable)
                    .IsNotNull();
            };
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Value can not be null. (Parameter 'variable')");
        }

        [TestMethod]
        public void PropertyName()
        {
            Action action = () =>
            {
                EnsureExpression
                    // WILL allocate a local func to capture your variable
                    .That(() => Console.Out.NewLine)
                    .Is(null);
            };
            action.Should().Throw<ArgumentException>()
                // TODO: We probably want it to get the whole Console.Out.NewLine
                .WithMessage("*(Parameter 'NewLine')");
        }

        [TestMethod]
        public void SomethingStrange()
        {
            Action action = () =>
            {
                EnsureExpression
                    // WILL allocate a local func to capture your variable
                    .That(() => 1 + 3)
                    .Is(0);
            };

            action.Should().Throw<NotSupportedException>();
        }
    }
}