using Newtonsoft.Json.Linq;
using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using ICredentials = Rhyous.EntityAnywhere.Interfaces.ICredentials;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    internal class Data
    {
        #region User
        public static IUser User = new User { Id = 1, Username = "TestUser1" };

        public static User FullUser = new User
        {
            CreateDate = DateTimeOffset.Now,
            CreatedBy = 1225566,
            Enabled = true,
            ExternalAuth = true,
            Id = 123546,
            IsHashed = true,
            LastUpdated = DateTimeOffset.Now,
            LastUpdatedBy = 1225566,
            Password = "TestPassword",
            Salt = "TestSalt",
            Username = "TestUser1"
        };

        public static User EmailUser = new User
        {
            CreateDate = DateTimeOffset.Now,
            CreatedBy = 1225567,
            Enabled = true,
            ExternalAuth = true,
            Id = 123546,
            IsHashed = true,
            LastUpdated = DateTimeOffset.Now,
            LastUpdatedBy = 1225566,
            Password = "TestPassword",
            Salt = "TestSalt",
            Username = "john.doe@domain.tld"
        };

        public static User EmailFirstPartUser = new User
        {
            CreateDate = DateTimeOffset.Now,
            CreatedBy = 1225567,
            Enabled = true,
            ExternalAuth = true,
            Id = 123546,
            IsHashed = true,
            LastUpdated = DateTimeOffset.Now,
            LastUpdatedBy = 1225566,
            Password = "TestPassword",
            Salt = "TestSalt",
            Username = "john.doe"
        };

        public static OdataObject<User, long> OdataObjectUser = new OdataObject<User, long> {
            Id = FullUser.Id,
            IdProperty = "IdPropertyTest",
            Object = FullUser,       
            Parent = new User(),
            PropertyUris = new List<OdataUri>(),
            RelatedEntityCollection =  new ParentedList<RelatedEntityCollection>(),
            Uri = new Uri("http://www.ivanttest.com")
        };

        public static OdataObject<User, long> OdataObjectUserEmail = new OdataObject<User, long>
        {
            Id = EmailUser.Id,
            IdProperty = "IdPropertyTest",
            Object = EmailUser,
            Parent = new User(),
            PropertyUris = new List<OdataUri>(),
            RelatedEntityCollection = new ParentedList<RelatedEntityCollection>(),
            Uri = new Uri("http://www.ivanttest.com")
        };

        public static OdataObject<User, long> OdataObjectUserEmailFirstPart = new OdataObject<User, long>
        {
            Id = EmailFirstPartUser.Id,
            IdProperty = "IdPropertyTest",
            Object = EmailFirstPartUser,
            Parent = new User(),
            PropertyUris = new List<OdataUri>(),
            RelatedEntityCollection = new ParentedList<RelatedEntityCollection>(),
            Uri = new Uri("http://www.ivanttest.com")
        };

        public static ICredentials Credentials => new Credentials
        {
            User = "usernametest",
            Password = "passwordtest"
        };

        public static ICredentials DomainCredentials => new Credentials
        {
            User = "domaintest\\usernametest",
            Password = "passwordtest"
        };

        public static ICredentials EmailCredentials => new Credentials
        {
            User = "domaintest@usernametest",
            Password = "passwordtest"
        };
        #endregion

        #region Related Entities
        public static RelatedEntity RelatedEntityRole1 => new RelatedEntity
        {
            Id = "1",
            Object = new JRaw("{\"Id\":1,\"Name\":\"Role1\",\"LandingPageId\":\"1\"}")
        };
        public static RelatedEntity RelatedEntityRole2 => new RelatedEntity
        {
            Id = "2",
            Object = new JRaw("{\"Id\":1,\"Name\":\"Role2\",\"LandingPageId\":\"2\"}")
        };

        public static RelatedEntity RelatedEntityGroup => new RelatedEntity
        {
            Id = "2",
            Object = new JRaw("{\"Id\":2,\"Name\":\"Group2\"}")
        };

        public static RelatedEntityCollection RelatedEntityCollectionRole
        {
            get
            {
                var relatedEntityCollection = new RelatedEntityCollection { RelatedEntity = "UserRole" };
                relatedEntityCollection.Add(RelatedEntityRole1);
                relatedEntityCollection.Add(RelatedEntityRole2);
                return relatedEntityCollection;
            }
        }
        public static RelatedEntityCollection RelatedEntityCollectionGroup
        {
            get
            {
                var relatedEntityCollection = new RelatedEntityCollection { RelatedEntity = "UserGroup" };
                relatedEntityCollection.Add(RelatedEntityGroup);
                return relatedEntityCollection;
            }
        }

        public static List<RelatedEntityCollection> RelatedEntityCollectionGroupAndRole
        {
            get { return new List<RelatedEntityCollection> { RelatedEntityCollectionGroup, RelatedEntityCollectionRole }; }
        }
        #endregion

        #region Claim Configurations

        public static ClaimConfiguration ClaimConfigurationUsername => new ClaimConfiguration { Domain = "User", Name = "Username", Entity = "User", EntityProperty = "Username", EntityIdProperty = "Id" };

        public static List<ClaimConfiguration> ClaimConfigurationListUser = new List<ClaimConfiguration> { ClaimConfigurationUsername };

        public static ClaimConfiguration ClaimConfigurationRole => new ClaimConfiguration { Domain = "UserRole", Name = "Role", Entity = "UserRole", EntityProperty = "Name", EntityIdProperty = "Id" };

        public static List<ClaimConfiguration> ClaimConfigurationListRole = new List<ClaimConfiguration> { ClaimConfigurationRole };

        public static ClaimConfiguration ClaimConfigurationGroup => new ClaimConfiguration { Domain = "UserGroup", Name = "Group", Entity = "UserGroup", EntityProperty = "Name", EntityIdProperty = "Id" };

        public static List<ClaimConfiguration> ClaimConfigurationListGroup = new List<ClaimConfiguration> { ClaimConfigurationGroup };

        public static ClaimConfiguration ClaimConfigurationLandingPageType => new ClaimConfiguration { Domain = "UserRole", Name = "LandingPageId", Entity = "UserRole", EntityProperty = "LandingPageId", EntityIdProperty = "Id", RelatedEntityIdProperty = "UserRole" };
        
        #endregion

        #region Plugins
        public static Token Token = new Token
        {
            ClaimDomains = new List<ClaimDomain>(),
            CreateDate = DateTimeOffset.Now,
            Text = "TestingText",
            CredentialEntityId = 123654
        };
        #endregion
    }
}
