using ExpressionTreeExample.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTreeExample
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void Test_Map()
        {
            var initial = new Foo
            {
                TestProperty = "test property",
                TestProperty2 = 33,
                TestProperty3 = "test 3",
                TestField = "test field",
                TestField2 = "test 2"
            };

            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var result = mapper.Map(initial);

            Assert.AreEqual(initial.TestProperty, result.TestProperty);
            Assert.AreEqual(default(string), result.TestProperty2);
            Assert.AreEqual(initial.TestField, result.TestField);
            Assert.AreEqual(default(int), result.TestField2);
            Assert.AreEqual(default(string), result.TestField3);
        }
    }

    public class Foo
    {
        public string TestProperty { get; set; }

        public int TestProperty2 { get; set; }

        public string TestProperty3 { get; set; }

        public string TestField;

        public string TestField2;
    }

    public class Bar
    {
        public string TestProperty { get; set; }

        public string TestProperty2 { get; set; }

        public string TestField;

        public int TestField2;

        public string TestField3;
    }
}
