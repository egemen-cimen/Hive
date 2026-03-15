namespace Hive.Core.Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var class1 = new Class1();
            Assert.IsNotNull(class1);
        }
    }
}
