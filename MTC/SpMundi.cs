using MTC.Core;
using MTC.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTC
{
    public class SpMundi : ICore
    {
        const string URL = @"http://www.spmundicambio.com.br/moedas-em-especie";

        public string GetQuotation()
        {
            // Request
            var resultUrl = WebRequestCore.GetUrlContext(URL);

            if (string.IsNullOrEmpty(resultUrl))
                return string.Empty;

            var coin = GetCoin(resultUrl).Split('|');
            var coinValue = GetCoinValue(resultUrl).Split('|');
            var lstCoin = new List<Coin>();

            for (var index = 0; index < coin.Length; index++)
            {
                lstCoin.Add(new Coin()
                {
                    Key = ConvertTo(coin[index]),
                    Value = coinValue[index]
                }
                );
            }

            return JsonConvert.SerializeObject(lstCoin.OrderBy(x => x.Key));
        }

        public string GetCoinValue(string resultUrl)
        {
            // Get all values
            var sbConValue = new StringBuilder();
            var result = string.Empty;

            foreach (var item in RegexCore.GetMatches(@"<div class=""price"">(.*?)</div>", resultUrl))
            {
                result = item.ToString().Replace(@"<div class=""price"">", string.Empty);
                result = result.Replace("</div>", string.Empty);
                sbConValue.AppendFormat("{0}|", result.Trim());
            }
            
            return sbConValue.ToString().Trim().TrimEnd('|');
        }

        public string GetCoin(string resultUrl)
        {
            // Get all title's
            var sbCoin = new StringBuilder();
            var result = string.Empty;

            foreach (var item in RegexCore.GetMatches(@"<img src=""image/flags[\d\s\D]+?<div class=""price", resultUrl))
            {
                result = item.ToString()
                    .Replace("flag-icon", "flagicon")
                    .Split('-')[1]
                    .Substring(0, 4);

                sbCoin.AppendFormat("{0}|", result.Trim());
            }
            
            return sbCoin.ToString().Trim().TrimEnd('|');
        }

        public string ConvertTo(string coin)
        {
            return coin;
        }
    }
}
