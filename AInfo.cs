using System;
using System.Collections.Generic;
using System.Text;

namespace LinqDemo
{
    public class AInfo
    {
        public Guid PKA { get; set; }
        public Guid PKB { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }

        public AInfo(Guid pkA, Guid pkB, int type, string name)
        {
            this.PKA = pkA;
            this.PKB = pkB;
            this.Type = type;
            this.Name = name;
        }
    }
}
