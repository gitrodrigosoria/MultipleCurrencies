using System;

namespace MTC.Core
{
    public interface ICore
    {
        String getQuotation();
        String getCoinValue(String resultUrl);
        String getCoin(String resultUrl);
        String convertTo(String coin);
    }
}
