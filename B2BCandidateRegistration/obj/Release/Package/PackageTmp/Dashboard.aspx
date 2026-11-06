<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCandDashboard.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="B2BCandidateRegistration.Dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
    <link href="Content/all.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="udpMain" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnlUpdPanel" runat="server">
        <ContentTemplate>
            <div class="LTIFormTable-formLayou-min-height575px">
                <br />
                <div id="dvMain" align="center">
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width550px">
                        <tr>
                            <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;Choose an Option</th>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-padleft15px" colspan="2">
                                    <img src="Images/Enrollment.png" alt="" title=""/>
                                    <asp:Label ID="lblEnrollment" runat="server" CssClass="LTIformlabel" Text="" />                                    
                                    <asp:LinkButton ID="lnkEnrollment" runat="server" CssClass="LTIformLinkButton" OnClientClick="ShowProgress('progloadingMsg');" OnClick="lnkEnrollment_Click" Text="Request New Test(s)" CausesValidation="false"></asp:LinkButton>
                                    <br />
                                    <br />
                                </td>
                            </tr>                            
                            <tr>
                                <td class="LTIFormTable-padleft15px" colspan="2">       
                                    <img src="Images/ToSchedule.png" alt="" title=""/>
                                    <asp:Label ID="lblTesttoSchedule" runat="server" CssClass="LTIformlabel" Text="Tests to be Scheduled:" />                                
                                    <asp:LinkButton ID="lnkTesttoSchedule" runat="server" CssClass="LTIformLinkButton" OnClientClick="ShowProgress('progloadingMsg');" OnClick="lnkTesttoSchedule_Click" CausesValidation="false"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; </td>
                            </tr>
                            <tr>
                                <td class="LTIFormTable-padleft15px" colspan="2">
                                    <img src="Images/Scheduled.png" alt="" title=""/>
                                    <asp:Label ID="lblTestScheduled" runat="server" CssClass="LTIformlabel" Text="Tests Scheduled:" />                                
                                    <asp:LinkButton ID="lnkTestScheduled" runat="server" CssClass="LTIformLinkButton" OnClientClick="ShowProgress('progloadingMsg');" OnClick="lnkTestScheduled_Click" CausesValidation="false"></asp:LinkButton>
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
                    <br />
                    <br />
					Note: Completed tests will not appear on this site. Your test results are sent to the organization through which the test is requested.
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
