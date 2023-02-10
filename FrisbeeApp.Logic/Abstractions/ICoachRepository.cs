﻿using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface ICoachRepository
    {
        Task<bool> AddTraining(Training training, string name);
    }
}
