<%@ Page Title="Add Company" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="AddCompany.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Companys.AddCompany" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Add Company
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainrightBoxtwo">
        <div>
            <div class="owmainactionBox">
                <div class="tickettop_left">
                    General Information
                </div>
            </div>
            <br />
            <table width="95%" border="0" align="center" cellpadding="5" cellspacing="0">
                <tr>
                    <th width="10%">
                        Company:<span class="redstar">*</span>
                    </th>
                    <td width="35%">
                        <asp:TextBox ID="txtCompanyName" ValidatorTitle="The Company field cannot be left blank."
                            Validation="true" length="1-200" runat="server" CssClass="input200"></asp:TextBox>
                    </td>
                    <td width="5%">
                        &nbsp;
                    </td>
                    <th width="15%">
                        Address 1:<span class="redstar">*</span>
                    </th>
                    <td width="35%">
                        <asp:TextBox ID="txtAddress1" Validation="true" ValidatorTitle="The Address field cannot be left blank."
                            length="1-500" runat="server" CssClass="input200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        Phone:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtPhone" Validation="true" ValidatorTitle="The Phone field cannot be left blank."
                            RegType="phone" length="1-20" runat="server" CssClass="input200"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <th>
                        Address 2:
                    </th>
                    <td>
                        <asp:TextBox ID="txtAddress2" runat="server" CssClass="input200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        Fax:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtFax" Validation="true" ValidatorTitle="The Fax number field cannot be left blank."
                            RegType="fax" length="1-20" runat="server" CssClass="input200"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <th>
                        City:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtCity" Validation="true" ValidatorTitle="The City field cannot be left blank."
                            length="1-100" runat="server" CssClass="input200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        Website:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtWebsite" Validation="true" ValidatorTitle="Please enter the website address."
                            length="1-100" runat="server" CssClass="input200"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <th>
                        State:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:DropDownList runat="server" CssClass="select205" ID="ddlState">
                            <asp:ListItem Value="0">Please Select</asp:ListItem>
                            <asp:ListItem Value="1">AK</asp:ListItem>
                            <asp:ListItem Value="2">AL</asp:ListItem>
                            <asp:ListItem Value="3">AR</asp:ListItem>
                            <asp:ListItem Value="4">AS</asp:ListItem>
                            <asp:ListItem Value="5">AZ</asp:ListItem>
                            <asp:ListItem Value="6">CA</asp:ListItem>
                            <asp:ListItem Value="7">CO</asp:ListItem>
                            <asp:ListItem Value="8">CT</asp:ListItem>
                            <asp:ListItem Value="9">DC</asp:ListItem>
                            <asp:ListItem Value="10">DE</asp:ListItem>
                            <asp:ListItem Value="11">FL</asp:ListItem>
                            <asp:ListItem Value="12">GA</asp:ListItem>
                            <asp:ListItem Value="13">GU</asp:ListItem>
                            <asp:ListItem Value="14">HI</asp:ListItem>
                            <asp:ListItem Value="15">IA</asp:ListItem>
                            <asp:ListItem Value="16">ID</asp:ListItem>
                            <asp:ListItem Value="17">IL</asp:ListItem>
                            <asp:ListItem Value="18">IN</asp:ListItem>
                            <asp:ListItem Value="19">KS</asp:ListItem>
                            <asp:ListItem Value="20">KY</asp:ListItem>
                            <asp:ListItem Value="21">LA</asp:ListItem>
                            <asp:ListItem Value="22">MA</asp:ListItem>
                            <asp:ListItem Value="23">MD</asp:ListItem>
                            <asp:ListItem Value="24">ME</asp:ListItem>
                            <asp:ListItem Value="25">MI</asp:ListItem>
                            <asp:ListItem Value="26">MN</asp:ListItem>
                            <asp:ListItem Value="27">MO</asp:ListItem>
                            <asp:ListItem Value="28">MS</asp:ListItem>
                            <asp:ListItem Value="29">MT</asp:ListItem>
                            <asp:ListItem Value="30">NC</asp:ListItem>
                            <asp:ListItem Value="31">ND</asp:ListItem>
                            <asp:ListItem Value="32">NE</asp:ListItem>
                            <asp:ListItem Value="33">NH</asp:ListItem>
                            <asp:ListItem Value="34">NJ</asp:ListItem>
                            <asp:ListItem Value="35">NM</asp:ListItem>
                            <asp:ListItem Value="36">NV</asp:ListItem>
                            <asp:ListItem Value="37">NY</asp:ListItem>
                            <asp:ListItem Value="38">OH</asp:ListItem>
                            <asp:ListItem Value="39">OK</asp:ListItem>
                            <asp:ListItem Value="40">OR</asp:ListItem>
                            <asp:ListItem Value="41">PA</asp:ListItem>
                            <asp:ListItem Value="42">PR</asp:ListItem>
                            <asp:ListItem Value="43">RI</asp:ListItem>
                            <asp:ListItem Value="44">SC</asp:ListItem>
                            <asp:ListItem Value="45">SD</asp:ListItem>
                            <asp:ListItem Value="46">TN</asp:ListItem>
                            <asp:ListItem Value="47">TX</asp:ListItem>
                            <asp:ListItem Value="48">UT</asp:ListItem>
                            <asp:ListItem Value="49">VA</asp:ListItem>
                            <asp:ListItem Value="50">VI</asp:ListItem>
                            <asp:ListItem Value="51">VT</asp:ListItem>
                            <asp:ListItem Value="52">WA</asp:ListItem>
                            <asp:ListItem Value="53">WI</asp:ListItem>
                            <asp:ListItem Value="54">WV</asp:ListItem>
                            <asp:ListItem Value="55">WY</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: none;">
                    <th colspan="2">
                        Assigned System Url:&nbsp;&nbsp;client.sunnet.us
                    </th>
                    <td>
                        &nbsp;
                    </td>
                    <th>
                        <%--Zip Code:--%>
                    </th>
                    <td>
                    </td>
                </tr>
            </table>
            <div class="btnBoxone">
                <asp:Button ID="btnSave" CssClass="btnone" runat="server" Text="Save" OnClick="btnSave_Click"
                    OnClientClick="return Validate();" />
                <input name="button2" id="btnClientCancel" type="button" class="btnone" value="Clear" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        jQuery(function() {
            jQuery("#<%=txtPhone.ClientID %>").add("#<%=txtFax.ClientID %>").mask("(999) 999-9999");
        });
    </script>

</asp:Content>
