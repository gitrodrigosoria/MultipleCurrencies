using MTC.Core;
using MTC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace MTC
{
    public class SpMundi : ICore
    {
        const String URL = @"http://www.spmundicambio.com.br/moedas-em-especie";

        public String getQuotation()
        {
            // Request
            String resultUrl = WebRequestCore.getUrlContext(URL);

            if (String.IsNullOrEmpty(resultUrl))
                return String.Empty;            
            
            String[] coin = getCoin(resultUrl).Split('|');
            String[] coinValue = getCoinValue(resultUrl).Split('|');
            List<Coin> lstCoin = new List<Coin>();

            for (Int32 index = 0; index < coin.Length; index++)
            {
                lstCoin.Add(
                    new Coin()
                    {
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
            StringBuilder sbConValue = new StringBuilder();
            String result = String.Empty;

            foreach (var item in RegexCore.getMatches(@"<div class=""price "">[\d\s\D]+?<\/div>", resultUrl))
            {
                result = item.ToString().Replace(@"<div class=""price "">", String.Empty);
                result = result.Replace("R$", String.Empty);
                result = result.Replace(@"<button class=""buy pull-right"">Comprar</button>", String.Empty);
                result = result.Replace("</div>", String.Empty);
                sbConValue.AppendFormat("{0}|", result.Trim());
            }

            return sbConValue.ToString().Trim().TrimEnd('|');
        }

        public String getCoin(String resultUrl)
        {
            // Get all title's
            StringBuilder sbCoin = new StringBuilder();
            String result = String.Empty;

            foreach (var item in RegexCore.getMatches(@"class=""product-box"">[\d\s\D]+?<img", resultUrl))
            {
                result = item.ToString().Replace(@"<div class=""title small"">", String.Empty);
                result = result.Replace("<img", String.Empty);
                result = result.Split('-')[2];
                sbCoin.AppendFormat("{0}|", result.Trim());
            }

            return sbCoin.ToString().Trim().TrimEnd('|');
        }

        public String convertTo(String coin)
        {
            return coin;
        }
    }
}
