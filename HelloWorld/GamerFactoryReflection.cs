using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HelloWorld
{
    public class GamerFactoryReflection
    {
        private readonly Dictionary<Type, Type> gamerByGame;
        private readonly GamingServiceContainer container;

        public GamerFactoryReflection(GamingServiceContainer container)
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
            var constructor = parameterType.GetConstructors().Single();
            
            var arguments = constructor.GetParameters().Select(
                info => this.container.Get(info.ParameterType));

            return constructor.Invoke(arguments.ToArray());
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
}
