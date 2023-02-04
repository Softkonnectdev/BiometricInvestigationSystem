using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UBA_Network_Security_System.Models
{
    public class AccountType
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
        public IEnumerable<AccountType> AccountTypeList()
        {
            return new List<AccountType>
            {
               new AccountType()
               {
                   ID = 1,
                   Type = "Savings"
               },
               new AccountType()
               {
                   ID = 2,
                   Type = "Current"
               },
               new AccountType()
               {
                   ID = 3,
                   Type = "Domiciary"
               },
               new AccountType()
               {
                   ID = 4,
                   Type = "Solo"
               },
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