using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRide.Common.Exceptions;

public class BaseException(string code, string module, string message) : Exception
{
    public string Code { get; } = code;
    public string Module { get; } = module;
    public override string Message { get; } = message;
}
