using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SBISCCMWeb.Models
{
    public class LayoutViewModel
    {
        public string FullName { get; set; }
        public readonly int[] PagingValue = { 5, 10, 15, 20, 30, 50, 75, 100 };
        public int PagingSize { get; set; }

    }
}