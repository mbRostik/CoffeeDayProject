using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.ChangeDTOs;
using Users.Application.Contracts.SendDTOs;

namespace Users.Application.UseCases.Commands
{
    public record ChangeUserAvatarCommand(ChangeProfilePhotoDTO model) : IRequest<GetUserProfileDTO>;

}
