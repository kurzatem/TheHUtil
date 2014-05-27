namespace TheHUtilTests.Logging.Boilerplate
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TheHUtil.Logging.Boilerplate;

    [TestClass]
    public class ObjectExtTests
    {
        private Testee testee = new Testee();

        [TestMethod]
        public void shouldClone()
        {
            var actual = ObjectExt.Clone(this.testee);

            Assert.IsTrue(this.testee.Equals(actual));
        }

        [TestMethod]
        public void shouldBeEqual()
        {
            var clone = this.testee.Clone() as Testee;

            var actual = ObjectExt.Equals(this.testee, clone);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void shouldGetHashCode()
        {
            var expected = this.testee.GetHashCode();

            var actual = ObjectExt.GetHashCode(this.testee);

            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void shouldCreateNewInstance()
        {
            var other = ObjectExt.New<Testee>();

            Assert.IsFalse(object.ReferenceEquals(testee, other));
        }
    }
}
