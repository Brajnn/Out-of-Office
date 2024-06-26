﻿using Out_of_Office.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Out_of_Office.Application.Approval_Request
{
    public class ApprovalRequestDto
    {
        public int ID { get; set; }
        public int ApproverID { get; set; }
        public string ApproverFullName { get; set; }
        public int LeaveRequestID { get; set; }
        public ApprovalStatus Status { get; set; }
        public string Comment { get; set; }
    }
}
