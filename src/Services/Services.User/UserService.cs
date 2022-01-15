using Rhyous.Collections;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public partial class UserService : ServiceCommonAlternateKey<User, IUser, long, string>,
                                       IUserService
    {
        private readonly IDuplicateUsernameDetector _DuplicateUsernameDetector;
        private readonly IPasswordManager _PasswordManager;

        public UserService(IServiceHandlerProviderAltKey<User, IUser, long, string> serviceHandlerProviderAltKey,
                           IDuplicateUsernameDetector duplicateUsernameDetector,
                           IPasswordManager passwordManager)
            : base(serviceHandlerProviderAltKey) 
        {
            _DuplicateUsernameDetector = duplicateUsernameDetector;
            _PasswordManager = passwordManager;
        }

        public override Task<List<IUser>> AddAsync(IEnumerable<IUser> users)
        {
            _DuplicateUsernameDetector.Detect(users.Select(u => u.Username), true);
            _PasswordManager.SetOrHashPassword(users);
            return _ServiceHandlerProvider.AddHandler.AddAsync(users);
        }

        public override IUser Update(long id, PatchedEntity<IUser, long> patchedEntity)
        {
            var existingUser = Get(id);
            if (existingUser == null)
                throw new EntityNotFoundException("No such user to update. User id: " + id);
            var user = patchedEntity.Entity;
            var changedProperties = patchedEntity.ChangedProperties;
            var changedPropClone = patchedEntity.ChangedProperties.ToHashSet(StringComparer.OrdinalIgnoreCase);
            if (changedProperties.Contains(nameof(User.Username), StringComparer.OrdinalIgnoreCase)
             && !existingUser.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase))
                _DuplicateUsernameDetector.Detect(new[] { user.Username }, true);

            existingUser.ConcreteCopy(user as User, changedPropClone);

            if (string.IsNullOrWhiteSpace(user.Password) && user.IsHashed)
                throw new InvalidUserDataException("The password cannot be blank if IsHashed is set to true.");

            _PasswordManager.SetOrHashPassword(user, changedPropClone.Contains(nameof(user.Password)) || changedPropClone.Contains(nameof(User.IsHashed)));
            if (existingUser.Password != user.Password && !changedPropClone.Contains(nameof(user.Password)))
                changedPropClone.Add(nameof(user.Password));
            if (existingUser.Salt != user.Salt && !changedPropClone.Contains(nameof(user.Salt)))
                changedPropClone.Add(nameof(user.Salt));
            patchedEntity.ChangedProperties = changedPropClone;
            return _ServiceHandlerProvider.UpdateHandler.Update(id, patchedEntity);
        }
    }
}