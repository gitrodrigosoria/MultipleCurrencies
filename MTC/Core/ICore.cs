using System;
using System.ServiceModel;

namespace MTC.Core
{
    public interface ICore
    {
        string GetQuotation();

        string GetCoinValue(string resultUrl);

        string GetCoin(string resultUrl);

        string ConvertTo(string coin);
    }
}
