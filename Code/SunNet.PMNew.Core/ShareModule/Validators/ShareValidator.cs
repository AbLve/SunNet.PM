using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/25 21:29:56
 * Description:		Please input class summary
 * Version History:	Created,5/25 21:29:56
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Framework.Core.Validator;

namespace SunNet.PMNew.Core.ShareModule.Validators
{
    public class ShareValidator : BaseValidator<ShareEntity>
    {
        protected override void ValidateExtraRules(ShareEntity o)
        {

        }
    }
}
