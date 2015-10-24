using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorld
{
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
}
