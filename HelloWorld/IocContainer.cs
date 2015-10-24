namespace HelloWorld
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
                { typeof(CallOfDutyGame) , typeof(CodGamer)},
                { typeof(MarioKartGame) ,typeof(MkGamer) }
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
        IGamingHeadsetService acne;

        public GamerFactory2(
            IPopcornService popcorn,
            ISweatyPalmsService palms,
            IChineseTakeawayService takeaway,
            IUnderpantsService pants,
            IGamingHeadsetService acne)
        {
            this.popcorn = popcorn;
            this.palms = palms;
            this.takeaway = takeaway;
            this.pants = pants;
            this.acne = acne;
        }

        public IGamer CreateGamer(MarioKartGame game)
        {
            return new MkGamer(
                this.popcorn, this.palms, this.pants, this.acne, this.takeaway);
        }

        public IGamer CreateGamer(CallOfDutyGame game)
        {
            return new CodGamer(
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
        IGamingHeadsetService acne;

        public GamerFactory3(
            IPopcornService popcorn,
            ISweatyPalmsService palms,
            IChineseTakeawayService takeaway,
            IUnderpantsService pants,
            IGamingHeadsetService acne)
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
            return new MkGamer(
                this.popcorn, this.palms, this.pants, this.acne, this.takeaway);
        }

        public IGamer InternalCreateGamer(CallOfDutyGame game)
        {
            return new CodGamer(
                this.popcorn, this.palms, this.pants, this.acne, this.takeaway);
        }
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

    public interface IGamingHeadsetService
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

    public class GamingHeadsetService : IGamingHeadsetService
    {
    }
}