<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/SiteMobile.Master" AutoEventWireup="true" CodeBehind="SiteMobileMasterDemo.aspx.cs" Inherits="SunNet.PMNew.PM2014.Mobile.SiteMobileMasterDemo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="meta" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadLinkCss" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadCss" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadLinkScript" runat="server">
    <script src="/Scripts/layer-v3.1.0/layer/mobile/layer.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadScript" runat="server">
    <script>
        $(function() {
            layer.open({
                content:"HeadScript"
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolderContentApp" runat="server">
    <div class="content-app">
        <div class="search clearfix">
            <div class="search-div">
                <label>Keyword:</label>
                <div class="search-input">
                    <input type="text" />
                </div>
            </div>
            <div class="search-div">
                <label>Project:</label>
                <div class="search-input">
                    <input type="text" />
                </div>
            </div>
            <div class="search-div">
                <label>Status:</label>
                <div class="search-input">
                    <input type="text" />
                </div>
            </div>
            <div class="search-div">
                <label>Created By:</label>
                <div class="search-input">
                    <input type="text" />
                </div>
            </div>
            <div class="search-div">
                <label>Type:</label>
                <div class="search-input">
                    <input type="text" />
                </div>
            </div>
            <div class="search-div">
                <label>Priority:</label>
                <div class="search-input">
                    <input type="text" />
                </div>
            </div>
            <div class="search-div">
                <label>Responsible User:</label>
                <div class="search-input">
                    <input type="text" />
                </div>
            </div>
            <div class="search-div">
                <label>&nbsp;</label>
                <div class="search-input">
                    <button>
                        <img src="/images/icons/search.png" /></button>
                </div>
            </div>
        </div>
        <div class="priority-app">
            <ul class="clearfix">
                <li><span style="background: #00A519;"></span>Low</li>
                <li><span style="background: #E7E073;"></span>Medium</li>
                <li><span style="background: #F37F03;"></span>High</li>
                <li><span style="background: #D14343;"></span>Emergency</li>
            </ul>
        </div>
        <div class="table-app">
            <table cellspacing="0" cellpadding="0">
                <tr>
                    <th>Project Name</th>
                    <th>Ticket ID</th>
                    <th>Title</th>
                    <th>Resp User</th>
                    <th>Priority</th>
                    <th>Status</th>
                    <th>Updated</th>
                    <th>Created By</th>
                    <th class="acticon">Action</th>
                </tr>
                <tr>
                    <td>Sunnet.PM</td>
                    <td>2949</td>
                    <td>PM mobile responsive</td>
                    <td>Jolly Li</td>
                    <td><span class="low"></span></td>
                    <td>Developing</td>
                    <td>10/30/2017</td>
                    <td>David Huang</td>
                    <td class="acticon">
                        <a>
                            <img src="/Images/icons/pmreview.png" /></a>
                        <a>
                            <img src="/Images/icons/category.png" /></a>
                        <a>
                            <img src="/Images/icons/share.png" /></a>
                        <a>
                            <img src="/Images/icons/packup1.png" /></a>
                    </td>
                </tr>
                <tr>
                    <td>Sunnet.PM</td>
                    <td>2949</td>
                    <td>PM mobile responsive</td>
                    <td>Jolly Li</td>
                    <td><span class="medium"></span></td>
                    <td>Developing</td>
                    <td>10/30/2017</td>
                    <td>David Huang</td>
                    <td class="acticon">
                        <a>
                            <img src="/Images/icons/pmreview.png" /></a>
                        <a>
                            <img src="/Images/icons/category.png" /></a>
                        <a>
                            <img src="/Images/icons/share.png" /></a>
                        <a>
                            <img src="/Images/icons/packup1.png" /></a>
                    </td>
                </tr>
                <tr>
                    <td>Sunnet.PM</td>
                    <td>2949</td>
                    <td>PM mobile responsive</td>
                    <td>Jolly Li</td>
                    <td><span class="high"></span></td>
                    <td>Developing</td>
                    <td>10/30/2017</td>
                    <td>David Huang</td>
                    <td class="acticon">
                        <a>
                            <img src="/Images/icons/pmreview.png" /></a>
                        <a>
                            <img src="/Images/icons/category.png" /></a>
                        <a>
                            <img src="/Images/icons/share.png" /></a>
                        <a>
                            <img src="/Images/icons/packup1.png" /></a>
                    </td>
                </tr>
                <tr>
                    <td>Sunnet.PM</td>
                    <td>2949</td>
                    <td>PM mobile responsive</td>
                    <td>Jolly Li</td>
                    <td><span class="emergency"></span></td>
                    <td>Developing</td>
                    <td>10/30/2017</td>
                    <td>David Huang</td>
                    <td class="acticon">
                        <a>
                            <img src="/Images/icons/pmreview.png" /></a>
                        <a>
                            <img src="/Images/icons/category.png" /></a>
                        <a>
                            <img src="/Images/icons/share.png" /></a>
                        <a>
                            <img src="/Images/icons/packup1.png" /></a>
                    </td>
                </tr>
                <tr>
                    <td>Sunnet.PM</td>
                    <td>2949</td>
                    <td>PM mobile responsive</td>
                    <td>Jolly Li</td>
                    <td><span class="low"></span></td>
                    <td>Developing</td>
                    <td>10/30/2017</td>
                    <td>David Huang</td>
                    <td class="acticon">
                        <a>
                            <img src="/Images/icons/pmreview.png" /></a>
                        <a>
                            <img src="/Images/icons/category.png" /></a>
                        <a>
                            <img src="/Images/icons/share.png" /></a>
                        <a>
                            <img src="/Images/icons/packup1.png" /></a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolderLinkScritp" runat="server">
    <script src="/Scripts/knockout-3.1.0.js"></script>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentPlaceHolderScritp" runat="server">
    <script>
        $(function () {
            alert("ContentPlaceHolderScritp");
        });
    </script>
</asp:Content>
