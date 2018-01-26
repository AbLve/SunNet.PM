<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientMaintenancePlan.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.ClientMaintenancePlan" %>
<table>
    <tr>
        <td id="tdOptions">
            <asp:RadioButton ID="rbtnHasPlan" GroupName="hasno" Text="Has a maintenance plan"
                runat="server" /><br />
            <asp:RadioButton ID="rbtnHasNoPlan" GroupName="hasno" Text="Does not have a maintenance plan"
                runat="server" /><br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbtnNEEDAPPROVAL" GroupName="no"
                Text="Needs a quote approval" runat="server" /><br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbtnDONTNEEDAPPROVAL" GroupName="no"
                Text="Does not need a quote approval" runat="server" /><br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbtnALLOWME" GroupName="no"
                Text="Allow me to choose per submission" runat="server" /><br />
        </td>
        <td style="vertical-align: top;">
            <div id="dvPlan" name="dvPlan" style="display: none;">
                <span style="margin-left: 40px;">
                    <label>
                        Total Hours</label><span class="redstar">*</span>
                    <asp:TextBox runat="server" ID="txtTotalHours" Width="68"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdTotalHours" />
                </span><span style="margin-left: 40px;">
                    <label>
                        Remain Hours</label>
                    <asp:TextBox runat="server" ID="txtRemainHours" Width="68" Enabled="false"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdRemainHours" />
                </span>
            </div>
        </td>
    </tr>
</table>

<script type="text/javascript">
    jQuery(function() {
        var rbtnHasPlan = jQuery("#<%=rbtnHasPlan.ClientID %>");
        var rbtnHasNoPlan = jQuery("#<%=rbtnHasNoPlan.ClientID %>");
        var rbtnNEEDAPPROVAL = jQuery("#<%=rbtnNEEDAPPROVAL.ClientID %>");
        var rbtnDONTNEEDAPPROVAL = jQuery("#<%=rbtnDONTNEEDAPPROVAL.ClientID %>");
        var rbtnALLOWME = jQuery("#<%=rbtnALLOWME.ClientID %>");
        //        .removeAttr("checked").eq(1).attr("checked", "checked");
        rbtnHasPlan.click(function() {
            rbtnHasNoPlan.removeAttr("checked");
            rbtnNEEDAPPROVAL.removeAttr("checked");
            rbtnDONTNEEDAPPROVAL.removeAttr("checked");
            rbtnALLOWME.removeAttr("checked");
        });
        rbtnHasNoPlan.click(function() {
            rbtnHasPlan.removeAttr("checked");
        });
        rbtnNEEDAPPROVAL.click(function() {
            rbtnHasNoPlan.attr("checked", "checked");
            rbtnHasPlan.removeAttr("checked");
            rbtnDONTNEEDAPPROVAL.removeAttr("checked");
            rbtnALLOWME.removeAttr("checked");
        });
        rbtnDONTNEEDAPPROVAL.click(function() {
            rbtnHasNoPlan.attr("checked", "checked");
            rbtnHasPlan.removeAttr("checked");
            rbtnNEEDAPPROVAL.removeAttr("checked");
            rbtnALLOWME.removeAttr("checked");
        });
        rbtnALLOWME.click(function() {
            rbtnHasNoPlan.attr("checked", "checked");
            rbtnHasPlan.removeAttr("checked");
            rbtnNEEDAPPROVAL.removeAttr("checked");
            rbtnDONTNEEDAPPROVAL.removeAttr("checked");
        });

        jQuery("#tdOptions").find('input:radio').on('click', function() {
            if (jQuery(this).attr('id') == '<%=rbtnHasPlan.ClientID %>' && jQuery(this).prop('checked')) {
                $('#dvPlan').css('display', '');
            }
            else {
                $('#dvPlan').css('display', 'none');
            }
        });
        if (jQuery('#' + '<%=rbtnHasPlan.ClientID%>').prop('checked')) {
            jQuery('#dvPlan').css('display', '');
        }

        jQuery('#' + '<%=txtTotalHours.ClientID%>').on('change',
        function() {
            //记录原始的total在用户改了total以后算一个差值，然后算出remain
            $('#' + '<%=txtRemainHours.ClientID%>').val(
            parseFloat($('#' + '<%=txtTotalHours.ClientID%>').val())
            - parseFloat($('#' + '<%=hdTotalHours.ClientID%>').val()) + parseFloat($('#' + '<%=hdRemainHours.ClientID%>').val()));
        });
    });
    
</script>

