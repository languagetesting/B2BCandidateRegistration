<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCandDashboard.Master" AutoEventWireup="true" CodeBehind="TestRequest.aspx.cs" Inherits="B2BCandidateRegistration.TestRequest" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
    <script type="text/javascript" language="javascript" src="Scripts/WebForms/Login.js"></script>   
    <script type="text/javascript">
        document.addEventListener("click", function (e) {
            if (e.target.matches('input[type="date"]')) {
                if (typeof e.target.showPicker === "function") {
                    try {
                        e.target.showPicker();
                    } catch (err) {
                        // Browser may block showPicker in some situations
                    }
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="udpMain" runat="server"></asp:ScriptManager>    
    <asp:UpdatePanel ID="pnlMainPanel" runat="server">
        <ContentTemplate>
            <div class="LTIFormTable-formLayou-min-height575px LTIFormTable-formLayout-padtop50px" align="center">
                <div class="form-group">
                    <asp:Label ID="lblLabel1" runat="server" CssClass="LTIpagetitle" Text="Request Tests"></asp:Label>
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width80per">
                        <tr>
                            <th class="LTINewHeaderTitle" colspan="4">&nbsp;&nbsp;Enter Details for <asp:Label ID="lblTestName" runat="server"></asp:Label> </th>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-textAlignRight">
                                    <asp:Label ID="lblName" runat="server" CssClass="LTIformlabel" Text="Name:"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-textAlignRight">
                                    <asp:Label ID="lblLanguage" runat="server" CssClass="LTIformlabel" Text="Language:"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLanguage" runat="server" CausesValidation="false" CssClass="LTImodernformdropdown LTIformtextbox-300px">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="cvLanguage" runat="server" ControlToValidate="ddlLanguage" Display="None" ErrorMessage="" Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <!-- Client Custom Field Area -->
                            <tr>
                                <td colspan="4" style="padding-left: 20px; padding-right: 20px">
                                    <asp:Panel ID="pnlClientCustomFields" runat="server" Visible="false" Style="padding-left: 5px;">
                                        <fieldset style="direction: ltr;">
                                            <legend><b>Additional Information:</b></legend>
                                            <table class="LTIFormTable-width100per">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCFClient1" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea1" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCFClient2" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea2" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCFClient3" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea3" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCFClient4" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea4" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCFClient5" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea5" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCFClient6" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea6" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCFClient7" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea7" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCFClient8" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea8" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCFClient9" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea9" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCFClient10" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFClientArea10" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan1" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea1" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan2" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea2" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan3" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea3" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan4" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea4" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan5" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea5" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan6" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea6" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan7" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea7" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan8" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea8" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan9" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea9" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCfCan10" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="ctrlCFCanArea10" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <!-- Below is for Candidate Custom fields -->
                            <tr>
                                <td colspan="4" style="padding-left:20px; padding-right:20px">
                                    <asp:Panel ID="pnlCustomSection" runat="server" Visible="false" Style="padding-left: 5px;">
                                        <fieldset style="direction: ltr;"><legend><b>Additional Information:</b></legend>
                                        <table class="LTIFormTable-width100per">
                                            <tr>
                                                <asp:Panel ID="pnlcf1section" runat="server" Visible="false">
                                                    <asp:Panel ID="pnlcf1brk" runat="server" Visible="false">
                                                        <td class="tdRight">
                                                            <asp:Label ID="lblCand_Bot_CFClient1" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                        <td class="tdLeft">
                                                            <asp:Panel ID="ctrlCand_Bot_CFClientArea1" runat="server" Visible="false">
                                                            </asp:Panel>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;</td>
                                                    </asp:Panel>
                                                    <td class="tdRight">
                                                        <asp:Label ID="lblCand_Bot_CFClient2" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td class="tdLeft">
                                                        <asp:Panel ID="ctrlCand_Bot_CFClientArea2" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <asp:Panel ID="pnlcf2section" runat="server" Visible="false">
                                                    <asp:Panel ID="pnlcf3brk" runat="server" Visible="false">
                                                        <td class="tdRight">
                                                            <asp:Label ID="lblCand_Bot_CFClient3" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                        <td class="tdLeft">
                                                            <asp:Panel ID="ctrlCand_Bot_CFClientArea3" runat="server" Visible="false">
                                                            </asp:Panel>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;</td>
                                                    </asp:Panel>
                                                    <td class="tdRight">
                                                        <asp:Label ID="lblCand_Bot_CFClient4" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td class="tdLeft">
                                                        <asp:Panel ID="ctrlCand_Bot_CFClientArea4" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <asp:Panel ID="pnlcf3section" runat="server" Visible="false">
                                                    <asp:Panel ID="pnlcf5brk" runat="server" Visible="false">
                                                        <td class="tdRight">
                                                            <asp:Label ID="lblCand_Bot_CFClient5" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                        <td class="tdLeft">
                                                            <asp:Panel ID="ctrlCand_Bot_CFClientArea5" runat="server" Visible="false">
                                                            </asp:Panel>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;</td>
                                                    </asp:Panel>
                                                    <td class="tdRight">
                                                        <asp:Label ID="lblCand_Bot_CFClient6" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td class="tdLeft">
                                                        <asp:Panel ID="ctrlCand_Bot_CFClientArea6" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <asp:Panel ID="pnlcf4section" runat="server" Visible="false">
                                                    <asp:Panel ID="pnlcf7brk" runat="server" Visible="false">
                                                        <td class="tdRight">
                                                            <asp:Label ID="lblCand_Bot_CFClient7" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                        <td class="tdLeft">
                                                            <asp:Panel ID="ctrlCand_Bot_CFClientArea7" runat="server" Visible="false">
                                                            </asp:Panel>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;</td>
                                                    </asp:Panel>
                                                    <td class="tdRight">
                                                        <asp:Label ID="lblCand_Bot_CFClient8" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td class="tdLeft">
                                                        <asp:Panel ID="ctrlCand_Bot_CFClientArea8" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <asp:Panel ID="pnlcf5section" runat="server" Visible="false">
                                                    <asp:Panel ID="pnlcf9brk" runat="server" Visible="false">
                                                        <td class="tdRight">
                                                            <asp:Label ID="lblCand_Bot_CFClient9" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                        <td class="tdLeft">
                                                            <asp:Panel ID="ctrlCand_Bot_CFClientArea9" runat="server" Visible="false">
                                                            </asp:Panel>
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;</td>
                                                    </asp:Panel>
                                                    <td class="tdRight">
                                                        <asp:Label ID="lblCand_Bot_CFClient10" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td class="tdLeft">
                                                        <asp:Panel ID="ctrlCand_Bot_CFClientArea10" runat="server" Visible="false">
                                                        </asp:Panel>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                        </table>
                                        </fieldset>
                                    </asp:Panel>
                                    <!-- Test fields -->
                                    <asp:Panel ID="pnlTestFields" runat="server" Style="padding-left: 5px;" Visible="false">
                                        <fieldset style="direction: ltr;"><legend><b>Test Information:</b></legend>
                                        <asp:Panel ID="pnlOPIFields" runat="server" Visible="false">                                                                                        
                                            <table cellspacing="5">
                                                <tr>
                                                    <td colspan="6" class="LTIFormTable-textAlignCenter">
                                                        <div style="text-align: center; white-space: normal;">
                                                            <asp:Label ID="lblText" runat="server" CssClass="LTIformlabelstd" Text="Enter a First Choice and Second Choice date for testing. First Choice date must be AT LEAST 72 BUSINESS HOURS ahead of the order input date. Enter Time Ranges of at least 3 hours. The below steps are for OPI test schedule matching purposes only. The OPI Test Length is 30-45 minutes long."></asp:Label>                                                            
                                                        </div>
                                                                                                                
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap" style="vertical-align: top">
                                                        <asp:Label ID="lbl1stChoice" runat="server" CssClass="LTIformlabel" Text="First Choice:"></asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap" style="vertical-align: top">
                                                        <asp:TextBox ID="txtdtMatch" runat="server" Width="125px" TextMode="Date" />
                                                        <%--<span style="color: Silver; font-weight: bold; font-style: italic">(e.g. mm/dd/yyyy)</span><br />--%>
                                                        <%--<asp:CalendarExtender ID="defaultCalendarExtender" runat="server" CssClass="MyCalendar" TargetControlID="txtdtMatch" />
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="False" ClipboardEnabled="False" ErrorTooltipEnabled="true" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtdtMatch" UserDateFormat="MonthDayYear" UserTimeFormat="None" />--%>
                                                        <asp:CompareValidator ID="checkdtmatch" runat="server" ControlToValidate="txtdtMatch" ErrorMessage="Invalid Date" ForeColor="Red" Operator="GreaterThanEqual"></asp:CompareValidator>
                                                    </td>
                                                    <td nowrap="nowrap" style="vertical-align: top">Hr:
                                                        <asp:DropDownList ID="ddlHour" runat="server">
                                                            <asp:ListItem Selected="True" Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        Min:<asp:DropDownList ID="ddlMinutes" runat="server">
                                                            <asp:ListItem Selected="True" Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlAMPM" runat="server">
                                                            <asp:ListItem Selected="True" Text="AM" Value="AM"></asp:ListItem>
                                                            <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 10px"></td>
                                                    <td nowrap="nowrap" style="vertical-align: baseline">
                                                        <asp:Label ID="lbl1stChoiceTo" runat="server" Text="To:"></asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap" style="vertical-align: baseline">Hr:
                                                        <asp:DropDownList ID="ddlToHour" runat="server">
                                                            <asp:ListItem Selected="True" Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        Min:<asp:DropDownList ID="ddlToMinutes" runat="server">
                                                            <asp:ListItem Selected="True" Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlToAMPM" runat="server">
                                                            <asp:ListItem Selected="True" Text="AM" Value="AM"></asp:ListItem>
                                                            <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap" style="vertical-align: top">
                                                        <asp:Label ID="Label11" runat="server" CssClass="LTIformlabel" Text="Second Choice:"></asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap" style="vertical-align: top">
                                                        <asp:TextBox ID="txtdtMatch2" runat="server" Width="125px" TextMode="Date" />
                                                        <%--<span style="color: Silver; font-weight: bold; font-style: italic">(e.g. mm/dd/yyyy)</span><br />--%>
                                                        <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="MyCalendar" TargetControlID="txtdtMatch2" />
                                                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="False" ClipboardEnabled="False" ErrorTooltipEnabled="true" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtdtMatch2" UserDateFormat="MonthDayYear" UserTimeFormat="None" />--%>
                                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtdtMatch2" ErrorMessage="Invalid Date" ForeColor="Red" Operator="GreaterThanEqual"></asp:CompareValidator>
                                                    </td>
                                                    <td nowrap="nowrap" style="vertical-align: top">Hr:
                                                        <asp:DropDownList ID="ddlHour2" runat="server">
                                                            <asp:ListItem Selected="True" Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        Min:<asp:DropDownList ID="ddlMinutes2" runat="server">
                                                            <asp:ListItem Selected="True" Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlAMPM2" runat="server">
                                                            <asp:ListItem Selected="True" Text="AM" Value="AM"></asp:ListItem>
                                                            <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 10px"></td>
                                                    <td nowrap="nowrap" style="vertical-align: baseline">
                                                        <asp:Label ID="Label12" runat="server" Text="To:"></asp:Label>
                                                    </td>
                                                    <td nowrap="nowrap" style="vertical-align: baseline">Hr:
                                                        <asp:DropDownList ID="ddlToHour2" runat="server">
                                                            <asp:ListItem Selected="True" Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        Min:<asp:DropDownList ID="ddlToMinutes2" runat="server">
                                                            <asp:ListItem Selected="True" Text="00" Value="00"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlToAMPM2" runat="server">
                                                            <asp:ListItem Selected="True" Text="AM" Value="AM"></asp:ListItem>
                                                            <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6" class="LTIFormTable-textAlignCenter">
                                                        <br />
                                                        <asp:Label ID="Label1" runat="server" CssClass="LTIformlabelstd" Text="If you are in AZ, AK or any other state or region that does not adjust time for Daylight Saving Time you may be in a later time zone."></asp:Label>                                                                                                                    
                                                    </td>
                                                </tr>
                                            </table>                                            
                                        </asp:Panel>
                                        <asp:Table ID="tstTable" runat="server" class="LTIFormTable-width100per">
                                        </asp:Table>
                                        </fieldset>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-textAlignCenter" colspan="4">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="LTIformNewButton" OnClick="btnSubmit_Click" OnClientClick="ShowProgress('progloadingMsg');CheckValidationControl();" Text="Submit" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-textAlignCenter" colspan="4">
                                    <asp:Label ID="lblError" runat="server" CssClass="LTIformlabel" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="hfCFChildDataValue" runat="server" Value=""  />
            <asp:HiddenField ID="hfCFChildDataValue1" runat="server" Value=""  />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
