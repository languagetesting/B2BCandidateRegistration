<%@ Page Title="" Language="C#" MasterPageFile="~/B2BCandDashboard.Master" AutoEventWireup="true" CodeBehind="ScheduledTests.aspx.cs" Inherits="B2BCandidateRegistration.ScheduledTests" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
    <script type="text/javascript">
        function HideInstructionWindow() {
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
                    <asp:Label ID="lblTitle" runat="server" CssClass="LTIpagetitle" Text="Tests Scheduled"></asp:Label>
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="LTIformtable LTIFormTable-width100per">                        
                        <tr>
                            <td>
                                <asp:GridView ID="gvTestsSchedule" runat="server" CellPadding="0" onrowdatabound="gvTestsSchedule_RowDataBound"
                                    GridLines="None" AutoGenerateColumns="False" Width="100%" DataKeyNames="AppointmentID" PageSize="10"
                                    AllowPaging="True" OnPageIndexChanging="gvTestsSchedule_PageIndexChanging" CssClass="LTImodern-grid">                                    
                                    <Columns>                                        
                                        <asp:BoundField HeaderText="Test" DataField="ClientTestName" HeaderStyle-HorizontalAlign="left">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Language" DataField="LanguageName" HeaderStyle-HorizontalAlign="left">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Scheduled Date/Time" DataField="TestScheduledDate" HeaderStyle-HorizontalAlign="left">                                        
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Time Zone" DataField="TimeZoneCode" HeaderStyle-HorizontalAlign="left">                                        
                                            <ItemStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="160px">
                                            <ItemTemplate>
                                                <span style="display:inline-block; width:160px;">
                                                    <asp:Button runat="server" ID="btnSchedule" Text="Start Test / Reschedule" CssClass="LTIformNewButton" ToolTip="Start Test / Reschedule your Test." onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'" 
                                                            OnClick="btnSchedule_Click" OnClientClick="ShowProgress('progloadingMsg');"/>
                                                    <asp:Button runat="server" ID="btnInstructions" Text="View Instruction" CssClass="LTIformNewButton" ToolTip="View Instruction for the Test." onmouseover="this.style.cursor='pointer'" onmouseout="this.style.cursor='default'"
                                                            OnClick="btnInstructions_Click" />
                                                </span>                                                
                                                <asp:HiddenField ID="hdnProductType" Value='<%#Bind("ProductType")%>' runat="server" />
                                                <asp:HiddenField ID="hdnProctorType" Value='<%#Bind("ProctorType")%>' runat="server" />
                                                <asp:HiddenField ID="hdnCand_HashValue" Value='<%#Bind("Cand_HashValue")%>' runat="server" />
                                                <asp:HiddenField ID="hdnTestSiteCenter" Value='<%#Bind("TestSiteCenter")%>' runat="server" />
                                                <asp:HiddenField ID="hdnRatingCertification" Value='<%#Bind("RatingCertification")%>' runat="server" />
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
            <div id="xmlPopup" runat="server" style="display: none; width: 750px; z-index: 2; font-size: 13px; background-color: #FFFFFF; border: 1px solid #d9dde3; border-radius: 14px; font-family: Verdana, Arial, sans-serif;
                box-shadow: 0 20px 45px rgba(0, 0, 0, .20), 0 6px 15px rgba(0, 0, 0, .10); overflow: hidden; box-sizing: border-box;">
                <div class="LTIPopUpDivider">
                    <div id="btnCloseParent" class="LTIPopUpCloseButtonContainer">
                        <asp:LinkButton ID="btnClose" runat="server" Text="X" ToolTip="Close" CssClass="LTIPopUpCloseButton" OnClientClick="HideInstructionWindow();return false;" />
                    </div>
                </div>
                <div id="div" runat="server" style="padding-left:10px; padding-right:10px">                    
                    <p style="text-align:center; font-weight:600; font-size:medium">
                        Instructions for your Language Test
                    </p>
                    <asp:Label ID="lblInstruction" runat="server" />
                    <br />                    
                    <p style="text-align:center">
                        <asp:Button ID="btnNo" runat="server" CssClass="LTIformNewButton" onMouseOver="this.style.cursor='pointer'" onMouseOut="this.style.cursor='default'" Text="Close" CausesValidation="false" OnClientClick="HideInstructionWindow(); return false;" />                    
                    </p>
                </div>
            </div>

            <asp:HiddenField ID="btnDummy" runat="server"/>

        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>


