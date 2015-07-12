using MTC.Core;
using MTC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace MTC
{
    public class Agk : ICore
    {
        const String URL = @"http://www.agkcorretora.com.br/pagina/cotacoes-e-simulador";

        public String getQuotation()
        {
            // Request
            String resultUrl = WebRequestCore.getUrlContext(URL);

            if (String.IsNullOrEmpty(resultUrl))
                return String.Empty;                 
            
            StringBuilder sb = new StringBuilder();            

            foreach (var item in RegexCore.getMatches(@"<input\b(?=((?!(\/>|type=""hidden"")).)*type=""hidden"" id=""especie_venda_).*?\/>", resultUrl))
                sb.Append(item.ToString());

            resultUrl = sb.ToString();

            String[] coin = getCoin(resultUrl).Split('|');
            String[] coinValue = getCoinValue(resultUrl).Split('|');
            List<Coin> lstCoin = new List<Coin>();

            for (Int32 index = 0; index < coin.Length; index++)
            {
                lstCoin.Add(
                    new Coin() { 
                        coin = convertTo(coin[index])
                        , coin_value = coinValue[index] 
                    }
                );
            }

            return new JavaScriptSerializer().Serialize(lstCoin.OrderBy(x => x.coin));
        }

        public String getCoinValue(String resultUrl)
        {
            // Get all values

            StringBuilder sbCoinValue = new StringBuilder();
            String result = String.Empty;

            foreach (var item in RegexCore.getMatches(@"value=['""][\d\s\D]+?['""]", resultUrl))
            {
                result = item.ToString().Replace(@"value=""", String.Empty);
                result = result.Replace(@"""", String.Empty);
                sbCoinValue.AppendFormat("{0}|", result);
            }
            return sbCoinValue.ToString().Trim().TrimEnd('|');
        }

        public String getCoin(String resultUrl)
        {
            // Get all id's

            StringBuilder sbCoin = new StringBuilder();
            String result = String.Empty;

            foreach (var item in RegexCore.getMatches(@"id=['""][\d\s\D]+?['""]", resultUrl))
            {
                result = item.ToString().Replace(@"id=""", String.Empty);
                result = result.Replace("especie_venda_", String.Empty);
                result = result.Replace(@"""", String.Empty);
                sbCoin.AppendFormat("{0}|", result);
            }
            return sbCoin.ToString().Trim().TrimEnd('|');
        }

        public String convertTo(String coin)
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