using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.FileModel
{
    public enum FileSourceType
    {
        Ticket = 2,
        Project = 1,
        FeedBack = 3,
        Company = 4,
        WorkRequest = 5,
        WorkRequestScope = 6,
        KnowledgeShare = 7 // HARD CODE : ShareRepositorySqlDataProvider
    }
}
