using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;

namespace Microwave.Test.Unit
{
    [TestFixture]
    public class PowerTubeTest
    {
        private PowerTube uut;
        private IOutput output;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            uut = new PowerTube(output);
        }

        [Test]
        public void TurnOn_WasOff_CorrectOutput()
        {
            uut.TurnOn(50);
            // 50% ud af 700 = 7% (afrundet)
            output.Received().OutputLine(Arg.Is<string>(str => str.Contains("7 %")));
        }

        [Test]
        public void TurnOff_WasOn_CorrectOutput()
        {
            uut.TurnOn(50);
            uut.TurnOff();
            output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void TurnOff_WasOff_NoOutput()
        {
            uut.TurnOff();
            output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void TurnOn_WasOn_ThrowsException()
        {
            uut.TurnOn(50);
            Assert.Throws<System.ApplicationException>(() => uut.TurnOn(60));
        }

        [Test]
        public void TurnOn_NegativePower_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => uut.TurnOn(-1));
        }

        [Test]
        public void TurnOn_HighPower_ThrowsException()
        {
            // Over max power 700
            Assert.Throws<System.ArgumentOutOfRangeException>(() => uut.TurnOn(701));
        }

        [Test]
        public void TurnOn_ZeroPower_ThrowsException()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => uut.TurnOn(0));
        }

    }
}