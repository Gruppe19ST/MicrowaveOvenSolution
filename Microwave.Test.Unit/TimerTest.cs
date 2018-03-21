using System.Threading;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Unit
{
    [TestFixture]
    public class TimerTest
    {
        private Timer uut;

        [SetUp]
        public void Setup()
        {
            uut = new Timer();
        }

        [Test]
        public void Start_TimerTick_ShortEnough()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            uut.TimerTick += (sender, args) => pause.Set();
            uut.Start(2000);

            // wait for a tick, but no longer
            Assert.That(pause.WaitOne(1100));

            uut.Stop();
        }

        [Test]
        public void Start_TimerTick_LongEnough()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            uut.TimerTick += (sender, args) => pause.Set();
            // Vi arbejder i minutter
            uut.Start(2*60);

            // wait shorter than a tick, shouldn't come
            Assert.That(!pause.WaitOne(900));

            uut.Stop();
        }

        [Test]
        public void Start_TimerExpires_ShortEnough()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            uut.Expired += (sender, args) => pause.Set();
            // Ændret, da vi arbejder i sekunder i stedet for millisekunder
            int seconds = 60;
            int minutes = 1 * seconds;
            int wait = (seconds * 1000) + 1000;
            uut.Start(minutes);

            // wait for expiration, but not much longer, should come
            Assert.That(pause.WaitOne(wait));

            uut.Stop();
        }

        [Test]
        public void Start_TimerExpires_LongEnough()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            uut.Expired += (sender, args) => pause.Set();
            uut.Start(2000);

            // wait shorter than expiration, shouldn't come
            Assert.That(!pause.WaitOne(1900));

            uut.Stop();
        }

        [Test]
        public void Start_TimerTick_CorrectNumber()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            int notifications = 0;

            uut.Expired += (sender, args) => pause.Set();
            uut.TimerTick += (sender, args) => notifications++;

            // 10 sekunder
            uut.Start(60/6);

            // wait longer than expiration (11 sekunder)
            Assert.That(pause.WaitOne(11000));
            uut.Stop();

            // 10 ticks expected, one for each second
            Assert.That(notifications, Is.EqualTo(10));
        }

    }
}