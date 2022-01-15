//---------------------------------------------------------------------------
// Copyright (C) Ivanti Corporation 2020. All rights reserved.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public class BearerDecoder : IBearerDecoder
    {
        private readonly IJWTValidator _JWTValdator;
        private readonly ITokenFromClaimsBuilder _TokenFromClaimsBuilder;

        public BearerDecoder(IJWTValidator jWTValdator,
                             ITokenFromClaimsBuilder tokenFromClaimsBuilder)
        {
            _JWTValdator = jWTValdator;
            _TokenFromClaimsBuilder = tokenFromClaimsBuilder;
        }

        public async Task<IAccessToken> DecodeAsync(string tokenText)
        {
            if (string.IsNullOrWhiteSpace(tokenText)) { throw new ArgumentException($"'{nameof(tokenText)}' cannot be null or whitespace.", nameof(tokenText)); }

            var claims = await _JWTValdator.ValidateAsync(tokenText);
            return _TokenFromClaimsBuilder.Build(claims);
        }
    }
}