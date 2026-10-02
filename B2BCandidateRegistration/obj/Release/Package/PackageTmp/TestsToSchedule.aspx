<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCandDashboard.Master" AutoEventWireup="true" CodeBehind="TestsToSchedule.aspx.cs" Inherits="B2BCandidateRegistration.TestsToSchedule" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
    <script language="javascript" type="text/javascript">
        //confirm test cancellation
        function showCancelConfirm(aid) {
            document.getElementById('<%= lblStatus.ClientID %>').innerHTML = "";
            document.getElementById('<%= hfCancelAptID.ClientID  %>').value = aid;            
            document.getElementById('<%= lblCancelMessage.ClientID  %>').innerHTML = "Are you sure you want to Cancel this Test?";
            $find("modelPopupExtender1").show();
        }

        function HideCancelWindow() {
            $find("modelPopupExtender1").hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="udpMain" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnlMainPanel" runat="server">
        <ContentTemplate>
            <div class="LTIFormTable-formLayou-min-height575px LTIFormTable-formLayout-padtop50px" align="center">                
                <div class="form-group">
                    <asp:Label ID="lblTitle" runat="server" CssClass="LTIpagetitle" Text="Tests to be Scheduled"></asp:Label>
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width100per">                        
                        <tr>
                            <td>
                                <asp:GridView ID="gvTestsToSchedule" runat="server" CellPadding="0" onrowdatabound="gvTestsToSchedule_RowDataBound"
                                    GridLines="None" AutoGenerateColumns="False" Width="100%" DataKeyNames="AppointmentID" PageSize="10"
                                    AllowPaging="True" OnPageIndexChanging="gvTestsToSchedule_PageIndexChanging" CssClass="LTImodern-grid">                                    
                                    <Columns>                                        
                                        <asp:BoundField HeaderText="Test" DataField="ClientTestName" HeaderStyle-HorizontalAlign="left">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Language" DataField="LanguageName" HeaderStyle-HorizontalAlign="left">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Order Date" DataField="OrderDate" HeaderStyle-HorizontalAlign="left">                                        
                                            <ItemStyle Width="150px" />
                                        </asp:BoundField>                                                                                
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="240px">
                                            <ItemTemplate>
                                                <span style="display:inline-block; width:120px;">
                                                    <asp:Button runat="server" ID="btnSchedule" Text="Schedule" CssClass="LTIformNewButton" ToolTip="Schedule Test." onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" OnClick="btnSchedule_Click" OnClientClick="ShowProgress('progloadingMsg');"/>
                                                </span>
                                                <span style="display:inline-block; width:120px;">
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="LTIformNewButton" ToolTip="Allows you to Cancel your Test." onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" 
                                                            OnClientClick='<%# "showCancelConfirm(" + Eval("AppointmentID") + "); return false;" %>' CausesValidation="false"/>			
                                                </span>
                                                <asp:HiddenField ID="hdnProductType" Value='<%#Bind("ProductType")%>' runat="server" />
                                                <asp:HiddenField ID="hdnProctorType" Value='<%#Bind("ProctorType")%>' runat="server" />
                                                <asp:HiddenField ID="hdnCand_HashValue" Value='<%#Bind("Cand_HashValue")%>' runat="server" />
                                                <asp:HiddenField ID="hdnTestSiteCenter" Value='<%#Bind("TestSiteCenter")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>                                    
                                    <PagerStyle CssClass="LTImodern-pager" />                                    
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; </td>
                        </tr>                        
                        <tr>
                            <td class="LTIFormTable-textAlignCenter">
                                <asp:Label ID="lblError" runat="server" CssClass="LTIformlabel" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <asp:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="modelPopupExtender1" runat="server" TargetControlID="btnDummy" PopupControlID="xmlPopup" BackgroundCssClass="LTImodalBackground" />
            <div id="xmlPopup" runat="server" style="display: none; width: 500px; z-index: 2; font-size: 13px; background-color: #FFFFFF; border: 1px solid #d9dde3; border-radius: 14px; font-family: Verdana, Arial, sans-serif;
                box-shadow: 0 20px 45px rgba(0, 0, 0, .20), 0 6px 15px rgba(0, 0, 0, .10); overflow: hidden; box-sizing: border-box;">
                <div class="LTIPopUpDivider">
                    <div id="btnCloseParent" class="LTIPopUpCloseButtonContainer">
                        <asp:LinkButton ID="btnClose" runat="server" Text="X" ToolTip="Close" CssClass="LTIPopUpCloseButton" OnClientClick="HideCancelWindow();return false;" />
                    </div>
                </div>
                <div id="div" runat="server" align="center">
                    <img align="absmiddle" src="Images/warning.jpg" />
                    <asp:Label ID="lblCancelMessage" runat="server" />
                    <br />
                    <br />
                    <asp:Button ID="btnOk" runat="server" CssClass="LTIformNewButton" onMouseOver="this.style.cursor='pointer'" onMouseOut="this.style.cursor='default'" Text="Yes" Width="50px" CausesValidation="false" OnClientClick="ShowProgress('progloadingMsg');" OnClick="btnOk_Click" />
                    <asp:Button ID="btnNo" runat="server" CssClass="LTIformNewButton" onMouseOver="this.style.cursor='pointer'" onMouseOut="this.style.cursor='default'" Text="No" Width="50px" CausesValidation="false" OnClientClick="HideCancelWindow(); return false;" />
                    <br />
                    <br />
                    <asp:Label ID="lblStatus" CssClass="blueFont" runat="server" />
                    <asp:HiddenField ID="hfCancelAptID" runat="server" Value='' />
                </div>
            </div>

            <asp:HiddenField ID="btnDummy" runat="server"/>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

