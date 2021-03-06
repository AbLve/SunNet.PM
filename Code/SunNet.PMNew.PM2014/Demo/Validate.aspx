﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Validate.aspx.cs" Inherits="SunNet.PMNew.PM2014.Demo.Validate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        jQuery(function () {
            
            // validate signup form on keyup and submit
            $("form").validate({
                rules: {
                    firstname: "required",
                    lastname: "required",
                    username: {
                        required: true,
                        minlength: 2
                    },
                    password: {
                        required: true,
                        minlength: 5
                    },
                    confirm_password: {
                        required: true,
                        minlength: 5,
                        equalTo: "#password"
                    },
                    email: {
                        required: true,
                        email: true
                    },
                    topic: {
                        required: "#newsletter:checked",
                        minlength: 2
                    },
                    agree: "required"
                },
                messages: {
                    firstname: "Please enter your firstname",
                    lastname: "Please enter your lastname",
                    username: {
                        required: "Please enter a username",
                        minlength: "Your username must consist of at least 2 characters"
                    },
                    password: {
                        required: "Please provide a password",
                        minlength: "Your password must be at least 5 characters long"
                    },
                    confirm_password: {
                        required: "Please provide a password",
                        minlength: "Your password must be at least 5 characters long",
                        equalTo: "Please enter the same password as above"
                    },
                    email: "Please enter a valid email address",
                    agree: "Please accept our policy"
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server"> 
	<fieldset>
		<legend>Validating a complete form</legend>
		<p>
			<label for="firstname">Firstname</label>
			<input id="firstname" name="firstname" type="text" />
		</p>
		<p>
			<label for="lastname">Lastname</label>
			<input id="lastname" name="lastname" type="text" />
		</p>
		<p>
			<label for="username">Username</label>
			<input id="username" name="username" type="text" />
		</p>
		<p>
			<label for="password">Password</label>
			<input id="password" name="password" type="password" />
		</p>
		<p>
			<label for="confirm_password">Confirm password</label>
			<input id="confirm_password" name="confirm_password" type="password" />
		</p>
		<p>
			<label for="email">Email</label>
			<input id="email" name="email" type="email" />
		</p>
		<p>
			<label for="agree">Please agree to our policy</label>
			<input type="checkbox" class="checkbox" id="agree" name="agree" />
		</p>
		<p>
			<label for="newsletter">I'd like to receive the newsletter</label>
			<input type="checkbox" class="checkbox" id="newsletter" name="newsletter" />
		</p>
		<fieldset id="newsletter_topics">
			<legend>Topics (select at least two) - note: would be hidden when newsletter isn't selected, but is visible here for the demo</legend>
			<label for="topic_marketflash">
				<input type="checkbox" id="topic_marketflash" value="marketflash" name="topic" />
				Marketflash
			</label>
			<label for="topic_fuzz">
				<input type="checkbox" id="topic_fuzz" value="fuzz" name="topic" />
				Latest fuzz
			</label>
			<label for="topic_digester">
				<input type="checkbox" id="topic_digester" value="digester" name="topic" />
				Mailing list digester
			</label>
			<label for="topic" class="error">Please select at least two topics you'd like to receive.</label>
		</fieldset>
		<p>
			<input class="submit" type="submit" value="Submit"/>
		</p>
	</fieldset> 
</asp:Content>
