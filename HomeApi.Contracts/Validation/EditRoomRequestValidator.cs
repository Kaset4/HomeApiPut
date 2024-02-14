using FluentValidation;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Contracts.Validation
{
    public class EditRoomRequestValidator: AbstractValidator<EditRoomRequest>
    {
        public EditRoomRequestValidator() 
        {
            RuleFor(x => x.Area).NotEmpty();
            RuleFor(x => x.NewName).NotEmpty();
            RuleFor(x => x.GasConnected).NotEmpty().Must(BeSupported);
            RuleFor(x => x.Voltage).NotEmpty();
        }

        private bool BeSupported(bool connected)
        {
            if (connected == true || connected == false) 
            {
                return true;
            }
            return false;
        }
    }
}
