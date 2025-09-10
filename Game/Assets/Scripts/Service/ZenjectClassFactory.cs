using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Service
{
    public class ZenjectClassFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public ZenjectClassFactory(DiContainer container)
        {
            _container = container;
        }

        public TClass Create<TClass>(params object[] extraArgs)
        {
            var args = new List<TypeValuePair>();

            foreach (var extraArg in extraArgs)
            {
                args.Add(new TypeValuePair(extraArg.GetType(), extraArg));
            }
            
            return _container.InstantiateExplicit<TClass>(args);
        }
    }
}