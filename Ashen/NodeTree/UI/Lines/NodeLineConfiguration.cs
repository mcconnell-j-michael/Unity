using System;

namespace Ashen.NodeTreeSystem
{
    [Serializable]
    public class NodeLineConfiguration
    {
        public Node node;
        [Hide]
        public UILocationConfiguration configuration;
    }
}