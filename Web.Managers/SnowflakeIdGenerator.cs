namespace Web.Managers
{
    public class SnowflakeIdGenerator
    {
        // 定义一些常量
        private const long Epoch = 1012752000000L; // 起始时间戳
        private const int MachineIdBits = 0;  // 机器ID长度
        private const int SequenceBits = 12;  // 序列号长度
        private const int MaxSequence = -1 ^ (-1 << SequenceBits);  // 最大序列号

        private long _lastTimestamp = -1L;
        private long _sequence = 0L;

        // 生成 ID 的方法
        public long GenerateId()
        {
            var timestamp = GetTimestamp();

            if (timestamp != _lastTimestamp)
            {
                _sequence = 0L;  // 不同的时间戳，序列号归零
                _lastTimestamp = timestamp;
            }
            else
            {
                _sequence = (_sequence + 1) & MaxSequence;  // 同一时间戳内，序列号递增
                if (_sequence == 0)
                {
                    timestamp = WaitForNextMillis(_lastTimestamp);  // 同一毫秒内序列号溢出，等待下一毫秒
                }
            }

            // 生成雪花ID
            long id = ((timestamp - Epoch) << (MachineIdBits + SequenceBits)) | (_sequence);
            return id;
        }

        private long GetTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        private long WaitForNextMillis(long lastTimestamp)
        {
            var timestamp = GetTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetTimestamp();  // 等待直到下一毫秒
            }
            return timestamp;
        }

        public DateTime ParseTime(long id)
        {
            long timestamp = (id >> SequenceBits) + Epoch;  // 恢复时间戳
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
        }
    }
}
