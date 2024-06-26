﻿using MediatR;
using Taka.App.Motor.Domain.Entitites;

namespace Taka.App.Motor.Domain.Commands
{
    public class ResultCreateMotorcycleCommand : IRequest
    {
        public Motorcycle Motorcycle { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
    }
}
