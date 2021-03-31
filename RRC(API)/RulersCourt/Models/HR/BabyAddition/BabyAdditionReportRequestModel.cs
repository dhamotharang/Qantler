﻿using System;

namespace RulersCourt.Models.BabyAddition
{
    public class BabyAdditionReportRequestModel
    {
        public string Status { get; set; }

        public string RequestType { get; set; }

        public string Creator { get; set; }

        public DateTime? RequestDateForm { get; set; }

        public DateTime? RequestDateTo { get; set; }

        public string SmartSearch { get; set; }

        public int UserID { get; set; }
    }
}
