using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StackOverflowAPI
{
    public class StackOverflow
    {
        public async Task<IEnumerable<Tag>> GetTags()
        {
            var url = "http://api.stackexchange.com/2.2/tags?site=stackoverflow&order=desc&sort=popular&filter=default";

            GZipWebClient wc = new GZipWebClient();
           wc.Headers.Add(HttpRequestHeader.Accept, "application/json");
       
            var tagsJson = await wc.DownloadStringTaskAsync(new Uri(url));

            var itemList = JsonConvert.DeserializeObject<ItemList<Tag>>(tagsJson);

            return itemList.Items;

        }
    }


}
