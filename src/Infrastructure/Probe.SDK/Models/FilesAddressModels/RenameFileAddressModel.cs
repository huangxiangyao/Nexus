﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiursoft.Probe.SDK.Models.FilesAddressModels
{
    public class RenameFileAddressModel
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        [FromRoute]
        public string SiteName { get; set; }
        [Required]
        [FromRoute]
        public string FolderNames { get; set; }

        public string TargetFileName { get; set; }
    }
}
