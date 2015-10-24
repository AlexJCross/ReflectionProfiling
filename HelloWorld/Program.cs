namespace HelloWorld
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            const int Loop = 100000;

            IPopcornService popcorn = new PopcornService();
            ISweatyPalmsService palms = new SweatyPalmsService();
            IChineseTakeawayService takeaway = new ChineseTakeawayService();
            IUnderpantsService pants = new UnderpantsService();
            IAcneService acne = new AcneService();

            var container = new GamingServiceContainer();
            
            container.Register(popcorn)
                     .Register(palms)
                     .Register(takeaway)
                     .Register(pants)
                     .Register(acne);

            var gamerFactory = new GamerFactory(container);
            var gamerFactory2 = new GamerFactory2(popcorn, palms, takeaway, pants, acne);
            var gamerFactory3 = new GamerFactory3(popcorn, palms, takeaway, pants, acne);

            var callOfDutyGame = new CallOfDutyGame();
            var marioKartGame = new MarioKartGame();

            // Make sure they have both been JITed
            for (int i = 0; i < 100; i++)
            {
                TestActivator(gamerFactory, callOfDutyGame, marioKartGame);
                TestNoReflection(gamerFactory2, callOfDutyGame, marioKartGame);
                TestDynamic(gamerFactory3, callOfDutyGame, marioKartGame);
            }

            Stopwatch stopwatch = new Stopwatch();

            Thread.Sleep(100);
            stopwatch.Start();
            for (int i = 0; i < Loop; i++)
            {
                TestActivator(gamerFactory, callOfDutyGame, marioKartGame);
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            Thread.Sleep(100);
            stopwatch.Restart();
            for (int i = 0; i < Loop; i++)
            {
                TestNoReflection(gamerFactory2, callOfDutyGame, marioKartGame);
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            Thread.Sleep(100);
            stopwatch.Restart();
            for (int i = 0; i < Loop; i++)
            {
                TestDynamic(gamerFactory3, callOfDutyGame, marioKartGame);
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            
            
            Console.ReadKey();
        }

        public static void TestActivator(GamerFactory gamerFactory, IGame callOfDutyGame, IGame marioKartGame)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame();

            IGamer gamer2 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer2.PlayGame();
        }

        public static void TestNoReflection(GamerFactory2 gamerFactory, CallOfDutyGame callOfDutyGame, MarioKartGame marioKartGame)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame();

            IGamer gamer2 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer2.PlayGame();
        }

        public static void TestDynamic(GamerFactory3 gamerFactory, IGame callOfDutyGame, IGame marioKartGame)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame();

            IGamer gamer2 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer2.PlayGame();
        }

        public static bool isPrime(int number)
        {
            double boundary = Math.Floor(Math.Sqrt(number));

            if (number == 1) return false;
            if (number == 2) return true;

            for (int i = 2; i <= boundary; ++i)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
    }


    public interface IWriter
    {
        void Write(string message);
    }
    public class Writer : IWriter
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Exclaimer
    {
        private IWriter writer;
        public Exclaimer(IWriter writer)
        {
            this.writer = writer;
        }

        public void Exclaim()
        {
            this.writer.Write("Hello World");
        }
    }

}
