using System.Collections.Generic;

/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/26 01:08:08
 * Description:		Please input class summary
 * Version History:	Created,5/26 01:08:08
 * 
 * 
 **************************************************************************/
namespace SunNet.PMNew.Entity.ShareModel.DTO
{
    public class SearchShareResponse
    {
        public SearchShareResponse()
        {
            Dataset = new List<ShareEntity>();
        }
        public List<ShareEntity> Dataset { get; set; }
        public int Count { get; set; }
    }
}
