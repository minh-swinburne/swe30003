using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRide.Common.Responses;

public readonly record struct ResponseInfo
{
    public string Module { get; init; }
    public string Code { get; init; }
    public string Message { get; init; }
}
