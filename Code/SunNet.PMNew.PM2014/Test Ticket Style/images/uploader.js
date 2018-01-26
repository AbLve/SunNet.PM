function getUploaderPrefix() {
    return "";
}

WebUploader.Uploader.register({
    'before-send-file': 'beforeSendFile',
    'before-send': 'beforeSend'
}, {
    beforeSendFile: function (file) {
        var uploader = this;
        var serverurl = this.options.server;
        var deferred = WebUploader.Deferred();
        var data = $.extend({}, {
            id: file.id,
            name: file.name,
            type: file.type,
            lastModifiedDate: file.lastModifiedDate,
            size: file.size,
            chunk: 0,
            chunks: 0,
            prefix: file.prefix
        });
        jQuery.ajax({
            url: serverurl,
            type: "post",
            data: data,
            success: function (response, textStatus, xhr) {
                if (xhr.statusText == "Existed") {
                    uploader.owner.skipFile(file);
                    deferred.resolve();
                }
                else if (xhr.statusText == "TooLarge") {
                    //uploader.owner.skipFile(file);
                    var $state = $('#' + file.id).find('.state');
                    $state.text($state.text() + ", file size too large");
                    uploader.owner.removeFile(file);
                    deferred.reject();
                }
                else if (xhr.statusText == "Illegal") {
                    //uploader.owner.skipFile(file);
                    var $state = $('#' + file.id).find('.state');
                    $state.text($state.text() + ", illegal file type");
                    uploader.owner.removeFile(file);
                    deferred.reject();
                }
                else {
                    deferred.resolve();
                }
            },
            error: function () {
                deferred.resolve();
            }
        });
        return deferred.promise();
    },
    beforeSend: function (block) {
        var serverurl = this.options.server;
        var deferred = WebUploader.Deferred();
        var file = block.file;
        var data = $.extend({}, {
            id: file.id,
            name: file.name,
            type: file.type,
            lastModifiedDate: file.lastModifiedDate,
            size: file.size,
            chunk: block.chunk,
            chunks: block.chunks,
            prefix: file.prefix
        });
        jQuery.ajax({
            url: serverurl,
            type: "post",
            data: data,
            success: function (response, textStatus, xhr) {
                if (xhr.statusText == "Existed")
                    deferred.reject();
                else
                    deferred.resolve();
            },
            error: function () {
                deferred.resolve();
            }
        });
        return deferred.promise();
    }
});

