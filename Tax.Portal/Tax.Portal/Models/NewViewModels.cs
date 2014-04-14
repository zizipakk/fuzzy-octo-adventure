using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Tax.Data.Models;
using System.Web.Mvc;


namespace Tax.Portal.Models
{
    /// <summary>
    /// Összefűzött html szöveg, a belső hírfolyam
    /// </summary>
    public class NewViewModel : BootstrapViewModel
    {
        public string FullText { get; set; }        
    }
}