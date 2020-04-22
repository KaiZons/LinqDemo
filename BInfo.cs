using System;
using System.Collections.Generic;
using System.Text;

namespace LinqDemo
{
    public class BInfo
    {
        public Guid PKB { get; set; }
        public int Type {get;set;}
        public string Name { get; set; }

        public BInfo(Guid pkB, int type, string name)
        {
            this.PKB = pkB;
            this.Type = type;
            this.Name = name;
        }
    }
}
