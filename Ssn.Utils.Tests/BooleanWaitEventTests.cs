using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Ssn.Utils.Misc;
namespace Ssn.Utils.Tests {
    public class BooleanWaitEventTests
    {
        [Test]
        public void Works() {
            var sut = new BooleanWaitEvent();
            Assert.IsTrue(sut.IsFalse);
            sut.SetTrue();
            Assert.IsTrue(sut.IsTrue);

            int count = 0;
            var task = Task.Run(() => {
                sut.WaitUntilFalse();
                count++;
                Thread.Sleep(500);
                count++;
                sut.SetTrue();
            }
                );
            Assert.AreEqual(0, count);
            sut.SetFalse();
            Assert.IsFalse(sut.WaitUntilTrue(100));
            Assert.AreEqual(1, count);
            Assert.IsFalse(sut.WaitUntilTrue(100));
            Assert.AreEqual(1, count);
            Assert.IsTrue(sut.WaitUntilTrue(400));
            Assert.IsFalse(sut.WaitUntilFalse(100));
            Assert.IsTrue(sut.WaitUntilTrue());
            Assert.AreEqual(2,count);
            task.Wait();

        }

    }
}