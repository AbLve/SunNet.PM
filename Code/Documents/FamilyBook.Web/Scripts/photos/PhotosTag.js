var Allfriends = null;
var divUI = null;
String.prototype.trim = function ()
{
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.ltrim = function ()
{
    return this.replace(/(^\s*)/g, "");
}
String.prototype.rtrim = function ()
{
    return this.replace(/(\s*$)/g, "");
}
function AddTag()
{
    $("#tempTagBoxForm").hide();
    $('#draggable').show();
    HideAllTags();
    var left = $('#imgViewPhoto' + currentPhoto.ID).position().left;
    var top = $('#imgViewPhoto' + currentPhoto.ID).position().top;
    $('#draggable').css("left", left);
    $('#draggable').css("top", top);
    var cursorAtX = $('#draggable').width() / 2;
    var cursorAtY = $('#draggable').height() / 2;
    var contain = '#imgViewPhoto' + currentPhoto.ID;

    $('#draggable').draggable(
                                {
                                    drag: Draging

                                    , stop: DragStop
                                     , appendTo: '#imgViewPhoto' + currentPhoto.ID
                                   , containment: contain
                                    , scroll: true
                                });
    $('#draggable').resizable({ start: ResizeStart, stop: ResizeStop, containment: contain });
}
function ResizeStart()
{
    $('#draggable').css("cursor", "ew-resize");
    $("#tempTagBoxForm").hide();
}

function ResizeStop(event, ui)
{
    ShowFriendForm(ui);
}
function Draging()
{
    $('#draggable').css("cursor", "move");
    $("#tempTagBoxForm").hide();
    $("#divFriendsList").hide();
}
function DragStop(event, ui)
{
    ShowFriendForm(ui);
}
function ShowFriendForm(ui)
{
    divUI = ui;

    $("#tempTagBoxForm").show();
    $('#tempTagBoxForm').css("left", ui.position.left);
    $('#tempTagBoxForm').css("top", ui.position.top + ui.helper.height() + 8);
    $('#divFriendsList').css("left", ui.position.left);
    $("#divFriendsList").css("top", ui.position.top + ui.helper.height() + 50);
   
}
function LoadAllFriends()
{
    $("#imgLoading").show();
    $.get("/Photos/Photos/SearchFriends",
            function (data)
            {
                var friendsList = eval(data);
                Allfriends = friendsList;
                for (var i = 0; i < Allfriends.length; i++)
                {
                    var firstName = Allfriends[i].FirstName;
                    var lastName = Allfriends[i].LastName;
                    var Email = Allfriends[i].Email;
                    var liStr = "";
                    if (Allfriends[i].LittleHeadImage == "")
                        liStr = "<li style='padding-bottom:2px;' title=\"" + firstName + " " + lastName + "\" id=\"li_" + Allfriends[i].ID + "\" onclick='SelectedFriend(" + Allfriends[i].ID + ",this)'><img   src=\"/Images/icons/userman_sample.png\" width=\"30\" height=\"30\" /> " + (firstName + " " + lastName).subName(13) + "</li>";
                    else
                        liStr = "<li title=\"" + firstName + " " + lastName + "\" id=\"li_" + Allfriends[i].ID + "\" style=\"margin-bottom:5px;text-align:left;padding-left:5px\" onclick='SelectedFriend(" + Allfriends[i].ID + ",this)'><img  src=\"" + Allfriends[i].staticHeadImage + "\" width=\"30\" height=\"30\" />" + (firstName + " " + lastName).subName(13) + "</li>";

                    $("#ulFriends").append(liStr);

                }
             
                 $("#imgLoading").hide();
                 $("#ulFriends").show();
                 SearchFriends(0);
            });

}
function ShowFriends(e)
{
    if (Allfriends == null)
        LoadAllFriends();
    else
    SearchFriends(e);
    $("#divFriendsList").show();
    $("#imgHideFriends").show();
    $("#imgShowFriends").hide();

}
function HideFriends()
{
    $("#imgHideFriends").hide();
    $("#imgShowFriends").show();
    $("#divFriendsList").hide();
    ShowAllTags();
}
function SelectedFriend(id, obj)
{
    $("#hiddenUserId").val(id);
    $("#txtFriebdName").val(obj.title);
    HideFriends();
}

function SubmitSelectFriend()
{
    if ($("#txtFriebdName").val().trim() == "")
    {
        jQuery.alert("info", "No friend entered or selected.");
        return;
    }
    var userId = $("#hiddenUserId").val();
    var photoId = currentPhoto.ID;

    var tagFrom = "";
    if (this.location.href.toLowerCase().indexOf("viewotherusersphoto") >= 0)
        tagFrom = "viewotherusersphoto";
    else if (this.location.href.toLowerCase().indexOf("viewphoto") >= 0)
        tagFrom = "viewphoto";
    else if (this.location.href.toLowerCase().indexOf("viewfavoritephoto") >= 0)
        tagFrom = "viewfavoritephoto";

    $.post("/Photos/Photos/AddTag",
        {
            photoIdstr: photoId,
            userIdstr: userId,
            top: divUI.position.top,
            left: divUI.position.left,
            width: divUI.helper.width(),
            height: divUI.helper.height(),
            tagFrom: tagFrom,
            userName: $("#txtFriebdName").val().trim()
        },
            function (data)
            {
                HideFriends();
                $("#hiddenUserId").val("");
                $("#txtFriebdName").val("");
                $("#draggable").hide();
                $("#tempTagBoxForm").hide();
                var list = eval(data);
                LoadTags(list);
            });

}
function CancelSelectFriend()
{
    HideFriends();
    $("#hiddenUserId").val("");
    $("#txtFriebdName").val("");
    $("#draggable").hide();
    $("#tempTagBoxForm").hide();

}
function SearchFriends(e)
{
    var keyword = $("#txtFriebdName").val();
    if (keyword == null)
        keyword = "";
        //if (Allfriends == null)
        //  LoadAllFriends();
    else
    {
        for (var i = 0; i < Allfriends.length; i++)
        {

            var firstName = Allfriends[i].FirstName;
            var lastName = Allfriends[i].LastName;
            var Email = Allfriends[i].Email;
            if (firstName.toLowerCase().indexOf(keyword.toLowerCase().trim()) >= 0
            || lastName.toLowerCase().indexOf(keyword.toLowerCase().trim()) >= 0
            || Email.toLowerCase().indexOf(keyword.toLowerCase().trim()) >= 0
            || (firstName.toLowerCase() + " " + lastName.toLowerCase()).indexOf(keyword.toLowerCase().trim()) >= 0)
            {
                if ($("#tagUser_" + Allfriends[i].ID).length <= 0)
                    $("#li_" + Allfriends[i].ID).show();
                else
                    $("#li_" + Allfriends[i].ID).hide();
            }
            else
            {
                $("#li_" + Allfriends[i].ID).hide();
            }
        }
    }
}
function CancelAllTags()
{
    $(".divTags").remove();
}
function HideAllTags()
{
    $(".divTags").css("display", "none");
}
function ShowAllTags()
{
    $(".divTags").css("display", "block");
}
function ShowPhotoTag(tagId)
{
    $("#div_Tag_" + tagId).find("div").removeClass("hideTags");
    $("#div_Tag_" + tagId).find("div").addClass("showTags");
}
function HidePhotoTag(tagId)
{
    $("#div_Tag_" + tagId).find("div").removeClass("showTags");
    $("#div_Tag_" + tagId).find("div").addClass("hideTags");
}
function DeleteTag(id, photo)
{
    $.get("/Photos/Photos/DeleteTag", { tagID: id, photoId: photo }, DeleteTag_CallBack);
}
function DeleteTag_CallBack(data)
{
    var list = eval(data);
    LoadTags(list);
}
function IsCanDelete(tagCreatedBy)
{
    var photoCreatedBy = currentPhoto.CreatedBy;
    if (currentUID == photoCreatedBy)
        return true;
    else if (tagCreatedBy == currentUID)
        return true;
    else
        return false;
}
function LoadTags(list)
{
    $(".divTags").remove();
    $("#ultags").empty();
    var liTag = "";
    var divTag = "";
     for (var i = 0; i < list.length; i++)
     {//onmousemove onmouseout 
         var userLnk = "<a href='/Users/User/MainPage/{userId}'>{UserName}</a>";
         var userLnk2 = "<a style='color:white;text-decoration:none' href='/Users/User/MainPage/{userId}'>{UserName}</a>"
         var delLnk = "<img style='cursor:pointer;float:right;right:0xp;'  title='Delete this tag' onclick=\"DeleteTag({tagId},{photoId})\" src='/Images/icons/close_notice.png'/>";
         liTag = "<li style=\" padding:0px 10px 0px 0px;width:160px \" id='tagUser_{userId}' onmousemove='ShowPhotoTag({tagId})' onmouseout='HidePhotoTag({tagId})'>{isVirtual}<img style=\"float:left\" src=\"{headImg}\"  width=\"30\" height=\"30\" /> {isCanDel}</li>";
         divTag = "<div onmousemove='ShowPhotoTag({tagId})' onmouseout='HidePhotoTag({tagId})' id=\"div_Tag_{tagId}\" class=\"divTags\"><div style=\"position: absolute; top: {nametop}px; left: {left}px;\" class=\"innerTag \">{isVirtual}</div><div class=\"photoTag-tag \" id=\"photoTag-tag_{tagId}\" style=\"cursor:default;position: absolute; top: {top}px; left: {left}px; height: {height}px; width: {width}px;\"></div></div>";

        if (IsCanDelete(list[i].CreatedBy))
        {
            liTag = liTag.replace(/{isCanDel}/g, delLnk);
        }
        else
        {
            liTag = liTag.replace(/{isCanDel}/g, "");
        }
        if (list[i].UserId == 0)
        {
            liTag = liTag.replace(/{isVirtual}/g, "{UserName}");
            divTag = divTag.replace(/{isVirtual}/g, "{UserName}")
        }
        else
        {
           
            liTag = liTag.replace(/{isVirtual}/g, userLnk);
            divTag = divTag.replace(/{isVirtual}/g, userLnk2)
           
        }

        divTag = divTag.replace(/{tagId}/g, list[i].ID);
        divTag = divTag.replace(/{top}/g, list[i].Top);
        divTag = divTag.replace(/{left}/g, list[i].Left);
        divTag = divTag.replace(/{nametop}/g, (list[i].Top + list[i].Height + 5));

        divTag = divTag.replace(/{height}/g, list[i].Height);
        divTag = divTag.replace(/{width}/g, list[i].Width);
        divTag = divTag.replace(/{UserName}/g, list[i].UserName.subName(13));
        divTag = divTag.replace(/{photoId}/g, list[i].PhotoId);
        divTag = divTag.replace(/{userId}/g, list[i].UserId);

        liTag = liTag.replace(/{headImg}/g, list[i].headImage);
        liTag = liTag.replace(/{userId}/g, list[i].UserId);
        liTag = liTag.replace(/{UserName}/g, list[i].UserName.subName(13));
        liTag = liTag.replace(/{tagId}/g, list[i].ID);
        liTag = liTag.replace(/{photoId}/g, list[i].PhotoId);
        $("#ultags").append(liTag);
        $("#tdPhotoViews").append(divTag);
        HidePhotoTag(list[i].ID);

    }
}
function RequestTags(photoId)
{
    $.get("/Photos/Photos/RequestTags",
       {
           photoId: photoId
       },
           function (data)
           {
               var list = eval(data);
               LoadTags(list);
           });
}