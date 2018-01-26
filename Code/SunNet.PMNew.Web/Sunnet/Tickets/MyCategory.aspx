<%@ Page Title="My Categories" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="MyCategory.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.MyCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Related A
        {
            display: block;
            float: left;
            margin-right: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    My Categories
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainactionBox">
        <span id="emptycategory"><a href="#">
            <img src="/icons/26.gif" border="0" align="absmiddle">
            Empty Current Category </a></span><span id="deletecategory"><a href="#">
                <img src="/icons/35.gif" border="0" align="absmiddle">
                Destroy Current Category</a></span></div>
    <div class="mainrightBoxtwo">
        <table id="dataTickets" width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th width="8">
                    &nbsp;
                </th>
                <th width="90">
                    Project
                </th>
                <th width="80">
                    Ticket Code
                </th>
                <th width="*">
                    Title
                </th>
                <th width="140">
                    Status
                </th>
                <th width="60">
                    Due Date
                </th>
                <th width="40">
                    Action
                </th>
                <th width="100">
                    Related Tickets
                </th>
            </tr>
            <asp:Repeater ID="rptTickets" runat="server">
                <ItemTemplate>
                    <tr type="tickets" class="<%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>"
                        opentype="newtab" dialogtitle="" href="/Sunnet/Tickets/TicketDetail.aspx?tid=<%#Eval("TicketID") %>"
                        id='<%#Eval("ID") %>'>
                        <td class="action" type="showtasks" ticketid='<%#Eval("ID") %>'>
                            <img src="/icons/04.gif">
                        </td>
                        <td>
                            <%#Eval("ProjectTitle")%>
                        </td>
                        <td>
                            <%#Eval("TicketCode")%>
                        </td>
                        <td>
                            <%#Eval("Title") %>
                        </td>
                        <td>
                            <%#Eval("Status").ToString().Replace("_"," ")%>
                        </td>
                        <td>
                            <%#Eval("DeliveryDate", "{0:MM/dd/yyyy}").ToString() == "01/01/1753" 
                                ? "" : Eval("DeliveryDate", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td class="action">
                            <a action="removefromcategory" href="###" ticketid="<%#Eval("ID") %>">
                                <img src="/icons/26.gif" title="Remove from Category"></a>
                        </td>
                        <td class="Related">
                            <%#GetRelatedTickets(Eval("ID"),Eval("ProjectID")) %>
                        </td>
                    </tr>
                    <tr type="tasks" style="display: none;">
                        <td colspan="9" class="listrownblank">
                            <table id='tasks<%#Eval("ID") %>' width="100%" border="0" cellpadding="0" cellspacing="0"
                                class="subList">
                                <tr class="listrowfour">
                                    <td width="100%">
                                        Tasks<%--&nbsp;<a href="###"><img src="/icons/ico_refresh.gif" /></a>--%>
                                    </td>
                                </tr>
                                <tr class="listrowsix load">
                                    <td>
                                        <img alt="loading" src="/Images/loading16_blue.gif" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpTickets" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpTickets_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>

    <script type="text/javascript">
        function OpenTicketDetail(tid) {
            window.open("/Sunnet/Tickets/TicketDetail.aspx?tid=" + tid);
        }
        
           function ViewTicketModuleDialog(selectTicketId) {
            var result = ShowIFrame("/Sunnet/Tickets/ViewRelatedTicket.aspx?tid=" + selectTicketId, 870, 620, true, "View Related Ticket");
            if (result == 0) {
                window.href = window.href;
            }
        }
        
        var CategoryID = <%=CateGoryID %>;
        
        var TasksLoaded=[];
        
        var GetDataTimeOut=0;
        function SetTasksLoaded(ticketid,loaded)
        {
            eval("TasksLoaded.ticket"+ticketid+"="+loaded.toString()+";");
        }
        function GetTasksLoaded(ticketid)
        {
           var result= eval("TasksLoaded.ticket"+ticketid);
           if(result==undefined||result==null)
           {
           return false;
           }
           return result; 
        }
        function GetClassName(index)
        {
            if(index%2==0)
            {
                return "listrowsix";
            }
            return "listrowfive";
        }
        
        var trtemplate="<tr class='{ClassName}'><td>{Title}<font color='red'>{Complete}</font>>{Description}</td></tr>";
        function GetObjsToHtml(objs)
        {
            if(objs==undefined||objs==null||objs.length==0)
            {
                 var classname=GetClassName(0);
                 var thisHtml=trtemplate.replace("{ClassName}",classname)
                                        .replace("{Title}","NoRecords")
                                        .replace("{Description}","0 task")
                                        .replace("{Complete}","");
                return thisHtml;
            }
            var html="";
            for(var i=0;i<objs.length;i++)
            {
                var classname=GetClassName(i);
                var thisHtml=trtemplate.replace("{ClassName}",classname)
                                        .replace("{Title}",objs[i].Title)
                                        .replace("{Description}",objs[i].Description)
                                        .replace("{Complete}",objs[i].IsCompleted==true?"[Completed]":"");
                html=html+thisHtml;
            }
            return html;
        }
        jQuery(function() {
            document.getElementById("leftMenu11").click();
            
            jQuery("#dataTickets td[type='showtasks']").css("cursor","pointer").click(function(){
                var _this=jQuery(this);
                _this.parent().toggleClass("listrowthree").next().slideToggle(100);
                var _ticketid=_this.attr("ticketid");
                var _loaded=GetTasksLoaded(_ticketid);
                
                if(_loaded==false)
                {
                     GetDataTimeOut=setTimeout(function(){
                        jQuery("#tasks"+_ticketid+" tr").remove(".load");
                        var classname=GetClassName(0);
                        var thisHtml=trtemplate.replace("{ClassName}",classname)
                                                .replace("{Title}","TimeOut")
                                                .replace("{Description}","Get data fail,time out.")
                                                .replace("{Complete}","");
                        jQuery("#tasks"+_ticketid).append(thisHtml);                             
                    },10000);
                    jQuery.getJSON(
                            "/Do/TicketTasks.ashx?r=" + Math.random(),
                            {
                                type:"GetTasksByTicket",
                                ticketid: _ticketid
                            },
                            function(taskList) {
                                var html=GetObjsToHtml(taskList);
                                jQuery("#tasks"+_ticketid+" tr").remove(".load");
                                jQuery("#tasks"+_ticketid).append(html);
                                SetTasksLoaded(_ticketid,true);
                                clearTimeout(GetDataTimeOut);
                            }
                    );
                }
            }).each(function(){
                var _this=jQuery(this);
                SetTasksLoaded(_this.attr("ticketid"),false);
            });
            
            // empty category
            jQuery("#emptycategory").click(function(){
                 jQuery.ajax({
                        type: "POST",
                        url: "/Do/CateGory.ashx?r=" + Math.random(),
                        data: {
                            type: "removefromcategory",
                            ticketid: 0,
                            cagetoryid: CategoryID
                        },
                        success: function(responseData) {
                            if (responseData == true || responseData == "true"||responseData == "True" ) {
                               MessageBox.Alert3(null, "Operation successful!", function() {
                                    window.location.reload();
                                });
                            }
                            else
                            {
                             ShowMessage("Current Category is empty now!",0,false,false);
                            }
                        }
                    });
            });
            // empty category
            jQuery("#deletecategory").click(function(){
                 jQuery.ajax({
                        type: "POST",
                        url: "/Do/CateGory.ashx?r=" + Math.random(),
                        data: {
                            type: "deletecategory",
                            ticketid: 0,
                            cagetoryid: CategoryID
                        },
                        success: function(responseData) {
                            if (responseData == true || responseData == "true"||responseData == "True") {
                                MessageBox.Alert3(null, "Operation successful!", function() {
                                    window.location.href="/sunnet/tickets/listTicket.aspx";
                                });
                            }
                            else
                            {
                             ShowMessage("Operation failed!",0,false,false);
                            }
                        }
                    });
            });
        });
    </script>

</asp:Content>
