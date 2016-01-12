using AutoMapper;
using System;

namespace StarterKit.AppStart
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            BusinessEntitiesToDataContracts();
            AgentToBusinessEntities();
			AgentToDataContracts();
        }

        private static void BusinessEntitiesToDataContracts()
        {
         
        }

        private static void AgentToBusinessEntities()
        {

		}

        private static void AgentToDataContracts()
        {
            
        }
    }
}