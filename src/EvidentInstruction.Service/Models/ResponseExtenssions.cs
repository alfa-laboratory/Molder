using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Service.Models
{
    public static class ResponseExtenssions
    {
        public static ResponseInfo CreateResponse(this ResponseInfo r, ResponseInfo response)
        {
            return new ResponseInfo()
            {
                Content = response.Content,
                Headers = response.Headers,
                Request = response.Request,
                Exception = response.Exception,
                StatusCode = response.StatusCode,
            };
        }
    }
}
