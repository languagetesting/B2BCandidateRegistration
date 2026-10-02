<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCandDashboard.Master" AutoEventWireup="true" CodeBehind="Enroll.aspx.cs" Inherits="B2BCandidateRegistration.Enroll" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="udpMain" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnlMainPanel" runat="server">
        <ContentTemplate>
            <div class="LTIFormTable-formLayou-min-height575px LTIFormTable-formLayout-padtop50px" align="center">                
                <div class="form-group">
                    <asp:Label ID="lblLabel1" runat="server" CssClass="LTIpagetitle" Text="Enrollment of your Tests"></asp:Label>
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width50per">
                        <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;Enrollment Code</th>
                        <tr>
                            <td>&nbsp; </td>
                        </tr>
                        <tr>
                            <td class="LTIFormTable-padleft15px">
                                <asp:Label ID="lblTitle" runat="server" Text="Enter the enrollment code provided by the Institute" CssClass="LTIformlabel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; </td>
                        </tr>
                        <tr>
                            <td class="LTIFormTable-padleft15px">
                                <asp:TextBox ID="txtCode" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Assessment Code."></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; </td>
                        </tr>
                        <tr>
                            <td class="LTIFormTable-textAlignCenter" colspan="2">
                                <asp:Button ID="btnEnroll" runat="server" CssClass="LTIformNewButton" Text="Continue" OnClientClick="ShowProgress('progloadingMsg');CheckValidationControl();" OnClick="btnEnroll_Click" />
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
