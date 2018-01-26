<%@ Page Title="Survey" Language="C#" MasterPageFile="~/NoLeftmenu.master" AutoEventWireup="true"
    CodeBehind="Survey.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.Survey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contactTitle">Survey</div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">

        <tr>
            <td>
                <table border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td width="70">Company:</td>
                            <td width="300">
                                <asp:TextBox ID="txtCompanyName" ReadOnly="true" runat="server" CssClass="Input310"></asp:TextBox>
                            </td>
                            <td width="50">Name:</td>
                            <td>
                                <asp:TextBox ID="txtUserName" ReadOnly="true" runat="server" CssClass="Input310"></asp:TextBox></td>
                        </tr>
                    </tbody>
                </table>
                <label></label>
            </td>
            <td class="work_star_box">&nbsp;</td>
        </tr>


        <tr>
            <td colspan="2">
                <p>
                    Dear Valued Customer:
                </p>
                <p>
                    Thank you for giving us the opportunity to better serve you. Please help us by taking
                            a few minutes to tell us about the service that you have received so far. We appreciate
                            your business and want to make sure we continue to meet your expectations.
                </p>
                <p>
                    This survey consists of 10 short questions. Survey results will be viewed only by
                            the President of the company, Mrs. Sandy Huang.
                </p>
                <p>
                    Sincerely,<br />
                SunNet Solutions
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr>
        <tr>
            <td height="10" colspan="2"></td>
        </tr>
        <tr>
            <td colspan="2" class="contentTitle titleeventlist">We will initiate this survey by asking you a couple of questions regarding the Project
                        Manager assigned to you.
            </td>
        </tr>
        <tr>
            <td height="10" colspan="2"></td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>1</strong>. The Project Manager assigned to me was very courteous.
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey1" runat="server">
                                <asp:ListItem>&nbsp;&nbsp;Strongly agree</asp:ListItem>
                                <asp:ListItem>&nbsp;&nbsp;Agree</asp:ListItem>
                                <asp:ListItem>&nbsp;&nbsp;Neutral</asp:ListItem>
                                <asp:ListItem>&nbsp;&nbsp;Disagree</asp:ListItem>
                                <asp:ListItem>&nbsp;&nbsp;Strongly disagree </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>2</strong>. The Project Manager handled my questions &amp; requests in a
                        timely manner.
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey2" runat="server">
                                <asp:ListItem>&nbsp;Strongly agree</asp:ListItem>
                                <asp:ListItem>&nbsp;Agree</asp:ListItem>
                                <asp:ListItem>&nbsp;Neutral</asp:ListItem>
                                <asp:ListItem>&nbsp;Disagree</asp:ListItem>
                                <asp:ListItem>&nbsp;Strongly disagree</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>3</strong>. The Project Manager was available when needed.
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey3" runat="server">
                                <asp:ListItem>&nbsp;Strongly agree</asp:ListItem>
                                <asp:ListItem>&nbsp;Agree</asp:ListItem>
                                <asp:ListItem>&nbsp;Neutral</asp:ListItem>
                                <asp:ListItem>&nbsp;Disagree</asp:ListItem>
                                <asp:ListItem>&nbsp;Strongly disagree</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>4</strong>. The Project Manager was very knowledgeable.
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey4" runat="server">
                                <asp:ListItem>&nbsp;Strongly agree</asp:ListItem>
                                <asp:ListItem>&nbsp;Agree</asp:ListItem>
                                <asp:ListItem>&nbsp;Neutral</asp:ListItem>
                                <asp:ListItem>&nbsp;Disagree</asp:ListItem>
                                <asp:ListItem>&nbsp;Strongly disagree</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>5</strong>. The Project Manager was on time for client meetings.
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey5" runat="server">
                                <asp:ListItem>&nbsp;Always</asp:ListItem>
                                <asp:ListItem>&nbsp;Usually </asp:ListItem>
                                <asp:ListItem>&nbsp;Rarely </asp:ListItem>
                                <asp:ListItem>&nbsp;Never </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>6</strong>. What about the Project Manager could be improved? Were they:
                        (You may select multiple answers)
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="survey6" runat="server">
                                <asp:ListItem>&nbsp;Not patient</asp:ListItem>
                                <asp:ListItem>&nbsp;Not enthusiastic</asp:ListItem>
                                <asp:ListItem>&nbsp;Didn't listen carefully</asp:ListItem>
                                <asp:ListItem>&nbsp;Not familiar with my system</asp:ListItem>
                                <asp:ListItem>&nbsp;Unfriendly</asp:ListItem>
                                <asp:ListItem>&nbsp;Unresponsive</asp:ListItem>
                                <asp:ListItem>&nbsp;No improvement needed</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="10" colspan="2"></td>
        </tr>
        <tr>
            <td colspan="2" class="contentTitle titleeventlist">Thank you for your answers. For the final part, we will ask you general questions
                        regarding your experience with Sunnet.
            </td>
        </tr>
        <tr>
            <td height="10" colspan="2"></td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>7</strong>. In evaluating your most recent experience with us, what was the
                        quality of service you received?
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey7" runat="server">
                                <asp:ListItem>&nbsp;Superior</asp:ListItem>
                                <asp:ListItem>&nbsp;Satisfactory</asp:ListItem>
                                <asp:ListItem>&nbsp;About average</asp:ListItem>
                                <asp:ListItem>&nbsp;Unsatisfactory (Please explain below):</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <label>
                                <asp:TextBox ID="txt7" runat="server" Rows="4" TextMode="MultiLine" CssClass="inputfaq"></asp:TextBox>
                            </label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>8</strong>. How likely are you to work with SunNet Solutions again?
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey8" runat="server">
                                <asp:ListItem>&nbsp;Very likely</asp:ListItem>
                                <asp:ListItem>&nbsp;Likely</asp:ListItem>
                                <asp:ListItem>&nbsp;Undecided</asp:ListItem>
                                <asp:ListItem>&nbsp;Unlikely</asp:ListItem>
                                <asp:ListItem>&nbsp;Very unlikely ( Please explain below):</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <label>
                                <asp:TextBox ID="txt8" runat="server" Rows="4" TextMode="MultiLine" CssClass="inputfaq"></asp:TextBox>
                            </label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>9</strong>. How likely are you to refer SunNet Solutions?
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey9" runat="server">
                                <asp:ListItem>&nbsp;Very likely</asp:ListItem>
                                <asp:ListItem>&nbsp;Likely</asp:ListItem>
                                <asp:ListItem>&nbsp;Undecided</asp:ListItem>
                                <asp:ListItem>&nbsp;Unlikely</asp:ListItem>
                                <asp:ListItem>&nbsp;Very unlikely ( Please explain below):</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <label>
                                <asp:TextBox ID="txt9" runat="server" Rows="4" TextMode="MultiLine" CssClass="inputfaq"></asp:TextBox>
                            </label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>10</strong>. Compared to other IT vendors that you have worked with, how
                        would you rate SunNet's pricing?
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey10" runat="server">
                                <asp:ListItem>&nbsp;Very Competitive</asp:ListItem>
                                <asp:ListItem>&nbsp;Competitive</asp:ListItem>
                                <asp:ListItem>&nbsp;Fair/ Average</asp:ListItem>
                                <asp:ListItem>&nbsp;Low</asp:ListItem>
                                <asp:ListItem>&nbsp;Unsure</asp:ListItem>
                                <asp:ListItem>&nbsp;I have not worked with other IT vendors in the past</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="surveyTitle">
                <strong>11</strong>. What is the main reason that led you to select us as your IT
                        provider?
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="survey11" runat="server">
                                <asp:ListItem>&nbsp;Competitive Pricing</asp:ListItem>
                                <asp:ListItem>&nbsp;Our Expertise</asp:ListItem>
                                <asp:ListItem>&nbsp;Client Referral</asp:ListItem>
                                <asp:ListItem>&nbsp;Dissatisfied with previous IT Provider</asp:ListItem>
                                <asp:ListItem>&nbsp;Other Please explain</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <label>
                                <asp:TextBox ID="txt11" runat="server" Rows="4" TextMode="MultiLine" CssClass="inputfaq"></asp:TextBox>
                            </label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <strong>We sincerely appreciate any comments or suggestions you may have which will
                            improve your satisfaction with our service. </strong>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="99%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <label>
                                <asp:TextBox ID="txt12" runat="server" Rows="5" TextMode="MultiLine" CssClass="inputfaq"></asp:TextBox>
                            </label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">Thank you for your feedback. We value you as a customer and will take your input
                        into serious consideration while providing services in the future.
                        <br />
                <br />
                If you have any comments or concerns about this survey or our services, please contact:
                        <br />
                <br />
                <a href="mailto:sandy.huang@sunnet.us" style="color: #2e6bc8; text-decoration: underline;">sandy.huang@sunnet.us</a>
            </td>
        </tr>
        <tr>
            <td height="40" colspan="2">
                <div class="buttonBox2">
                    <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" runat="server" Text="Submit" OnClientClick="return OnSubmit();"
                        OnClick="btnSave_Click" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
