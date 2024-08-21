using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Ashen.NodeTreeSystem
{
    [Serializable]
    public class NodeTree
    {
        [OdinSerialize]
        private List<List<NodeUIContainer>> columns;
        public List<List<NodeUIContainer>> Columns
        {
            get
            {
                if (columns == null)
                {
                    columns = new List<List<NodeUIContainer>>();
                }
                return columns;
            }
        }
    }
}