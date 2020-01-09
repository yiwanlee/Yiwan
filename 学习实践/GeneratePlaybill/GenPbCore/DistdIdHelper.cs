using System;
using System.Collections.Generic;
using System.Text;

namespace GenPbCore
{
    /// <summary>
    /// 基于分布式ID
    /// </summary>
    public sealed class DistdIdHelper
    {
        public static class Snowflake
        {
            const long twEpoch = 1577808000000L; // 起始的时间戳 2020-01-01 00:00:00
            const long workerIdBits = 5L; // 机器id所占的位数
            const long datacenterIdBits = 5L; // 数据中心所占的位数
            const long sequenceBits = 10L; // 毫秒内顺序号所占位数
        }
    }
}
