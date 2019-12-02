using System;

namespace Ophelia.Web.Service.Extensions
{
    public static class ServiceResultExtensions
    {
        public static ServiceObjectResult<T> AsServiceResult<T>(this T data) where T : class
        {
            return new ServiceObjectResult<T> { Data = data };
        }
    }
}
