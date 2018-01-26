﻿/**************************************************************************
 * Developer: 		JackZhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		2014/7/21 21:27:48
 * Description:		Please input class summary
 * Version History:	Created,2014/7/21 21:27:48
 * 
 * 
 **************************************************************************/

namespace FamilyBook.Entity.Common
{
    internal interface IShowUserName
    {
        string LastName { get; set; }
        string FirstName { get; set; }
        string UserName { get; set; }
        string FirstAndLastName { get; }
        string LastNameAndFirst { get; }
    }
}