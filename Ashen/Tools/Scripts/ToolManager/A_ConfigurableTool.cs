using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public abstract class A_ConfigurableTool<T, E> : A_EnumeratedTool<T> where T : A_ConfigurableTool<T, E> where E : A_Configuration<T, E>
    {
        private E config;
        protected E Config
        {
            get
            {
                if (config == null)
                {
                    config = DefaultValues.Instance.config.GetConfiguration<E>();
                }
                return config;
            }
            set
            {
                config = value;
            }
        }

        public void Initialize(E config)
        {
            this.config = config;
            Initialize();
        }

        public void Initialize(E config, Dictionary<string, object> arguments)
        {
            this.config = config;
            ReadArguments(arguments);
            Initialize();
        }

        public virtual void ReadArguments(Dictionary<string, object> arguments) { }
    }
}