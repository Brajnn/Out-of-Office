﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Out_of_Office.Application.Project.Query.GetAllProjectsQuery
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
        public int UserId { get; set; }
        public string UserRole { get; set; }
    }
}
