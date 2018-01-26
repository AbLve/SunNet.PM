using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Principal;
using System.Runtime.InteropServices;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using SF.Framework.ExceptionManager;

namespace SF.Framework.LDAP
{
    public enum ADLoginResult
    {
        OK,
        Invalid_UserName_or_Password,
        Account_Inactive,
        Account_Locked,
        Account_Expired,
        Failed
    }

    public class LDAPHelper
    {
        public static ADLoginResult ValidateUser(string userName, string password, out string accountName)
        {
            accountName = "";
            PrincipalContext ctx = null;
            UserPrincipal up = null;
            try
            {
                ctx = new PrincipalContext(ContextType.Domain, null, SFConfig.LDAP, userName, password);
                up = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, userName);
                if (up != null)
                {
                    accountName = up.SamAccountName;
                    return ADLoginResult.OK;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.Handle(ex);
                if (rethrow) throw;
            }
            finally
            {
                if (up != null)
                    up.Dispose();

                if (ctx != null)
                    ctx.Dispose();
            }

            return ADLoginResult.Failed;
        }

        public static bool ValidateADUser(string userName, string password)
        {
            using (DirectoryEntry de = new DirectoryEntry(SFConfig.LDAPUrl, userName, password, AuthenticationTypes.Secure))
            {
                try
                {
                    return !string.IsNullOrEmpty(de.Name);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.Handle(ex);
                    //if (rethrow) throw;
                }
                finally
                {
                    if (de != null)
                        de.Dispose();
                }


                return false;
            }
        }

        public static bool SearchADUser(string userName, string password)
        {
            using (DirectoryEntry de = new DirectoryEntry(SFConfig.LDAPUrl, userName, password, AuthenticationTypes.Secure))
            {
                try
                {
                    DirectorySearcher search = new DirectorySearcher(de);
                    search.Filter = "(&(sAMAccountName=" + userName + "))";
                    search.SearchScope = SearchScope.Subtree;
                    SearchResultCollection results = search.FindAll();
                    if (results != null)
                    {
                        foreach (SearchResult sr in results)
                        {
                            DirectoryEntry subde = new DirectoryEntry(sr.Path, userName, password, AuthenticationTypes.Secure);
                            return !string.IsNullOrEmpty(subde.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.Handle(ex);
                    //if (rethrow) throw;
                }
                finally
                {
                    if (de != null)
                        de.Dispose();
                }


                return false;
            }
        }

    }
}
