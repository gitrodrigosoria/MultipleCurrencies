using MTC.Core;
using MTC.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTC
{
    public class Agk : ICore
    {
        const string URL = @"http://www.agkcorretora.com.br/pagina/cotacoes-e-simulador";
        const string PATTERN = @"<input\b(?=((?!(\/>|type=""hidden"")).)*type=""hidden"" id=""especie_venda_).*?\/>";

        public string GetQuotation()
        {
            // Request
            var resultUrl = WebRequestCore.GetUrlContext(URL);

            if (string.IsNullOrEmpty(resultUrl))
                return string.Empty;                 
            
            var sb = new StringBuilder();            

            foreach (var item in RegexCore.GetMatches(PATTERN, resultUrl))
                sb.Append(item.ToString());

            resultUrl = sb.ToString();

            var coin = GetCoin(resultUrl).Split('|');
            var coinValue = GetCoinValue(resultUrl).Split('|');
            var lstCoin = new List<Coin>();

            for (var index = 0; index < coin.Length; index++)
            {
                lstCoin.Add(new Coin() {
                    Key = ConvertTo(coin[index]),
                    Value = coinValue[index]
                });
            }
            
            return JsonConvert.SerializeObject(lstCoin.OrderBy(x => x.Key));
        }

        public string GetCoinValue(string resultUrl)
        {
            // Get all values

            var sbCoinValue = new StringBuilder();
            var result = string.Empty;

            foreach (var item in RegexCore.GetMatches(@"value=['""][\d\s\D]+?['""]", resultUrl))
            {
                result = item.ToString().Replace(@"value=""", string.Empty);
                result = result.Replace(@"""", string.Empty);
                sbCoinValue.AppendFormat("{0}|", result);
            }
            return sbCoinValue.ToString().Trim().TrimEnd('|');
        }

        public string GetCoin(string resultUrl)
        {
            // Get all id's

            var sbCoin = new StringBuilder();
            var result = string.Empty;

            foreach (var item in RegexCore.GetMatches(@"id=['""][\d\s\D]+?['""]", resultUrl))
            {
                result = item.ToString().Replace(@"id=""", string.Empty);
                result = result.Replace("especie_venda_", string.Empty);
                result = result.Replace(@"""", string.Empty);
                sbCoin.AppendFormat("{0}|", result);
            }
            return sbCoin.ToString().Trim().TrimEnd('|');
        }

        public string ConvertTo(string coin)
        {
            if (coin.Equals("EURO"))
                return "EUR";

            if (coin.Equals("YUAN"))
                return "CNY";

            if (coin.Equals("UIP"))
                return "UYU";            

            return coin;
        }
    }
}
