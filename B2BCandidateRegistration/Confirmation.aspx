<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCandDashboard.Master" AutoEventWireup="true" CodeBehind="Confirmation.aspx.cs" Inherits="B2BCandidateRegistration.Confirmation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="udpMain" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnlMainPanel" runat="server">
        <ContentTemplate>
            <div class="LTIFormTable-formLayou-min-height575px LTIFormTable-formLayout-padtop50px" align="center">                
                <div class="form-group">
                    <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width80per">
                        <th class="LTINewHeaderTitle" colspan="2">&nbsp;&nbsp;Confirmation</th>
                        <tr>
                            <td>&nbsp; </td>
                        </tr>
                        <tr>
                            <td class="LTIFormTable-padleft15px">
                                <div>
                                    Thank you for Requesting <%=testname %>                                    
                                </div>
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

