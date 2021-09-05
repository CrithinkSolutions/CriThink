using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Repositories;
using MediatR;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UpdateUserRoleCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _roleRepository = roleRepository ??
                throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userId = request.Id;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null)
                throw new ResourceNotFoundException("User not found", userId);

            var role = await _roleRepository.FindRoleByNameAsync(request.Role);
            if (role is null)
                throw new ResourceNotFoundException("The role is not valid", $"Role: '{request.Role}'");

            var currentUserRoles = await _userRepository.GetUserRolesAsync(user);
            await _userRepository.RemoveUserFromRolesAsync(user, currentUserRoles);
            await _userRepository.AddUserToRoleAsync(user, role.Name);

            return Unit.Value;
        }
    }
}
