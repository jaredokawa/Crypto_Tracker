using Newtonsoft.Json;

namespace Crypto_Tracker.Models
{
    public class Coin
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("inst_price_usd")]
        public decimal Price { get; set; }
        [JsonProperty("inst_market_cap_plain")]
        public decimal MarketCap { get; set; }

    }
}
