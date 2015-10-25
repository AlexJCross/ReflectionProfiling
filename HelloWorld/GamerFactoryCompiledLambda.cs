using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HelloWorld
{
    public class GamerFactoryCompiledLambda
    {
        private readonly Dictionary<Type, Type> gamerByGame;
        private readonly Dictionary<Type, ObjectActivator> creationMethodByGame;
        private readonly GamingServiceContainer container;

        private delegate IGamer ObjectActivator(params object[] args);

        public GamerFactoryCompiledLambda(GamingServiceContainer container)
        {
            this.gamerByGame = LoadFromGamerConfigXml();
            this.container = container;
            this.creationMethodByGame = CreateCompiledLambdas();
        }
    
        public IGamer CreateGamer(IGame game)
        {
            return (IGamer)this.GetObject(game.GetType());
        }
    
        private object GetObject(Type parameterType)
        {
            ObjectActivator createdActivator = this.creationMethodByGame[parameterType];

            var gamerType = this.gamerByGame[parameterType];

            var constructor = gamerType.GetConstructors().Single();
            var arguments = constructor.GetParameters().Select(info => this.container.Get(info.ParameterType));
    
            //create an instance:
            return createdActivator(arguments.ToArray());
        }

        private static ObjectActivator GetActivator(ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param =
                Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp =
                new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);

                Expression paramCastExp =
                    Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator), newExp, param);

            //compile it
            ObjectActivator compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }

        private static Dictionary<Type, Type> LoadFromGamerConfigXml()
        {
            return new Dictionary<Type, Type>
            {
                { typeof(CallOfDutyGame) , typeof(CodGamer)},
                { typeof(MarioKartGame) ,typeof(MkGamer) }
            };
        }

        private Dictionary<Type, ObjectActivator> CreateCompiledLambdas()
        {
            var compiledLambdasByType = new Dictionary<Type, ObjectActivator>();

            foreach (var kvp in this.gamerByGame)
            {
                var gamer = kvp.Value;
                ConstructorInfo ctor = gamer.GetConstructors().First();
                compiledLambdasByType[kvp.Key] = GetActivator(ctor);
            }

            return compiledLambdasByType;
        }
    }
}
