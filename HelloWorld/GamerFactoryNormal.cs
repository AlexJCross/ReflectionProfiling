using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HelloWorld
{
    public class GamerFactoryNormal
    {
        private readonly Dictionary<Type, Type> gamerByGame;
        private readonly GamingServiceContainer container;
    
        IPopcornService popcorn;
        ISweatyPalmsService palms;
        IChineseTakeawayService takeaway;
        IUnderpantsService pants;
        IGamingHeadsetService headset;
    
        public GamerFactoryNormal(
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
    
        public IGamer CreateGamer(MarioKartGame game)
        {
            return new MkGamer(
                this.popcorn, this.palms, this.pants, this.headset, this.takeaway);
        }
    
        public IGamer CreateGamer(CallOfDutyGame game)
        {
            return new CodGamer(
                this.popcorn, this.palms, this.pants, this.headset, this.takeaway);
        }
    }
}
