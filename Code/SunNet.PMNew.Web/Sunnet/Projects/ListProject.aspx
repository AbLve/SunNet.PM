<%@ Page Title="List Projects" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ListProject.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Projects.ListProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenAddObject() {
            var result = ShowIFrame("AddProject.aspx",
                            780,
                            520,
                            true,
                            "");
            if (result == 0) {
                window.location.reload();
            }
            return false;
        }
        
         
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    List Projects
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td style="width: 50px;">
                Company:
            </td>
            <td style="width: 200px;">
                <asp:DropDownList ID="ddlCompany" CssClass="select150" runat="server">
                </asp:DropDownList>
            </td>
            <td width="50">
                Keyword:
            </td>
            <td style="  width:250px;">
                <asp:TextBox ID="txtKeyword" runat="server" CssClass="input200"></asp:TextBox>
            </td>
            <td width="*">
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <span action="new"><a runat="server" id="AddNewObject" onclick="return OpenAddObject();"
            href="###" title="Add Project">
            <img align="absmiddle" border="0" src="/icons/09.gif" />New</a></span>
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="ModifiedOn" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="desc" />
    </div>
    <div class="mainrightBoxtwo">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th style="width: 15%;" orderby="ProjectCode">
                    Project Code
                </th>
                <th style="width: 15%;" orderby="Title">
                    Title
                </th>
                <th style="width: 10%;" orderby="Priority">
                    Priority
                </th>
                <th style="width: 10%;" orderby="ModifiedOn">
                    Modified On
                </th>
                <th style="width: 10%;">
                    Modified By
                </th>
                <th style="width: 10%;" orderby="StartDate">
                    Start Date
                </th>
                <th style="width: 10%;" orderby="EndDate">
                    End Date
                </th>
                <th style="width: 10%;" orderby="Status">
                    Status
                </th>
                <th style="width: 10%;" orderby="Billable">
                    Over Free Hour
                </th>
            </tr>
            <asp:Repeater ID="rptProjects" runat="server">
                <ItemTemplate>
                    <tr opentype="popwindow" dialogwidth="830" dialogheight="700" dialogtitle="" href="/Sunnet/Projects/EditProject.aspx?id=<%#Eval("ID") %>&companyid=<%#Eval("CompanyID") %>"
                        class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                        <td>
                            <%#Eval("ProjectCode")%>
                        </td>
                        <td>
                            <%#Eval("Title")%>
                        </td>
                        <td>
                            <%#Eval("Priority").ToString().Substring(1)%>
                        </td>
                        <td>
                            <%#Eval("ModifiedOn","{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#GetClientUserName(Convert.ToInt32(Eval("ModifiedBy").ToString()))%>
                        </td>
                        <td>
                            <%#Eval("StartDate","{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#Eval("EndDate", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#((SunNet.PMNew.Entity.ProjectModel.ProjectStatus)Convert.ToInt32(Eval("Status").ToString())).ToString()%>
                        </td>
                        <td>
                            <%#Convert.ToBoolean(Eval("IsOverFreeTime").ToString()) ?
                                "<font color='red'><b>Yes</b></font>" : "No"%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpProjects" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpProjects_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>
</asp:Content>
