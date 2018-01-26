using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/25 23:25:31
 * Description:		Please input class summary
 * Version History:	Created,5/25 23:25:31
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Framework.Core.Validator;

namespace SunNet.PMNew.Core.ShareModule.Validators
{
    public class ShareTypeValidator : BaseValidator<ShareTypeEntity>
    {
        protected override void ValidateExtraRules(ShareTypeEntity o)
        {
        }
    }
}
