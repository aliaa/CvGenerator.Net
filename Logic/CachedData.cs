using System;

namespace CvGenerator.Logic
{
    public class CachedData<T>
    {
        public Func<T, T> RefreshFunc { get; set; }

        public DateTime CacheTime { get; private set; }
        public TimeSpan ExpireDuration { get; private set; }

        private T _data;
        public T Data
        {
            private set
            {
                _data = value;
                CacheTime = DateTime.Now;
            }
            get
            {
                if (IsExpired && RefreshFunc != null)
                    Data = RefreshFunc(_data);
                return _data;
            }
        }

        public CachedData(T Data, TimeSpan ExpireDuration)
        {
            this.Data = Data;
            this.ExpireDuration = ExpireDuration;
            this.CacheTime = DateTime.Now;
        }

        public CachedData(T Data, int ExpireSeconds)
        {
            this.Data = Data;
            ExpireDuration = new TimeSpan(0, 0, ExpireSeconds);
            this.CacheTime = DateTime.Now;
        }

        public CachedData(Func<T, T> RefreshFunc, TimeSpan ExpireDuration)
        {
            this.RefreshFunc = RefreshFunc;
            this.ExpireDuration = ExpireDuration;
        }

        public CachedData(Func<T, T> RefreshFunc, int ExpireSeconds)
        {
            this.RefreshFunc = RefreshFunc;
            this.ExpireDuration = new TimeSpan(0, 0, ExpireSeconds);
        }

        public bool IsExpired => DateTime.Now > CacheTime.Add(ExpireDuration);

        public static implicit operator T(CachedData<T> cachedData)
        {
            return cachedData.Data;
        }
    }
}
