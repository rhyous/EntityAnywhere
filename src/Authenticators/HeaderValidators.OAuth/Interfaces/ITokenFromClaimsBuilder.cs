//---------------------------------------------------------------------------
// Copyright (C) Ivanti Corporation 2020. All rights reserved.
//---------------------------------------------------------------------------

using System.Security.Claims;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface ITokenFromClaimsBuilder { IAccessToken Build(ClaimsPrincipal claims); }
}
