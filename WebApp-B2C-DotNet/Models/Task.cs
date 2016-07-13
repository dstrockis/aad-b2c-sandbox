using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_B2C_DotNet.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }
        public DateTime DateModified { get; set; }
    }
}