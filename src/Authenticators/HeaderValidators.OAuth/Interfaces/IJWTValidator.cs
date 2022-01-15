//---------------------------------------------------------------------------
// Copyright (C) Ivanti Corporation 2020. All rights reserved.
//---------------------------------------------------------------------------

using System.Security.Claims;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface IJWTValidator { Task<ClaimsPrincipal> ValidateAsync(string jwt); }
}