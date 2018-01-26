<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketBasicInfoView.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.Ticket.TicketBasicInfo" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>
<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagPrefix="custom" TagName="fileUpload" %>

<div class="form-group">
    <label class="col-left-1 lefttext">Project:</label>
    <div class="col-right-col2 righttext"><%=TicketsEntity.ProjectTitle %></div>
    <label class="col-left-1 lefttext">Ticket ID:</label>
    <div class="col-right-col2 righttext"><%=TicketsEntity.TicketID %></div>
</div>
<div class="form-group">
    <label class="col-left-1 lefttext">Type:</label>
    <div class="col-right-col2 righttext">
        <%=TicketsEntity.TicketType.ToText() %>
    </div>
    <label class="col-left-1 lefttext">Priority:</label>
    <div class="col-right-col2 righttext">
        <%=GetPriority((int)TicketsEntity.Priority) %>
    </div>
</div>
<div class="form-group">
    <label class="col-left-1 lefttext">Estimation:</label>
    <div class="col-right-col2 righttext">
        <span class="rightItem">
            <%= TicketsEntity.IsEstimates ? "Needed" : "Not needed" %>
        </span>
    </div>
    <label class="col-left-1 lefttext col-left-status">Status:</label>
    <div class="col-right-col2 righttext">
        <%=GetDisplayStatus() %>
    </div>
</div>
<%if (FromSunnet)
  {%>
<div class="form-group">
    <label class="col-left-1 lefttext">Internal:</label>
    <div class="col-right-col2 righttext"><%=TicketsEntity.IsInternal ? "Yes" : "No" %></div>
    <label class="col-left-1 lefttext">Source:</label>
    <div class="col-right-col2 righttext"><%=TicketsEntity.Source.ToText() %></div>
</div>

<div class="form-group">
    <label class="col-left-1 lefttext">Created By:</label>

    <% if (TicketsEntity.IsInternal)
       {%>
    <div class="col-right-col2 righttext"><%=TicketsEntity.CreatedUserEntity.FirstAndLastName %></div>
    <%}
       else
       { %>
    <%  if (UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.PM || UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.Sales)
        { %>
    <div class="col-right-col2 righttext"><%=TicketsEntity.CreatedUserEntity.FirstAndLastName %></div>
    <%}
        else
        { %>
    <div class="col-right-col2 righttext">CLIENT</div>
    <%} %>
    <%} %>
    <label class="col-left-1 lefttext">Created Date:</label>
    <div class="col-right-col2 righttext"><%=TicketsEntity.CreatedOn.ToString("MM/dd/yyyy") %></div>
</div>

<%}%>
<div class="form-group">

    <label class="col-left-1 lefttext">Title:</label>
    <div class="col-right-1 righttext">
        <%=TicketsEntity.Title %>
    </div>

</div>
<div class="form-group">
    <label class="col-left-1 lefttext">Description:</label>
    <div class="col-right-1 righttext">
        <%=Server.HtmlDecode(TicketsEntity.FullDescription.Replace("\n", "<br/>"))%>
    </div>
</div>

<div class="form-group">
    <label class="col-left-1 lefttext">URL:</label>
    <div class="col-right-1 righttext nowrap">
        <a href="<%=TicketsEntity.URL %>" target="_blank"><%=TicketsEntity.URL %></a>
    </div>
</div>
<div class="form-group">
    <label class="col-left-1 lefttext">Attachments:</label>
    <div class="col-right-1 righttext">
        <custom:fileUpload runat="server" ID="fileUpload" UploadType="View" />
    </div>
</div>


<div class="buttonBox1">
    <table border="0" cellspacing="0" cellpadding="0" style="width: 100%;">
        <tbody>
            <tr class="buttonClass">
                <td align="center" style="text-align: center;">
                    <% 
                        if (ReviewUrl != "")
                        { %>
                    <input type="button" href="<%= ReviewUrl %>" data-toggle="modal" data-target="#modalsmall" value="<%= ReviewName %>" class="saveBtn1 mainbutton" />
                    <% 
                        }
                        if (isReadyForReview)
                        {
                    %>
                    <input type="button" href="<%= UrlApprove %>" data-target="#modalsmall" data-toggle="modal" value="Approve" class="saveBtn1 mainbutton" />
                    <input type="button" href="<%= UrlDeny %>" data-target="#modalsmall" data-toggle="modal" value="Deny" class="saveBtn1 mainbutton" />
                    <%
                        }
                    %>
                    <input type="button" value="Back" class="backBtn mainbutton redirectback" />
                    <asp:PlaceHolder ID="phlWorkingOn" runat="server">
                        <div class="btn-group" style="vertical-align: top;" data-remote="workingon">
                            <button type="button" class="backBtn mainbutton dropdown-toggle" data-workingstatus="<%=TicketsEntity.TicketID %>" data-toggle="dropdown">
                                <span class="text"><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></span>
                                <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="javascript:;" ticket="<%=TicketsEntity.TicketID %>" data-action="setworkingon">WorkingOn</a></li>
                                <li><a href="javascript:;" ticket="<%=TicketsEntity.TicketID %>" data-action="setworkingcomplete">Completed</a></li>
                                <li><a href="javascript:;" ticket="<%=TicketsEntity.TicketID %>" data-action="setworkingcancelled">Cancelled</a></li>
                                <li class="divider"></li>
                                <li><a href="javascript:;" ticket="<%=TicketsEntity.TicketID %>" data-action="setworkingonnone">None</a></li>
                            </ul>
                        </div>
                    </asp:PlaceHolder>
                </td>
            </tr>
        </tbody>
    </table>
</div>
