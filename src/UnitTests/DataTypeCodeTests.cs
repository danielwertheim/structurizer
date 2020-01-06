using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizer;

namespace UnitTests
{
    [TestClass]
    public class DataTypeCodeTests : UnitTests
    {
        [TestMethod]
        public void IsNumeric_Should_return_true_When_number_type_only()
        {
            var allNumbers = new[]
            {
                DataTypeCode.FractalNumber,
                DataTypeCode.IntegerNumber,
                DataTypeCode.UnsignedIntegerNumber
            };

            var all = Enum.GetValues(typeof(DataTypeCode)).Cast<DataTypeCode>().ToList();

            all
                .Where(code => allNumbers.Contains(code))
                .ToList()
                .ForEach(code =>
                    code.IsNumeric().Should().BeTrue($"{code} is not numeric"));

            all
                .Where(code => !allNumbers.Contains(code))
                .ToList()
                .ForEach(code =>
                    code.IsNumeric().Should().BeFalse($"{code} is numeric"));
        }
    }
}