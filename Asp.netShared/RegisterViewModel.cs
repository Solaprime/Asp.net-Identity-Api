﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Asp.netShared
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength =5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", 
            ErrorMessage ="Tested a new Data Attribute confirm Password and Paassword dont match")]
        public string ConfirmPassword { get; set; }

        // this class will reperesent the data transfer object{DTO} that
        // will be sent fromt the Client to the server, it
        // is going to contain the propertuedd and the ATtribute that we will
        // validate

        // we adde some validation atribute to the Dto Propetiex
        // we are going to serialize an object of this class ans send to thr Api
    }
}

// Added some Data attributes to compare Password 
//Added capabolity for all user ti have unique email