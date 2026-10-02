<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCand.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="B2BCandidateRegistration.Login" %>
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
                        <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width550px">
                            <tr>
                                <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;Login or Create an Account</th>                                
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-padleft15px" colspan="2">
                                        <asp:Label ID="lblCreateAccount" runat="server" CssClass="LTIformlabel" Text="Please create an account or login to continue." />
                                        <br />
                                        <asp:LinkButton ID="lnkCreateAccount" runat="server" CssClass="LTIformLinkButton" OnClick="lnkCreateAccount_Click" Text="Create an Account" CausesValidation="false"></asp:LinkButton>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-padleft15px" colspan="2">
                                        <asp:Label ID="lblExistingUser" runat="server" CssClass="LTIformlabel" Text="If you have an account, sign in with your email address." />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-padleft15px">
                                        <asp:Label ID="lblLoginID" runat="server" CssClass="LTIformlabel LTIformlabel-100px" Text="Email ID:" />
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtLoginID" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" />
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtLoginID" Display="None" ErrorMessage="Please enter a valid email address." CssClass="LTIvalidation-error" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"/>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-textAlignRight LTIFormTable-padright25px" colspan="2">
                                        <asp:LinkButton ID="lnkForgotPassword" runat="server" CssClass="LTIformLinkButton LTIformLinkButton-Size11px" OnClick="lnkForgotPassword_Click" Text="Forgot Password" CausesValidation="false"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-textAlignCenter" colspan="2">
                                        <asp:Button ID="btnLogin" runat="server" CssClass="LTIformNewButton" meta:resourcekey="btnLogin" Text="Login" OnClientClick="ShowProgress('progLoginMsg');CheckValidationControl();" OnClick="btnLogin_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td class="LTIFormTable-textAlignCenter" colspan="2">
                                        <asp:Label ID="lblError" runat="server" CssClass="LTIformlabel LTIvalidation-error" ></asp:Label>
                                    </td>
                                </tr>
                            </tr>
                        </table>
                    </div>                                        
                </asp:Panel>                

                <asp:Panel ID="pnlOTP" runat="server" Visible="false">
                    <div id="dvOTP" align="center" style="margin-top:100px">                        
                        <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width550px">
                            <tr>
                                <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;OTP has been send to your Email Id, valid for next 10 mins</th>
                                <tr>
                                    <td colspan="2" style="text-align: center">
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
                                        <asp:TextBox ID="txtOTP" runat="server" CssClass="LTIformtextbox LTIformtextbox-300px" TextMode="Number" />
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

