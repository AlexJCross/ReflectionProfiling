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
            IGamingHeadsetService acne = new GamingHeadsetService();

            var container = new GamingServiceContainer()
                .Register(popcorn)
                .Register(palms)
                .Register(takeaway)
                .Register(pants)
                .Register(acne);

            var gamerFactory1 = new GamerFactory(container);
            var gamerFactory2 = new GamerFactory2(popcorn, palms, takeaway, pants, acne);
            var gamerFactory3 = new GamerFactory3(popcorn, palms, takeaway, pants, acne);

            var callOfDutyGame = new CallOfDutyGame();
            var marioKartGame = new MarioKartGame();

            // Make sure they have all been JITed
            for (int i = 0; i < 100; i++)
            {
                TestActivator(gamerFactory1, callOfDutyGame, marioKartGame, 100);
                TestNoReflection(gamerFactory2, callOfDutyGame, marioKartGame, 100);
                TestDynamic(gamerFactory3, callOfDutyGame, marioKartGame, 100);
            }

            Console.WriteLine(string.Join("\t",
                    new[]
                {
                    "Cycles",
                    "Reflec",
                    "Normal",
                    "Dynamic",
                    "Reflec",
                    "Normal",
                    "Dynamic"
                }));

            for (int cycles = 1; cycles < 5000; cycles = cycles * 2)
            {
                Stopwatch stopwatch = new Stopwatch();

                Thread.Sleep(50);
                stopwatch.Start();
                for (int i = 0; i < Loop; i++)
                {
                    TestActivator(gamerFactory1, callOfDutyGame, marioKartGame, cycles);
                }
                stopwatch.Stop();
                var activatorTime = stopwatch.Elapsed;

                Thread.Sleep(50);
                stopwatch.Restart();
                for (int i = 0; i < Loop; i++)
                {
                    TestNoReflection(gamerFactory2, callOfDutyGame, marioKartGame, cycles);
                }
                stopwatch.Stop();
                var traditionalTime = stopwatch.Elapsed;

                Thread.Sleep(50);
                stopwatch.Restart();
                for (int i = 0; i < Loop; i++)
                {
                    TestDynamic(gamerFactory3, callOfDutyGame, marioKartGame, cycles);
                }
                stopwatch.Stop();
                var dynamicTime = stopwatch.Elapsed;

                


                Console.WriteLine(string.Join("\t",
                    new[]
                {
                    cycles.ToString(),
                    activatorTime.ToString("ss':'fff"),
                    traditionalTime.ToString("ss':'fff"),
                    dynamicTime.ToString("ss':'fff"),
                    (activatorTime.TotalMilliseconds / traditionalTime.TotalMilliseconds).ToString("#.##"),
                    (traditionalTime.TotalMilliseconds / traditionalTime.TotalMilliseconds).ToString("#.##"),
                    (dynamicTime.TotalMilliseconds / traditionalTime.TotalMilliseconds).ToString("#.##")
                }));
                
            }

            

            Console.ReadKey();
        }

        public static void TestActivator(
            GamerFactory gamerFactory, IGame callOfDutyGame, IGame marioKartGame, int cycles)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame(cycles);

            IGamer gamer2 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer2.PlayGame(cycles);
        }

        public static void TestNoReflection(
            GamerFactory2 gamerFactory, CallOfDutyGame callOfDutyGame, MarioKartGame marioKartGame, int cycles)
        {
            IGamer gamer1 = gamerFactory.CreateGamer(callOfDutyGame);
            var gameReport1 = gamer1.PlayGame(cycles);

            gamer1 = gamerFactory.CreateGamer(marioKartGame);
            var gameReport2 = gamer1.PlayGame(cycles);
        }

        public static void TestDynamic(
            GamerFactory3 gamerFactory, IGame callOfDutyGame, IGame marioKartGame, int cycles)
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
