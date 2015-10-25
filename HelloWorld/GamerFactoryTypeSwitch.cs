using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HelloWorld
{
    public class GamerFactoryTypeSwitch
    {
        private readonly Dictionary<Type, Type> gamerByGame;
        private readonly GamingServiceContainer container;
    
        IPopcornService popcorn;
        ISweatyPalmsService palms;
        IChineseTakeawayService takeaway;
        IUnderpantsService pants;
        IGamingHeadsetService headset;
        
        private Dictionary<Type, Func<IGame, IGamer>> factoryMethodByGame;
    
        public GamerFactoryTypeSwitch(
            IPopcornService popcorn,
            ISweatyPalmsService palms,
            IChineseTakeawayService takeaway,
            IUnderpantsService pants,
            IGamingHeadsetService headset)
        {
            this.popcorn = popcorn;
            this.palms = palms;
            this.takeaway = takeaway;
            this.pants = pants;
            this.headset = headset;
    
            this.factoryMethodByGame = new Dictionary<Type, Func<IGame, IGamer>>()
            {
                { typeof(MarioKartGame), this.InternalCreateGamer1 },
                { typeof(CallOfDutyGame),this.InternalCreateGamer2 } 
            };
        }
    
        public IGamer CreateGamer(IGame game)
        {
            return this.factoryMethodByGame[game.GetType()].Invoke(game);
        }
    
        public IGamer InternalCreateGamer1(IGame game)
        {
            //var inevitableCast = (MarioKartGame)game;
            //if (inevitableCast == null)
            //{
            //    throw new Exception();
            //}
    
            return new MkGamer(
                this.popcorn, this.palms, this.pants, this.headset, this.takeaway);
        }
    
        public IGamer InternalCreateGamer2(IGame game)
        {
            //var inevitableCast = (CallOfDutyGame)game;
            //if (inevitableCast == null)
            //{
            //    throw new Exception();
            //}
    
            return new CodGamer(
                this.popcorn, this.palms, this.pants, this.headset, this.takeaway);
        }
    }
}
