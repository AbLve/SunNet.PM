<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testJson.aspx.cs" Inherits="SunNet.PMNew.Web.testJson" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Scripts/Validate/regex.js" type="text/javascript"></script>

    <script src="Scripts/jquery-1.8.0.js" type="text/javascript"></script>

    <script type="text/javascript" src="Scripts/Validate/Validator.js"></script>
    <style type="text/css">
        .star, .color
        {
            position: absolute;
            left: 0px;
            top: 0px;
            width: 115px;
            height: 20px;
        }
        .star
        {
            background-image: url(/icons/stars.png);
            background-position: initial initial;
            background-repeat: repeat no-repeat;
        }
        .color
        {
            position: absolute;
            width: 46px;
            background-color: Yellow;
        }
        .star div
        {
            cursor: pointer;
            margin: 0px;
            padding: 0px;
            display: inline-block;
            width: 23px;
            height: 20px;
        }
    </style>

    <script type="text/javascript">
        function FormatPhone(sender) {
            var source = sender.value;
            var number = "0123456789";
            var output = "";
            for (var i = 0; i < source.length; i++) {
                if (number.indexOf(source.charAt(i)) > -1) {
                    output = output + source.charAt(i);
                    if (output.length == 3 || output.length == 7) {
                        output = output + "-";
                    }
                }
            }
            while (output.charAt(output.length - 1) == "-") {
                output = output.substr(0, output.length - 1);
            }
            if (output.length > 12) {
                output = output.substr(0, 12);
            }
            sender.value = output;
        }
        jQuery(function() {
            jQuery("#btnAdd").click(function() {
                var pContainer = jQuery("#testlive");
                pContainer.append("<span  class='abc'>" + (pContainer.children().length + 1) + "</span>");
            });
            jQuery("span.abc").live("click", function() {
                alert(jQuery(this).html());
            });
        });
        Validate();
        function Checkbox1_onclick(event) {
            if (jQuery.browser.msie) {
                window.event.cancelBubble = true;
            }
            else if (event && event.stopPropagation) {
                event.stopPropagation();
            }
            alert("Checkbox1_onclick");
        }

        function parent_onclick(event) {
            alert("parent");
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <div class="color">
                </div>
                <div class="star">
                    <div title="1 of 5 stars">
                    </div>
                    <div title="2 of 5 stars">
                    </div>
                    <div title="3 of 5 stars">
                    </div>
                    <div title="4 of 5 stars">
                    </div>
                    <div title="5 of 5 stars">
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <input id="txtPhone" onkeyup="FormatPhone(this);" type="text" />
    <div>
        <asp:TextBox ID="TextBox1" TextMode="MultiLine" runat="server" Height="213px" Width="511px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" /><asp:TextBox
            ID="TextBox2" runat="server"></asp:TextBox>
    </div>
    <br />
    <div>
        <p id="testlive">
        </p>
        <input id="btnAdd" type="button" value="Add Node" />&nbsp;<input id="btnClick" type="button"
            value="Click" />
    </div>
    <br />
    <div id="parent" onclick="return parent_onclick(event)">
        parent
        <input id="Checkbox1" type="checkbox" onclick="return Checkbox1_onclick(event)" />
        <div>
            child</div>
    </div>
    <div id="uploadfile"> 
        <asp:FileUpload ID="FileUpload1" runat="server" multiple/>
    </div>
    </form>
</body>
</html>
