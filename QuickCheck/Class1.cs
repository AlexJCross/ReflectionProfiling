namespace QuickCheck
{
    using HelloWorld;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class Class1
    {
        [Test]
        public void check()
        {
            var writer = new Mock<IWriter>();
            var exclaimer = new Exclaimer(writer.Object);

            exclaimer.Exclaim();

            writer.Verify(w => w.Write("Hello World"));
        }
    }
}
