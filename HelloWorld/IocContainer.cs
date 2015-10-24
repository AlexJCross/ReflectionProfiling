namespace HelloWorld
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class GamingServiceContainer
    {
        private readonly Dictionary<Type, Object> gamingServiceByInterfaceType;
    
        public GamingServiceContainer()
        {
            this.gamingServiceByInterfaceType = new Dictionary<Type, Object>();
        }

        public GamingServiceContainer Register<TInterface>(TInterface instance)
        {
            this.gamingServiceByInterfaceType.Add(typeof(TInterface), instance);

            return this;
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }
    
        public object Get(Type parameterType)
        {
            return this.gamingServiceByInterfaceType[parameterType];
        }
    }

    public class GamerFactory
    {
        private readonly Dictionary<Type, Type> gamerByGame;
        private readonly GamingServiceContainer container;

        public GamerFactory(GamingServiceContainer container)
        {
            this.gamerByGame = this.LoadFromGamerConfigXml();
            this.container = container;
        }

        public IGamer CreateGamer(IGame game)
        {
            var targetGamerType = this.gamerByGame[game.GetType()];

            return (IGamer)this.GetObject(targetGamerType);
        }

        private object GetObject(Type parameterType)
        {
            var constructorParams = GetConstructorParams(parameterType);

            return Activator.CreateInstance(parameterType, constructorParams.ToArray());
        }

        private IEnumerable<object> GetConstructorParams(Type concreteType)
        {
            var constructor = concreteType.GetConstructors().Single();

            return constructor.GetParameters().Select(info => this.container.Get(info.ParameterType));
        }

        private Dictionary<Type, Type> LoadFromGamerConfigXml()
        {
            return new Dictionary<Type, Type>
            {
                { typeof(CallOfDutyGame) , typeof(NerdyGamer)},
                { typeof(MarioKartGame) ,typeof(SpottyGamer) }
            };
        }
    }

    public interface IGamerFactory
    {

    }

    public class GamerFactory2
    {
        private readonly Dictionary<Type, Type> gamerByGame;
        private readonly GamingServiceContainer container;

        IPopcornService popcorn;
        ISweatyPalmsService palms;
        IChineseTakeawayService takeaway;
        IUnderpantsService pants;
        IAcneService acne;

        public GamerFactory2(
            IPopcornService popcorn,
            ISweatyPalmsService palms,
            IChineseTakeawayService takeaway,
            IUnderpantsService pants,
            IAcneService acne)
        {
            this.popcorn = popcorn;
            this.palms = palms;
            this.takeaway = takeaway;
            this.pants = pants;
            this.acne = acne;
        }

        public IGamer CreateGamer(MarioKartGame game)
        {
            return new SpottyGamer(
                this.popcorn, this.palms, this.pants, this.acne, this.takeaway);
        }

        public IGamer CreateGamer(CallOfDutyGame game)
        {
            return new NerdyGamer(
                this.popcorn, this.palms, this.pants, this.acne, this.takeaway);
        }
    }

    public class GamerFactory3
    {
        private readonly Dictionary<Type, Type> gamerByGame;
        private readonly GamingServiceContainer container;

        IPopcornService popcorn;
        ISweatyPalmsService palms;
        IChineseTakeawayService takeaway;
        IUnderpantsService pants;
        IAcneService acne;

        public GamerFactory3(
            IPopcornService popcorn,
            ISweatyPalmsService palms,
            IChineseTakeawayService takeaway,
            IUnderpantsService pants,
            IAcneService acne)
        {
            this.popcorn = popcorn;
            this.palms = palms;
            this.takeaway = takeaway;
            this.pants = pants;
            this.acne = acne;
        }

        public IGamer CreateGamer(IGame game)
        {
            return this.Dispatch(game);
        }

        private IGamer Dispatch(dynamic game)
        {
            return this.InternalCreateGamer(game);
        }

        public IGamer InternalCreateGamer(MarioKartGame game)
        {
            return new SpottyGamer(
                this.popcorn, this.palms, this.pants, this.acne, this.takeaway);
        }

        public IGamer InternalCreateGamer(CallOfDutyGame game)
        {
            return new NerdyGamer(
                this.popcorn, this.palms, this.pants, this.acne, this.takeaway);
        }
    }

    public interface IGame
    {
    }

    public class CallOfDutyGame : IGame
    {
    }

    public class MarioKartGame : IGame
    {
    }

    public interface IGamer
    {
        GameReport PlayGame();
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

        public abstract GameReport PlayGame();
    }

    public class SpottyGamer : Gamer
    {
        public SpottyGamer(
            IPopcornService s1,
            ISweatyPalmsService s2,
            IUnderpantsService s3,
            IAcneService s4,
            IChineseTakeawayService s5)
        {
            this.Guard(s1);
            this.Guard(s2);
            this.Guard(s3);
            this.Guard(s4);
            this.Guard(s5);
        }

        public override GameReport PlayGame()
        {
            isPrime(321);
            return new GameReport();
        }
    }

    public class NerdyGamer : Gamer
    {
        public NerdyGamer(
            IPopcornService s1,
            ISweatyPalmsService s2,
            IUnderpantsService s3,
            IAcneService s4,
            IChineseTakeawayService s5)
        {
            this.Guard(s1);
            this.Guard(s2);
            this.Guard(s3);
            this.Guard(s4);
            this.Guard(s5);
        }

        

        public override GameReport PlayGame()
        {
            isPrime(321);
            return new GameReport();
        }
    }

    public class GameReport
    {
    }

    public interface IPopcornService
    {
    }

    public interface ISweatyPalmsService
    {
    }

    public interface IChineseTakeawayService
    {
    }

    public interface IUnderpantsService
    {
    }

    public interface IAcneService
    {
    }

    public class PopcornService : IPopcornService
    {
    }

    public class SweatyPalmsService : ISweatyPalmsService
    {
    }

    public class ChineseTakeawayService : IChineseTakeawayService
    {
    }

    public class UnderpantsService : IUnderpantsService
    {
    }

    public class AcneService : IAcneService
    {
    }
}