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
            // Ændret da start tager sekunder frem for millisekunder
            // 10 seconds
            uut.Start(60/6);

            // wait for a tick, but no longer
            // Lidt over 10 sekunder
            Assert.That(pause.WaitOne(10100));

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
            // Ændret da start tager sekunder frem for millisekunder
            // 10 seconds
            uut.Start(60/6);

            // wait for expiration, but not much longer, should come
            // 11 second
            Assert.That(pause.WaitOne(11000));

            uut.Stop();
        }

        [Test]
        public void Start_TimerExpires_LongEnough()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            uut.Expired += (sender, args) => pause.Set();
            // Ændret da start tager sekunder frem for millisekunder
            // 10 seconds
            uut.Start(60/6);

            // wait shorter than expiration, shouldn't come
            // 8 sekunder
            Assert.That(!pause.WaitOne(8000));

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