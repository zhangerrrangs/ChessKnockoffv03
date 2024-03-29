﻿<%@ Page Title="Reset" Language="C#" MasterPageFile="~/BaseWithHeaderNav.master" AutoEventWireup="true" CodeBehind="Reset.aspx.cs" Inherits="ChessKnockoff.ResetForm" %>
<asp:Content ContentPlaceHolderID="BaseContentWithHeaderNavTitle" runat="server">
    Reset
</asp:Content>

<asp:Content ContentPlaceHolderID="BaseContentWithHeaderNav" runat="server">
    <div class="inputForm mx-auto">
        <div class="form-group">
            <label for="inpPassword">New password</label>
            <asp:CustomValidator ID="valPassword" runat="server" ControlToValidate="inpPassword" ClientValidationFunction="wrappedPassword" Display="None" ValidationGroup="grpReset" ValidateEmptyText="True" OnServerValidate="checkPassword"></asp:CustomValidator>
            <input id="inpPassword" required="" type="password" class="form-control" placeholder="Password" runat="server"/>
        </div>
        <div class="form-group">
            <asp:CustomValidator ID="valRePassword" runat="server" ControlToValidate="inpRePassword" ClientValidationFunction="wrappedPassword" Display="None" ValidationGroup="grpReset" ValidateEmptyText="True" OnServerValidate="checkPassword"></asp:CustomValidator>
            <input id="inpRePassword" required="" type="password" class="form-control" placeholder="Re-enter password" runat="server"/>
            <div class="invalid-feedback">Passwords do not match.</div>
        </div>
        <div class="form-group">
            <asp:Button id="btnSubmitReset" class="btn btn-primary btn-block" type="submit" runat="server" Text="Change password" ValidationGroup="grpReset" OnClick="ResetPassword" />
        </div>
        <div id="altError" class="alert alert-danger" role="alert" runat="server">
        </div>
    </div>
</asp:Content>