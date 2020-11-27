using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class BenificiaryGraphModel
    {
        public List<GraphNodes> nodes { get; set; }
        public List<GraphEdges> edges { get; set; }
    }

    public class GraphNodes
    {
        public string id { get; set; }
        public NodeAttributes attributes { get; set; }
        public NodeData data { get; set; }
    }

    public class NodeAttributes
    {
        public int radius { get; set; }
        public string color { get; set; }
        public string text { get; set; }
    }

    public class NodeData
    {
        public string area { get; set; }
    }

    public class GraphEdges
    {
        public string id { get; set; }
        public string source { get; set; }
        public string target { get; set; }
    }
}