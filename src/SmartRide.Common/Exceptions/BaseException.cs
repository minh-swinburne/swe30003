﻿using SmartRide.Common.Responses;

namespace SmartRide.Common.Exceptions;

public class BaseException(string module, ResponseInfo responseInfo, Exception? innerException = null) : Exception(responseInfo.Message, innerException)
{
    public string Module { get; } = module;
    public string Code { get; } = responseInfo.Code;
    public override string Message { get; } = responseInfo.Message;
}
