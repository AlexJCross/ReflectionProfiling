using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorld
{
    public interface IGamer
    {
        GameReport PlayGame(int level);
    }

    public abstract class Gamer : IGamer
    {
        public void Guard<T>(T blah)
        {
            if (blah == null)
            {
                throw new NullReferenceException();
            }
        }
    
        protected static bool Spin(int number)
        {
            int j = 0;

            for (int i = 1; i < number; ++i)
            {
                j = ((number * i) % i == 0) ? j++ : j;
            }
    
            return j > 12;
        }
    
        public abstract GameReport PlayGame(int level);
    }

    public class MkGamer : Gamer
    {
        public MkGamer(
            IPopcornService s1,
            ISweatyPalmsService s2,
            IUnderpantsService s3,
            IGamingHeadsetService s4,
            IChineseTakeawayService s5)
        {
            this.Guard(s1);
            this.Guard(s2);
            this.Guard(s3);
            this.Guard(s4);
            this.Guard(s5);
        }

        public override GameReport PlayGame(int level)
        {
            Spin(level);
            return new GameReport();
        }
    }

    public class CodGamer : Gamer
    {
        public CodGamer(
            IPopcornService s1,
            ISweatyPalmsService s2,
            IUnderpantsService s3,
            IGamingHeadsetService s4,
            IChineseTakeawayService s5)
        {
            this.Guard(s1);
            this.Guard(s2);
            this.Guard(s3);
            this.Guard(s4);
            this.Guard(s5);
        }



        public override GameReport PlayGame(int level)
        {
            Spin(level);
            return new GameReport();
        }
    }
}