var SunnetWebUploader = {
    defaultOptions: {
        chunked: true,
        threads: 5,
        chunkSize: 5242880,
        resize: null,
        duplicate: 10,

        prepareNextFile: true,
        fileNumLimit: 1,
        // swf文件路径
        swf: '/Scripts/webuploader/Uploader.swf',
        // 文件接收服务端。
        server: '/Service/FileUploader.ashx',
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: { id: '#picker', multiple: false }
    },
    getOptions: function (options) {
        return jQuery.extend({}, this.defaultOptions, options);
    },
    getSuffix: function (files) {
        if (!files || files.length == 0)
            return "";
        var lastIndex = files.length - 1;
        var lastFile = files[lastIndex];
        var lastFilename = lastFile.name;
        var pre, suf, indexNow;

        // /(.*)\((\d)\)(\.\w*)$/.exec("ab.c(1).jpg") = ["ab.c(1).jpg", "ab.c", "1", ".jpg"]
        var matchItems = /(.*)\((\d)\)(\.\w*)$/.exec(lastFilename);
        if (matchItems && matchItems.length) {
            pre = matchItems[1];
            suf = matchItems[3];
            indexNow = +matchItems[2];
        }
        else {
            // /(.*)(\.\w*)$/.exec("ab.c.jpg") = ["ab.c.jpg", "ab.c", ".jpg"]
            matchItems = /(.*)(\.\w*)$/.exec(lastFilename);
            if (matchItems && matchItems.length) {
                pre = matchItems[1];
                suf = matchItems[2];
                indexNow = 0;
            }
        }
        jQuery.each(files, function (findex, file) {
            if (findex < lastIndex) {
                var matchItems2 = /(.*)\((\d)\)(\.\w*)$/.exec(file.name);
                if (matchItems2 && matchItems2.length) {
                    if (matchItems2[1] == pre && matchItems2[3] == suf) {
                        indexNow = Math.max(indexNow, +matchItems2[2]);
                        indexNow++;
                    }
                } else {
                    matchItems2 = /(.*)(\.\w*)$/.exec(file.name);
                    if (matchItems2 && matchItems2.length) {
                        if (matchItems2[1] == pre && matchItems2[2] == suf) {
                            indexNow = 1;
                        }
                    }
                }
            }
        });
        return pre + (indexNow == 0 ? "" : "(" + indexNow + ")") + suf;
    },

    /*
     * 页面需要重写getUploaderPrefix函数来返回每个页面自己的上传前缀
     * 参数说明：内部已经处理了一些统一的参数，每个页面可定制参数
     * options参数至少需要：
     * pick:选择文件触发器
     * container:文件夹列表容器
     * targetField:上传成功之后赋值给哪个控件(仅使用于单文件上传，多文件请自己重写uploadSuccess,uploadComplete事件)
     * 可选参数：
     * speed:速度提示
     * uploadbutton:触发上传的按钮
     * submitbutton:表单提交按钮（data:clicked 为true表示，已经点击提交）
     * filetemplate:文件列表模版
     * showProgress:是否显示每个文件上传进度
     */
    CreateWebUploader: function (options) {
        var $list, $uploadbutton, $submitbutton, template, $speed, $picker, $form, showProgress, $targetField;
        if (options.pick) {
            var pickerSelector = typeof options.pick == "string" ? options.pick : options.pick.id;
            $picker = $(pickerSelector);
            $form = $picker.closest("form");
        }
        if (options.container) $list = $(options.container);
        if (options.uploadbutton) $uploadbutton = $form.find(options.uploadbutton);
        if (options.submitbutton) $submitbutton = $form.find(options.submitbutton);
        if (options.speed) $speed = $form.find(options.speed);
        if (options.targetField) $targetField = $(options.targetField);

        template = options.filetemplate || '<div id="{% this.id %}" class="item">' +
                '<span class="info">{% this.name %}</span>&nbsp;: &nbsp;' +
                '<span class="state">Done</span>&nbsp;&nbsp;<span class="delete" title="Remove this file"><a href="javascript:;">&times;</a></span>' +
                '</div>';
        showProgress = options.showProgress || true;
        options = this.getOptions(options);
        var uploader = WebUploader.create(options);
        // 删除文件
        $list && $list.on("click", ".delete", function () {
            var $this = $(this);
            var $file = $this.closest("div.item");
            var fileID = $file.attr("id");
            //console.log(fileID);
            uploader.removeFile(fileID);
            $file.remove();
        });
        // 分块上传之前
        uploader.on('uploadBeforeSend', function (block, params) {
            //console.log(block);
            if (!timeStart) timeStart = new Date();
            params.prefix = block.file.prefix;
        });
        // 当有文件添加进来的时候
        uploader.on('fileQueued', function (file) {
            //console.log(file.source.ruid, file.source.uid);
            file.prefix = getUploaderPrefix();
            file.name = SunnetWebUploader.getSuffix(uploader.getFiles());
            $list.append(TemplateEngine(template, file));
        });

        var totalTime = 0, totalBytes = 0, timeStart;
        // 文件上传过程中创建进度条实时显示。
        uploader.on('uploadProgress', function (file, percentage) {
            var $li = $form.find('#' + file.id),
                $percent = $li.find('.progress .progress-bar');
            totalTime += (new Date() - timeStart) / 1000;
            if (totalTime < 1) totalTime = 1;
            totalBytes += file.size * percentage / (1000);
            if ($speed) {
                $speed.html("Average speed：" + (totalBytes / totalTime).toFixed(2) + "KB/S   Uploaded:" + (percentage * 100).toFixed(2) + "%").show();
            }
            if (showProgress) {
                // 避免重复创建
                if (!$percent.length) {
                    $percent = $('<div class="progress progress-striped active">' +
                        '<div class="progress-bar" role="progressbar" style="width: 0%;">' +
                        '</div>' +
                        '</div>').appendTo($li).find('.progress-bar');
                }
                $li.find('.state').text('Uploading');
                $percent.css('width', percentage * 100 + '%');
                if (percentage >= 1) {
                    $('#' + file.id).find('.state').text('Being processed...');
                }
            }
        });
        uploader.on('uploadSuccess', function (file, responseText) {
            uploader.removeFile(file);
            $form.find('#' + file.id).find('.state').text('Uploaded');
            if ($targetField && $targetField.length) {
                var filenames = $targetField.val();
                if ($targetField.val())
                    filenames += "|";
                filenames += file.prefix + file.name;
                $targetField.val(filenames);
            }
        });
        uploader.on('uploadError', function (file) {
            var $state = $('#' + file.id).find('.state');
            $state.text($state.text() + ', upload failed.');
            if ($submitbutton.data("clicked")) {
                $submitbutton.click();
            }
        });
        uploader.on('uploadComplete', function (file) {
            //console.log("uploadComplete");
            $form.find('#' + file.id).find('.progress').fadeOut();
            var stats = uploader.getStats();
            if (uploader.getFiles("progress") == 0 && $submitbutton.data("clicked")) {
                $submitbutton.removeData("clicked");
                $submitbutton.click();
            }
        });
        if ($uploadbutton) {
            $uploadbutton.on('click', function () {
                if (uploader.state === 'uploading') {
                    uploader.stop();
                } else {
                    uploader.upload();
                    timeStart = new Date();
                }
                return false;
            });
        }
        return uploader;
    }
}
