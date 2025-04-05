using SmartRide.Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRide.Common.Exceptions;

public class BaseException(string module, ResponseInfo responseInfo) : Exception
{
    public string Module { get; } = module;
    public string Code { get; } = responseInfo.Code;
    public override string Message { get; } = responseInfo.Message;
}
