using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqDemo
{
    class Program
    {
        private static List<AInfo> aInfos;
        private static List<BInfo> bInfos;

        static void Main(string[] args)
        {
            InitList();
            LeftJoin();
            RightJoin();
            InnerJoin();
            Group1();
            Group2();
            Group3();
            Group4();
        }

        private static void InitList()
        {
            if (aInfos == null)
            {
                aInfos = new List<AInfo>();
            }
            if (bInfos == null)
            {
                bInfos = new List<BInfo>();
            }
            aInfos.Clear();
            bInfos.Clear();

            Guid pkB1 = Guid.NewGuid();
            Guid pkB2 = Guid.NewGuid();
            aInfos.Add(new AInfo(Guid.NewGuid(), pkB1, 0, "pkB1"));
            aInfos.Add(new AInfo(Guid.NewGuid(), pkB1, 0, "pkB1"));
            aInfos.Add(new AInfo(Guid.NewGuid(), pkB1, 0, "pkB1"));
            aInfos.Add(new AInfo(Guid.NewGuid(), pkB2, 1, "pkB2"));
            aInfos.Add(new AInfo(Guid.NewGuid(), pkB2, 1, "pkB2"));
            aInfos.Add(new AInfo(Guid.NewGuid(), pkB2, 1, "pkB2"));
            aInfos.Add(new AInfo(Guid.NewGuid(), Guid.NewGuid(), 2, "any"));
            aInfos.Add(new AInfo(Guid.NewGuid(), Guid.NewGuid(), 3, "any"));

            bInfos.Add(new BInfo(pkB1, 0, "0_a"));
            bInfos.Add(new BInfo(pkB2, 0, "0_b"));
            bInfos.Add(new BInfo(Guid.NewGuid(), 1, "1_a"));
            bInfos.Add(new BInfo(Guid.NewGuid(), 1, "1_b"));
            bInfos.Add(new BInfo(Guid.NewGuid(), 2, "2_a"));
            bInfos.Add(new BInfo(Guid.NewGuid(), 2, "2_b"));
            bInfos.Add(new BInfo(Guid.NewGuid(), 3, "3_a"));
            bInfos.Add(new BInfo(Guid.NewGuid(), 3, "3_b"));
        }

        /// <summary>
        /// 左联
        /// </summary>
        private static void LeftJoin()
        {
            var result = from a in aInfos
                         join b in bInfos
                         on a.PKB equals b.PKB
                         into bAll
                         from bDefault in bAll.DefaultIfEmpty()
                         select new
                         {
                             PKA = a.PKA,
                             AName = a.Name,
                             PKB = bDefault == null ? "[无]" : bDefault.PKB.ToString(),
                             BName = bDefault == null ? "[无]" : bDefault.Name
                         };
            var data = result.ToList();
        }

        /// <summary>
        /// 右连
        /// </summary>
        private static void RightJoin()
        {
            var result = from b in bInfos
                         join a in aInfos
                         on b.PKB equals a.PKB
                         into aAll
                         from aDefault in aAll.DefaultIfEmpty()
                         select new
                         {
                             PKB = b.PKB,
                             BName = b.Name,
                             PKA = aDefault == null ? "[无]" : aDefault.PKA.ToString(),
                             AName = aDefault == null ? "[无]" : aDefault.Name,
                         };
            var data = result.ToList();
        }

        /// <summary>
        /// 内连
        /// </summary>
        private static void InnerJoin()
        {
            var result = from a in aInfos
                         join b in bInfos
                         on a.PKB equals b.PKB
                         select new
                         {
                             PKA = a.PKA,
                             AName = a.Name,
                             AType = a.Type,
                             PKB = b.PKB,
                             BName = b.Name,
                             BType = b.Type,
                         };
            var data = result.ToList();
        }

        /// <summary>
        /// 简单分组
        /// </summary>
        public static void Group1()
        {
            var group = from a in aInfos
                        group a by a.PKB into grp
                        select new
                        {
                            Key = grp.Key,
                            Values = grp.ToList()
                        };
            var data1 = group.ToList();
            var data2 = group.ToDictionary(item1 => item1.Key, item2 => item2.Values);
        }

        /// <summary>
        /// 分组,定义临时变量
        /// </summary>
        public static void Group2()
        {
            var group = from a in aInfos
                        group a by a.PKB into grp
                        let single = grp.First()
                        let value = grp.ToList()
                        select new
                        {
                            Key = grp.Key,
                            Type = single.Type,
                            Count = value.Count(),
                        };
            var data = group.ToList();
        }

        /// <summary>
        /// 按多字段分组
        /// </summary>
        public static void Group3()
        {
            var group = from a in aInfos
                        group a by new{ a.PKB, a.Type} into grp
                        select new
                        {
                            PKB = grp.Key.PKB,
                            Type = grp.Key.Type,
                            Values = grp.ToList()
                        };
            var data = group.ToList();
            var data2 = group.ToDictionary(item1 => item1.Type + "_" + item1.PKB, item2 => item2.Values);
        }

        /// <summary>
        /// 按多个集合中的字段分组
        /// </summary>
        public static void Group4()
        {
            var group = from a in aInfos
                        join b in bInfos
                        on a.PKB equals b.PKB
                        group new { a, b } by new { AType = a.Type, BType = b.Type } into grp
                        select new
                        {
                            AType = grp.Key.AType,
                            BType = grp.Key.BType,
                            AValues = grp.Select(item => item.a).ToList(),
                            BValues = grp.Select(item => item.b).ToList()
                        };
            var data = group.ToList();
        }


    }
}
