<%@ Page Title="Faqs" Language="C#" MasterPageFile="~/NoLeftmenu.master" AutoEventWireup="true"
    CodeBehind="Faqs.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.Faqs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contactTitle">FAQ</div>
    <table border="0" cellspacing="0" cellpadding="5" width="100%">
        <tbody>
            <tr>
                <td width="100%" class="faqsBottom">
                    <div class="faqsTitle">
                        1. How can I create a new user?
                    </div>
                    <div class="faqsText" id="div1">
                        To create a new user, please visit <a href="http://client.sunnet.us">http://client.sunnet.us</a>
                        and select the ‘Create a new account’ option located under the Login button.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        2. How do I submit a bug?
                    </div>
                    <div class="faqsText" id="div2">
                        To enter a bug, select the ‘Submit a Bug’ option and fill in all applicable fields.
                                Next, select your project name and press ‘Submit.’ Once submitted you’ll be taken
                                to the ‘View Ticket Progress’ page where you will be able to track your ticket’s
                                status until completion.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        3. How do I submit a new request?
                    </div>
                    <div class="faqsText" id="div3">
                        To enter a new request, select the ‘Submit a Request’ option and fill in all applicable
                                fields. Next, select your project name and press ‘Submit.’ Once submitted you’ll
                                be taken to the ‘View Ticket Progress’ page where you will be able to track your
                                ticket’s status until completion.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        4. I have multiple tickets entered. How can I sort through the tickets?
                    </div>
                    <div class="faqsText" id="div4">
                        Yes, you can easily sort through tickets by clicking on the corresponding title.
                                You can sort by ‘Project, Type, Ticket Code, Date created or updated, Status, Priority,
                                Created by and Action.’
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        5. How will I know when there are updates to my ticket?
                    </div>
                    <div class="faqsText" id="div5">
                        Each time your ticket is updated, a green icon
                                <img src="images/ticket_status.gif" width="16" height="16">
                        will appear next to your ticket in the client portal. You will also receive an automatic
                                email informing you about what was updated on your ticket. The email received will
                                be similar to the text below:<br>
                        <p class="faqsspantext">
                            Your Ticket R324 "<strong>Refresh my Server</strong>" has been updated.
                                    <br>
                            "Your server has been refreshed, please confirm."<br>
                            For more information, log into the client portal. <a href="http://client.sunnet.us">http://client.sunnet.us</a>
                        </p>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        6. What does the ticket status mean?
                    </div>
                    <div class="faqsText" id="div7">
                        <p>
                            <strong class="faqsspantext1">Draft </strong>- <u>Draft</u> stands for that you
                                    have just write a draft, it has not entered into the system.
                        </p>
                        <p>
                            <strong class="faqsspantext1">Submitted </strong>- <u>Submitted tickets</u> have
                                    been successfully entered into the system, but have not yet been reviewed by your
                                    Project Manager.
                        </p>
                        <p>
                            <strong class="faqsspantext1">In progress </strong>- <u>In Progress tickets</u>
                            have been reviewed by your Project Manager and are currently under review or in
                                    the development stage.
                        </p>
                        <p>
                            <strong class="faqsspantext1">Estimating </strong>- <u>Estimating</u> tickets are
                                    estimating.
                        </p>
                        <p>
                            <strong class="faqsspantext1">Denied </strong>- <u>Denied</u>
                            tickets were denied by saler.
                        </p>
                        <p>
                            <strong class="faqsspantext1">Waiting Feedback </strong>- <u>Waiting Feedback</u>
                            tickets need your review and feedback before we can proceed.
                        </p>
                        <p>
                            <strong class="faqsspantext1">Waiting Verify </strong>- <u>Waiting Verify</u> tickets
                                    have already been published to your production/live site and need your review and
                                    approval.
                        </p>
                        <p>
                            <strong class="faqsspantext1">Not Approved </strong>- <u>Not Approved</u> ticket
                                    has been released on product server, but you didn't approve.
                        </p>
                        <p>
                            <strong class="faqsspantext1">Completed </strong>- <u>Completed</u> tickets have
                                    been approved and published to your production/live site. These tickets are considered
                                    closed.
                        </p>
                        <p>
                            <strong class="faqsspantext1">Cancelled</strong> - <u>Cancelled</u> tickets are
                                    tickets that are no longer being worked on but continue to be stored in the client
                                    portal.
                        </p>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        7. Can I cancel tickets once they’ve been entered into the portal?
                    </div>
                    <div class="faqsText" id="div8">
                        If you need to cancel your ticket, simply select the ‘Cancel’ button located within
                                your ticket. Please keep in mind that once your ticket is cancelled, you will not
                                be able to access it again.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        8. Can I cancel once my ticket is ‘In Progress?’
                    </div>
                    <div class="faqsText" id="div9">
                        Yes, you can still cancel you ticket once it’s in progress but please note that
                                you will still be financially responsible for what has already been worked on.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        9. Why do I need an emergency contact?
                    </div>
                    <div class="faqsText" id="div10">
                        We suggest including an emergency contact in case we need to contact your regarding
                                an urgent ticket matter and you are unavailable.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        10. What is an IT consulting agreement?
                    </div>
                    <div class="faqsText" id="div11">
                        Our IT consulting agreement grants us permission to work on your requests and guarantees
                                payment.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        11. How do I know if my company has a maintenance plan?
                    </div>
                    <div class="faqsText" id="div12">
                        <ul>
                            <li>If you are unsure whether or not your company has a maintenance plan, please select
                                        ‘Does not have maintenance plan’ and we will correct it once we’ve verified.</li>
                            <li>Selecting ‘Has a maintenance plan’ lets us know to use your available maintenance
                                        hours </li>
                            <li>Selecting ‘Does not have maintenance plan’ lets us know that we must invoice all
                                        tickets.</li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        12. How do I know if my company requires a quote approval?
                    </div>
                    <div class="faqsText" id="div13">
                        <ul>
                            <li>Selecting ‘Needs a quote approval’ requires you to approve any ticket submitted
                                        on the client portal before we can proceed. Once approved, we will begin working
                                        on your ticket</li>
                            <li>Selecting ‘Does not need a quote approval’ allows us to immediately
                                            work on all tickets as soon as they are submitted within the client portal</li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        13. Why do I need to use the client portal to submit my requests?
                    </div>
                    <div class="faqsText" id="div14">
                        By entering all your requests &amp; bugs on the client portal, you will be able
                                to easily and immediately track the status of all your submitted requests in real
                                time.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        14. Why do I need to assign a priority to my requests?
                    </div>
                    <div class="faqsText" id="div15">
                        Assigning a priority is essential when submitting multiple tickets. Selecting a
                                priority will help us sort out urgent tickets so that we may work on them immediately.
                                Lower priority tickets will be worked on after urgent tickets are completed.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        15. How will I know the status of my request?
                    </div>
                    <div class="faqsText" id="div16">
                        You can track the status of your request in real time by going to the ‘View Ticket
                                Progress’ page and locating the status column.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        16. How can I change my password?
                    </div>
                    <div class="faqsText" id="div17">
                        To change your password, simply go to ‘My Account’ and select ‘Change Password.’
                                You will be prompted to input a new password
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        17. What is the Survey option?
                    </div>
                    <div class="faqsText" id="div18">
                        The survey option is an opportunity for you to let us know how we are doing. The
                                survey consists of 10 short questions that will be sent directly to the President
                                of the company.
                    </div>
                </td>
            </tr>
            <tr>
                <td class="faqsBottom">
                    <div class="faqsTitle">
                        18. Who can I contact if I have an emergency request?
                    </div>
                    <div class="faqsText" id="div19">
                        For all technical emergencies during business hours, please contact your Project
                                Manager directly.
                    </div>
                </td>
            </tr>
        </tbody>
    </table>






    <script type="text/javascript">
        jQuery(function () {
            $("div.faqsText").toggle();
            jQuery("div.faqsTitle").click(
            function () {
                jQuery(this).next().slideToggle("fast");
            }
            );
        });
    </script>

</asp:Content>
