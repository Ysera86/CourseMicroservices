using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Services
{
    public class RedisService
    {
        private readonly string _host;
        private readonly string _port;
        private ConnectionMultiplexer _connectionMultiplexer; // redisle bağlantı kurabilmek için
        public RedisService(string host, string port)
        {
            _host = host;
            _port = port;
        }

        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        /// <summary>
        /// 15 db vardı rediste, 1 db test, 1 db prod, 1 db test vs mantıklık
        /// </summary>
        /// <param name="db"></param>
        public IDatabase GetDb(int db = 1) => _connectionMultiplexer.GetDatabase(db);

    }
}
