﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrimeInvestigationSystem.Models.Utility
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "Account Number is critical")]
        public string  AcountNumber { get; set; }

        public HttpPostedFileBase SignatureUpload { get; set; }

        public HttpPostedFileBase PassportUpload { get; set; }

    }
}