using MTC.Core;
using MTC.Entities;
using Newtonsoft.Json;
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
                lstCoin.Add(new Coin(){
                    Key = ConvertTo(coin[index]),
                    Value = coinValue[index]} 
                );
            }

            return JsonConvert.SerializeObject(lstCoin.OrderBy(x => x.Key));            
        }

        public string GetCoinValue(string resultUrl)
        {
            // Get all values
            var sbConValue = new StringBuilder();
            var result = string.Empty;

            foreach (var item in RegexCore.GetMatches(@"<div class=""price "">[\d\s\D]+?<\/div>", resultUrl))
            {
                result = item.ToString().Replace(@"<div class=""price "">", string.Empty);
                result = result.Replace("R$", string.Empty);
                result = result.Replace(@"<button class=""buy pull-right"">Comprar</button>", string.Empty);
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

            foreach (var item in RegexCore.GetMatches(@"class=""product-box"">[\d\s\D]+?<img", resultUrl))
            {
                result = item.ToString().Replace(@"<div class=""title small"">", string.Empty);
                result = result.Replace("<img", string.Empty);
                result = result.Split('-')[2];
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
