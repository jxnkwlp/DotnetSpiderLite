using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.NodeHub
{
    public class NodeManager
    {
        private INodeStore _store;

        public NodeManager(INodeStore store)
        {
            _store = store;
        }

        public void AddOrUpdateNode(Node node)
        {

        }

        public void RemoveNode(string nodeId)
        {

        }


    }
}
