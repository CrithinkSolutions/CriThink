﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Core.Exceptions
{
    /// <summary>
    /// Custom exception to report Identity Provider errors
    /// </summary>
    public class IdentityOperationException : AggregateException
    {
        public IdentityOperationException(IdentityResult identityResult)
            : base(identityResult?.Errors.Select(e => new Exception(e.Description)))
        { }
    }
}
