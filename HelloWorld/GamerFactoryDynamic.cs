using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HelloWorld
{
    public class GamerFactoryDynamic
    {
        private readonly Dictionary<Type, Type> gamerByGame;
        private readonly GamingServiceContainer container;
    
        IPopcornService popcorn;
        ISweatyPalmsService palms;
        IChineseTakeawayService takeaway;
        IUnderpantsService pants;
        IGamingHeadsetService headset;
    
        public GamerFactoryDynamic(
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
                this.popcorn, this.palms, this.pants, this.headset, this.takeaway);
        }
    
        public IGamer InternalCreateGamer(CallOfDutyGame game)
        {
            return new CodGamer(
                this.popcorn, this.palms, this.pants, this.headset, this.takeaway);
        }
    }
}
