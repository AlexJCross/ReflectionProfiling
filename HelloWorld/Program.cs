namespace HelloWorld
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            const int Loop = 500000;

            IPopcornService popcorn = new PopcornService();
            ISweatyPalmsService palms = new SweatyPalmsService();
            IChineseTakeawayService takeaway = new ChineseTakeawayService();
            IUnderpantsService pants = new UnderpantsService();
            IGamingHeadsetService headset = new GamingHeadsetService();

            var container = new GamingServiceContainer()
                .Register(popcorn)
                .Register(palms)
                .Register(takeaway)
                .Register(pants)
                .Register(headset);

            var gamerFactoryR = new GamerFactoryReflection(container);
            var gamerFactoryN = new GamerFactoryNormal(popcorn, palms, takeaway, pants, headset);
            var gamerFactoryD = new GamerFactoryDynamic(popcorn, palms, takeaway, pants, headset);
            var gamerFactoryT = new GamerFactoryTypeSwitch(popcorn, palms, takeaway, pants, headset);

            var callOfDutyGame = new CallOfDutyGame();
            var marioKartGame = new MarioKartGame();

            // Make sure they have all been JITed
            for (int i = 0; i < 100; i++)
            {
                TestActivator(gamerFactoryR, callOfDutyGame, marioKartGame, 100);
                TestNormal(gamerFactoryN, callOfDutyGame, marioKartGame, 100);
                TestDynamic(gamerFactoryD, callOfDutyGame, marioKartGame, 100);
                TestTypeSwitch(gamerFactoryT, callOfDutyGame, marioKartGame, 100);
            }

            Console.WriteLine(string.Join("\t",
                    new[]
                {
                    "Cycles",
                    "Normal",
                    "Type",
                    "Dynamic",
                    "Reflec",
                    "Normal",
                    "Type",
                    "Dynamic",
                    "Reflec",
                }));

            for (int cycles = 1; cycles < 5000; cycles = cycles * 2)
            {
                Stopwatch stopwatch = new Stopwatch();

                // Using the new operator
                Thread.Sleep(50);
                stopwatch.Restart();
                for (int i = 0; i < Loop; i++)
                {
                    TestNormal(gamerFactoryN, callOfDutyGame, marioKartGame, cycles);
                }
                stopwatch.Stop();
                var traditionalTime = stopwatch.Elapsed;

                // Using the dynamic keyword
                Thread.Sleep(50);
                stopwatch.Restart();
                for (int i = 0; i < Loop; i++)
                {
                    TestDynamic(gamerFactoryD, callOfDutyGame, marioKartGame, cycles);
                }
                stopwatch.Stop();
                var dynamicTime = stopwatch.Elapsed;

                // Using a dictionary cached on type
                Thread.Sleep(50);
                stopwatch.Restart();
                for (int i = 0; i < Loop; i++)
                {
                    TestTypeSwitch(gamerFactoryT, callOfDutyGame, marioKartGame, cycles);
                }
                stopwatch.Stop();
                var typeSwitchTime = stopwatch.Elapsed;

                // Using reflection
                Thread.Sleep(50);
                stopwatch.Start();
                for (int i = 0; i < Loop; i++)
                {
                    TestActivator(gamerFactoryR, callOfDutyGame, marioKartGame, cycles);
                }
                stopwatch.Stop();
                var activatorTime = stopwatch.Elapsed;

                Console.WriteLine(string.Join("\t",
                    new[]
                {
                    cycles.ToString(),
                    traditionalTime.ToString("ss':'fff"),
                    typeSwitchTime.ToString("ss':'fff"),
                    dynamicTime.ToString("ss':'fff"),
                    activatorTime.ToString("ss':'fff"),
                    (traditionalTime.TotalMilliseconds / traditionalTime.TotalMilliseconds).ToString("#.##"),
                    (typeSwitchTime.TotalMilliseconds / traditionalTime.TotalMilliseconds).ToString("#.##"),
                    (dynamicTime.TotalMilliseconds / traditionalTime.TotalMilliseconds).ToString("#.##"),
                    (activatorTime.TotalMilliseconds / traditionalTime.TotalMilliseconds).ToString("#.##")
                }));
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static void TestActivator(
            GamerFactoryReflection gamerFactory, IGame callOfDutyGame, IGame marioKartGame, int cycles)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame(cycles);

            IGamer gamer2 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer2.PlayGame(cycles);
        }

        public static void TestNormal(
            GamerFactoryNormal gamerFactory, CallOfDutyGame callOfDutyGame, MarioKartGame marioKartGame, int cycles)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame(cycles);

            gamer1 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer1.PlayGame(cycles);
        }

        public static void TestDynamic(
            GamerFactoryDynamic gamerFactory, IGame callOfDutyGame, IGame marioKartGame, int cycles)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame(cycles);

            gamer1 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer1.PlayGame(cycles);
        }

        public static void TestTypeSwitch(
            GamerFactoryTypeSwitch gamerFactory, IGame callOfDutyGame, IGame marioKartGame, int cycles)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame(cycles);

            gamer1 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer1.PlayGame(cycles);
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
