using System;
using System.ServiceModel;

namespace MTC.Core
{
    public interface ICore
    {
        [OperationContract]
        string GetQuotation();

        [OperationContract]
        string GetCoinValue(string resultUrl);

        [OperationContract]
        string GetCoin(string resultUrl);

        [OperationContract]
        string ConvertTo(string coin);
    }
}