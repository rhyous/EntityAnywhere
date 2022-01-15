using Jose;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Rhyous.EntityAnywhere.Services
{
    public class JWTToken : IJWTToken
    {
        private readonly ITokenKeyPair _TokenKeyPair;

        public JWTToken(ITokenKeyPair tokenKeyPair)
        {
            _TokenKeyPair = tokenKeyPair;
        }

        public string GetTokenText(IEnumerable<ClaimDomain> claimDomains) => CreateToken(claimDomains, _TokenKeyPair.PrivateKey);

        public string CreateToken(IEnumerable<ClaimDomain> claimDomains, string privateRsaKey)
        {
            if (claimDomains == null)
                claimDomains = new List<ClaimDomain>();
            RSAParameters rsaParams;
            using (var tr = new StringReader(privateRsaKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new ConfigurationException("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);

                var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                var jsonClaims = JsonConvert.SerializeObject(claimDomains, Formatting.None, jsonSettings);
                return JWT.Encode(jsonClaims, rsa, JwsAlgorithm.RS256);
            }
        }

        public string DecodeToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                RSAParameters rsaParams;

                using (var tr = new StringReader(_TokenKeyPair.PublicKey))
                {
                    var pemReader = new PemReader(tr);
                    var publicKeyParams = pemReader.ReadObject() as RsaKeyParameters;
                    if (publicKeyParams == null)
                    {
                        throw new ConfigurationException("Could not read RSA public key");
                    }
                    rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParams);
                }
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsaParams);
                    // This will throw if the signature is invalid
                    return JWT.Decode(token, rsa, JwsAlgorithm.RS256);
                }
            }
            return string.Empty;
        }

        public List<ClaimDomain> GetClaimDomains(string decodedToken)
        {
            return JsonConvert.DeserializeObject<List<ClaimDomain>>(decodedToken);
        }
    }
}