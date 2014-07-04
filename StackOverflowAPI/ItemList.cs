using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StackOverflowAPI
{
    class ItemList<T>
    {
        public List<T> Items { get; set; }
        [JsonProperty(PropertyName = "has_more")]
        public bool HasMore { get; set; }

        [JsonProperty(PropertyName = "quota_max")]
        public int QuotaMax { get; set; }
    }
}
