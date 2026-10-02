<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCandDashboard.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="B2BCandidateRegistration.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
    <script type="text/javascript" language="javascript" src="Scripts/WebForms/Login.js"></script>
    <link href="Content/all.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="udpMain" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnlUpdPanel" runat="server">
        <ContentTemplate>
            <div class="LTIFormTable-formLayou-min-height575px">
                <br />
                <div id="dvChangePassword" align="center">
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width550px">
                        <tr>
                            <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;Change Password</th>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-padleft15px">
                                    <asp:Label ID="lblPwd" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="Current Password:" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" MaxLength="25" TextMode="Password" />
                                    <i id="togglePassword" class="fa fa-eye" onclick="registerPasswordToggle(this,'<%= txtPassword.ClientID %>');" title="Show Password"></i>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Password is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-padleft15px">
                                    <asp:Label ID="lblNewPwd" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="New Password:" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" MaxLength="25" TextMode="Password" />
                                    <i id="newtogglePassword" class="fa fa-eye" onclick="registerPasswordToggle(this,'<%= txtNewPassword.ClientID %>');" title="Show Password"></i>
                                    <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Password is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-padleft15px">
                                    <asp:Label ID="lblReConfirmPwd" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="Confirm password:" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" MaxLength="25" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Please re-enter your password." />
                                    <asp:CompareValidator ID="cvConfirmPassword" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Passwords do not match." Operator="Equal" Type="String" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-textAlignCenter" colspan="2">
                                    <asp:Button ID="btnChangepw" runat="server" CssClass="LTIformNewButton" Text="Submit" OnClientClick="ShowProgress('progLoginMsg');CheckValidationControl();" OnClick="btnChangepw_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-textAlignCenter" colspan="2">
                                    <asp:Label ID="lblError" runat="server" CssClass="LTIformlabel LTIvalidation-error"></asp:Label>
                                </td>
                            </tr>
                        </tr>
                    </table>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
