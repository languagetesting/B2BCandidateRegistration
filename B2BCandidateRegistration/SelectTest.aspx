<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCandDashboard.Master" AutoEventWireup="true" CodeBehind="SelectTest.aspx.cs" Inherits="B2BCandidateRegistration.SelectTest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
    <script type="text/javascript" language="javascript" src="Scripts/WebForms/Login.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="udpMain" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnlMainPanel" runat="server">
        <ContentTemplate>
            <div class="LTIFormTable-formLayou-min-height575px LTIFormTable-formLayout-padtop50px" align="center">                
                <div class="form-group">
                    <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width50per">
                        <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;Select Test</th>
                        <tr>
                            <td>&nbsp; </td>
                        </tr>                        
                        <tr>
                            <td colspan="2">
                                <div align="center">
                                    <asp:radiobuttonlist id="rbgSelectTest" runat="server" RepeatDirection="Vertical" CssClass="LTImodernradio-list">
                                    </asp:radiobuttonlist>
                                    <asp:RequiredFieldValidator ID="rvSelectTest" runat="server" ErrorMessage="Please select a test" ControlToValidate="rbgSelectTest" CssClass="LTIvalidation-error"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; </td>
                        </tr>
                        <tr>
                            <td class="LTIFormTable-textAlignCenter" colspan="2">
                                <asp:Button ID="btnContinue" runat="server" CssClass="LTIformNewButton" Text="Continue" OnClientClick="ShowProgress('progloadingMsg');CheckValidationControl();" OnClick="btnContinue_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="LTIFormTable-textAlignCenter">
                                <asp:Label ID="lblError" runat="server" CssClass="LTIformlabel" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
