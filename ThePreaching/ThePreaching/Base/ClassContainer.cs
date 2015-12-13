using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace ThePreaching.Base
{
    public class ClassContainer
    {
        public static WindsorContainer Instance;
        public ClassContainer()
        {
            Instance = new WindsorContainer();
            Instance.Register(Component.For<BaseViewModel>());

        }
    }
}