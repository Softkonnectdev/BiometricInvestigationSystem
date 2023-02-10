using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrimeInvestigationSystem.Models.Utility
{
    public class CrimeType
    {
        public int ID { get; set; }
        public string Type { get; set; }
        
    }

    public class _Gender
    {
        public string Name { get; set; }

    }


    public class MaritalStatus
    {
        public int ID { get; set; }
        public string Status { get; set; } 

    }

    public class State
    {
        public string Name { get; set; }
    }

    public class Country
    {
        public int ID { get; set; }
        public string Name { get; set; }

    }

    public class IdentificationType
    {
        public int ID { get; set; }
        public string Name { get; set; }

    }


    public class UtilityHelpers 
    {
        public List<State> StateList()
        {
            List<State> _states = new List<State>()
            {
                new State(){ Name = "Abia"},new State(){ Name = "Adamawa"},new State(){ Name = "Akwa-Ibom"},new State(){ Name = "Anambra"},
                new State(){ Name = "bonyi"},new State(){ Name = "Edo"},new State(){ Name = "Ekiti"},new State(){ Name = "Enugu"},
                new State(){ Name = "FCT"},new State(){ Name = "Gombe"},new State(){ Name = "Imo"},new State(){ Name = "Jigawa"},
                new State(){ Name = "Kaduna"},new State(){ Name = "Kano"},new State(){ Name = "Kastina"},new State(){ Name = "Kebbi"},
                new State(){ Name = "Kogi"},new State(){ Name = "Kwara"},new State(){ Name = "Lagos"},new State(){ Name = "Nasarawa"},
                new State(){ Name = "Niger"},new State(){ Name = "Ogun"},new State(){ Name = "Ondo"},new State(){ Name = "Osun"},
                new State(){ Name = "Oyo"},new State(){ Name = "Plateau"},new State(){ Name = "Rivers"},new State(){ Name = "Sokoto"},
                new State(){ Name = "Taraba"},new State(){ Name = "Yobe"},new State(){ Name = "Zamfara"}
            };
            return _states;
        }

        public IEnumerable<CrimeType> CrimeTypeList()
        {
            return new List<CrimeType>
            {
               new CrimeType()
               {
                   ID = 1,
                   Type = "Murder"
               },
               new CrimeType()
               {
                   ID = 2,
                   Type = "Theft"
               },
               new CrimeType()
               {
                   ID = 3,
                   Type = "Bribe"
               },
               new CrimeType()
               {
                   ID = 4,
                   Type = "Sexual Assault"
               },
               new CrimeType()
               {
                   ID = 5,
                   Type = "Other"
               }
            };
        }

        public IEnumerable<MaritalStatus> MaritalStatusList()
        {
            return new List<MaritalStatus>
            {
               new MaritalStatus()
               {
                   ID = 1,
                   Status = "Single"
               },
               new MaritalStatus()
               {
                   ID = 2,
                   Status = "Married"
               },
               new MaritalStatus()
               {
                   ID = 3,
                   Status = "In Relationship"
               },
               new MaritalStatus()
               {
                   ID = 4,
                   Status = "Divorced"
               },
            };
        }

        public IEnumerable<Country> CountryList()
        {
            return new List<Country>
            {
               new Country()
               {
                   ID = 1,
                   Name = "Nigeria"
               },
               new Country()
               {
                   ID = 2,
                   Name = "Ghana"
               },
               new Country()
               {
                   ID = 3,
                   Name = "Egypt"
               },
               new Country()
               {
                   ID = 4,
                   Name = "Cameron"
               },
            };
        }

        public IEnumerable<IdentificationType> IdentificationTypeList()
        {
            return new List<IdentificationType>
            {
               new IdentificationType()
               {
                   ID = 1,
                   Name = "National Identity"
               },
               new IdentificationType()
               {
                   ID = 2,
                   Name = "Voters' Card"
               },
               new IdentificationType()
               {
                   ID = 3,
                   Name = "Drivers' License"
               },
               new IdentificationType()
               {
                   ID = 4,
                   Name = "Passport"
               },
            };
        }

        public IEnumerable<_Gender> GetGenders()
        {
            return new List<_Gender>()
            {
                new _Gender()
                {
                    Name = "Male"
                },
                new _Gender()
                {
                    Name = "Female"
                }
            };
        }
    }

}