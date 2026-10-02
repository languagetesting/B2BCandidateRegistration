<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCand.Master" AutoEventWireup="true" CodeBehind="CreateRegistration.aspx.cs" Inherits="B2BCandidateRegistration.CreateRegistration" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
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
                <asp:Panel ID="pnlLogin" runat="server">
                    <div id="dvLogin" align="center">
                        <br />
                        <br />
                        <br />                        
                        <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width550px">
                            <tr>
                                <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;Create your Account</th>                                
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-padleft15px">
                                        <asp:Label ID="lblFirstName" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="First Name:" />
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" MaxLength="50"/>
                                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" CssClass="LTIvalidation-error" Display="None" ErrorMessage="First Name is required."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-padleft15px">
                                        <asp:Label ID="lblLastName" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="Last Name:" />
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" MaxLength="50"/>
                                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Last Name is required."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-padleft15px">
                                        <asp:Label ID="lblLoginID" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="Email ID:" MaxLength="100"/>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtLoginID" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" />
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtLoginID" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Please enter a valid email address." ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-padleft15px">
                                        <asp:Label ID="lblPwd" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="Password:" />
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" MaxLength="25" TextMode="Password" />
                                        <i id="togglePassword" class="fa fa-eye" onclick="registerPasswordToggle(this,'<%= txtPassword.ClientID %>');" title="Show Password"></i>
                                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Password is required."></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" ValidationExpression="^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).{12,25}$"
                                                    ErrorMessage="Password must be 12-25 characters and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character." Display="None" CssClass="LTIvalidation-error"/>
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
                                        <asp:CompareValidator ID="cvConfirmPassword" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" CssClass="LTIvalidation-error" Display="None" ErrorMessage="Passwords do not match." Operator="Equal" Type="String" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-textAlignCenter" colspan="2">
                                        <asp:Button ID="btnCreate" runat="server" CssClass="LTIformNewButton" meta:resourcekey="btnCreate" Text="Create" OnClientClick="ShowProgress('progloadingMsg');CheckValidationControl();" OnClick="btnCreate_Click" />
                                        <asp:Button ID="btnBack" runat="server" CssClass="LTIformNewButton" OnClick="btnBack_Click" Text="Back" OnClientClick="ShowProgress('progloadingMsg');" CausesValidation="false"/>
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
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />                    
                </asp:Panel>
                
                <asp:Panel ID="pnlOTP" runat="server" Visible="false">
                    <div id="dvOTP" align="center" style="margin-top:100px">                        
                        <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width550px">
                            <tr>
                                <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;OTP has been send to your Email Id, valid for next 10 mins</th>
                                <tr>
                                    <td colspan="2" class="LTIFormTable-textAlignCenter">
                                        <asp:Label ID="lblError_OTP" runat="server" CssClass="LTIformlabel" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-padleft15px">
                                        <asp:Label ID="lblOTP" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="Enter OTP:" />
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtOTP" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" TextMode="Number"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-textAlignCenter" colspan="2">
                                        <asp:Button ID="btnValidateOTP" runat="server" CssClass="LTIformNewButton" Text="Validate" OnClientClick="ShowProgress('progloadingMsg');" OnClick="btnValidateOTP_Click" CausesValidation="false"/>
                                        <asp:Button ID="btnResendOTP" runat="server" CssClass="LTIformNewButton" OnClick="btnResend_Click" Text="Resend OTP" OnClientClick="ShowProgress('progloadingMsg');" CausesValidation="false"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

