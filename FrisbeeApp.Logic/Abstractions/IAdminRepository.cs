﻿namespace FrisbeeApp.Logic.Abstractions
{
    public interface IAdminRepository 
    {
        Task<bool> CreateTeam(string teamName);
    }
}