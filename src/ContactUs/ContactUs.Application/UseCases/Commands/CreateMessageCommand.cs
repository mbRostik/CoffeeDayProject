using ContactUs.Application.Contracts.ChangeDTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactUs.Application.UseCases.Commands
{

    public record CreateMessageCommand(SendMessageDTO model) : IRequest;

}
