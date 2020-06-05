$.getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
};
$.gsLoading = function () {
    top.layer.load(1, {
        shade: [0.5, '#000'],
        isOutAnim: false,
        anim: -1,
    })
}

$.gsClostLoading = function () {
    top.layer.closeAll('loading');

}


$.ajaxPost = function (url, data, successCallback, failCallback) {
    top.layer.load(1, { offset: '300px' });
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        success: function (r) {
            if (r.success) {
                layer.msg(r.message,
                    {
                        icon: 1,
                        time: 1200
                    }, function () {
                        if (successCallback) {
                            successCallback();
                        }
                        top.layer.closeAll('loading');
                    });
            } else if (r.error) {
                top.layer.closeAll('loading');

                var msg = "服务器开小差中~请联系管理员";

                if (r.data && r.data.status == 403) {
                    window.location.href = "/home/error?value=" + r.message;
                } else {

                    if (r.message && r.message != "") {
                        msg = r.message;
                    }

                    layer.msg(msg,
                        {
                            icon: 0,
                            time: 2000
                        });
                }
            } else {
                layer.msg(r.message,
                    {
                        icon: 2,
                        time: 2000
                    }, function () {
                        if (failCallback) {
                            failCallback();
                        }
                        top.layer.closeAll('loading');
                    });
            }
        },
        error: function (xmlHttpRequest, textStatus) {
            top.layer.closeAll('loading');

            if (xmlHttpRequest.status == 403) {
                window.location.href = "/home/error?value=" + xmlHttpRequest.responseJSON.msg;
            } else {
                layer.msg("服务器开小差中~请联系管理员",
                    {
                        icon: 0,
                        time: 2000
                    });
            }

        },
        complete: function () {
            top.layer.closeAll('loading');
        }
    });
};

$.Delete = function (url, data, successCallback, failCallback, title) {
    var t = '确定删除?';
    if (title) {
        t = title;
    }

    layer.confirm(t,
        { icon: 3, title: '提示' },
        function (index) {
            layer.close(index);
            $.ajaxPost(url, data, successCallback, failCallback);
        });
};


$.confirmPost = function (url, data, successCallback, failCallback) {
    layer.confirm('是否确认提交?',
        { icon: 3, title: '提示' },
        function (index) {
            layer.close(index);
            $.ajaxPost(url, data, successCallback, failCallback);
        });
};
$.tableReload = function (formId, tableId) {
    var form = layui.form;
    var table = layui.table;
    form.on('submit(' + formId + ')',
        function (data) {
            var field = data.field;
            table.reload(tableId,
                {
                    where: field
                });
        });
};

$.openLayer = function (title, url, endCallBack, w, h, IsNoBtn) {
    var width = $(window).width() * 0.8 + 'px';
    var height = $(window).height() * 0.8 + 'px';

    if (w) {
        width = w + 'px';
    }
    if (h) {
        height = h + 'px';
    }
    var btns = ['确定', '取消']
    if (IsNoBtn) {
        btns = [];
    }
    layer.open({
        type: 2,
        title: title,
        content: url,
        area: [width, height],
        btn: btns,
        yes: function (index, layero) {
            var submit = layero.find('iframe').contents().find('#btn-submit');
            submit.trigger('click');

        },
        end: function () {
            endCallBack();
        }
    });
};

$.fn.extend({
    formPost: function (successCallback, failCallback) {
        var msg = "服务器开小差中~请联系管理员";
        top.layer.load(1, { offset: '300px' });
        $(this).ajaxSubmit({
            success: function (r) {
                if (r.message) {
                    msg = r.message;
                }
                if (r.success) {
                    layer.msg(msg, {
                        icon: 1
                        , time: 1200
                    }, function () {
                        if (successCallback) {
                            successCallback(r);
                        }
                        top.layer.closeAll('loading');
                    });
                }
                else if (r.error) {
                    top.layer.closeAll('loading');

                    var msg = "服务器开小差中~请联系管理员";

                    if (r.data && r.data.status == 403) {
                        window.location.href = "/home/error?value=" + r.message;
                    } else {

                        if (r.message && r.message != "") {
                            msg = r.message;
                        }

                        layer.msg(msg,
                            {
                                icon: 0,
                                time: 2000
                            });
                    }
                } else {
                    $.gsClostLoading();
                    layer.msg(msg, {
                        icon: 2
                        , time: 2000
                    }, function () {
                        if (failCallback) {
                            failCallback();
                        }
                        top.layer.closeAll('loading');
                    });
                }
            },
            error: function (xmlHttpRequest, textStatus) {
                top.layer.closeAll('loading');
                if (xmlHttpRequest.status == 403) {
                    window.location.href = "/home/error?value=" + xmlHttpRequest.responseJSON.msg;
                } else {
                    layer.msg("服务器开小差中~请联系管理员",
                        {
                            icon: 0,
                            time: 2000
                        });
                }
            }
        });
    },
    bindSelectData: function (url, value) {
        var $self = $(this);
        if (!$self.is('select')) {
            throw new Error('bindSelectData\'s extension requires select tag');
        }
        $.getJSON(url, { value: value }).done(function (d) {
            var html = "";

            if (d && d.length > 0) {
                $.each(d, function (i, v) {
                    html += '<option ' + (v.selected === true ? 'selected="selected"' : '') + ' value="' + v.value + '">' + v.text + '</option>';
                });
            }

            $self.append(html);

            layui.form.render('select');
        });
    },
    bindMultiSelectData: function (url, value, idStr) {

        var $self = $(this);
        if (!$self.is('select')) {
            throw new Error('bindSelectData\'s extension requires select tag');
        }
        var param = { value: value.split(",") };
        var qwe = jQuery.param(param, true);
        $.getJSON(url + "?" + qwe, null).done(function (d) {
            var html = "";

            if (d && d.length > 0) {
                $.each(d, function (i, v) {
                    html += '<option ' + (v.selected === true ? 'selected="selected"' : '') + ' value="' + v.value + '">' + v.text + '</option>';
                });
            }

            $self.append(html);

            layui.formSelects.render(idStr);
        });
    },
    bindCheckBoxData: function (url, value, name) {
        var $self = $(this);

        $.getJSON(url, { value: value }).done(function (d) {
            var html = "";

            if (d && d.length > 0) {
                $.each(d, function (i, v) {
                    html += '<input name="' + name + '" lay-skin="primary" title="' + v.text + '" type="checkbox" value="' + v.value + '" ' + (v.selected === true ? 'checked="checked"' : '') + ' />';
                });
            }

            $self.append(html);

            layui.form.render('checkbox');
        });
    },
    bindRadioData: function (url, value, name) {
        var $self = $(this);

        $.getJSON(url, { value: value }).done(function (d) {
            var html = "";

            if (d && d.length > 0) {
                $.each(d, function (i, v) {
                    html += '<input name="' + name + '" lay-skin="primary" title="' + v.text + '" type="radio" value="' + v.value + '" ' + (v.selected === true ? 'checked="checked"' : '') + ' />';
                });
            }

            $self.append(html);

            layui.form.render('radio');
        });
    },
    treeTableSearch: function (keyword) {
        var searchCount = 0;
        $(this).next('.treeTable').find('.layui-table-body tbody tr td').each(function () {
            $(this).css('background-color', 'transparent');
            var text = $(this).text();
            if (keyword != '' && text.indexOf(keyword) >= 0) {
                $(this).css('background-color', 'rgba(250,230,160,0.5)');
                if (searchCount == 0) {
                    layui.treetable.expandAll('#auth-table');
                    $('html,body').stop(true);
                    $('html,body').animate({ scrollTop: $(this).offset().top - 150 }, 500);
                }
                searchCount++;
            }
        });
        if (keyword == '') {
            layer.msg("请输入搜索内容", { icon: 5 });
        } else if (searchCount == 0) {
            layer.msg("没有匹配结果", { icon: 5 });
        }
    }
});


Array.prototype.arrayColumn = function (name) {
    var newArr = [];
    var type = Object.prototype.toString.call(name);
    this.forEach(data => {
        if (type === '[object Array]') {
            var obj = {};
            name.forEach((_name) => {
                obj[_name] = data[_name];
            });
            newArr.push(obj);
        } else if (type === '[object String]') {
            newArr.push(data[name]);
        }
    });
    return newArr;
};

$.Editor = function () {
    //实例化编辑器
    //建议使用工厂方法getEditor创建和引用编辑器实例，如果在某个闭包下引用该编辑器，直接调用UE.getEditor('editor')就能拿到相关的实例
    var ue = UE.getEditor('editor');


    function isFocus(e) {
        alert(UE.getEditor('editor').isFocus());
        UE.dom.domUtils.preventDefault(e);
    }
    function setblur(e) {
        UE.getEditor('editor').blur();
        UE.dom.domUtils.preventDefault(e);
    }
    function insertHtml() {
        var value = prompt('插入html代码', '');
        UE.getEditor('editor').execCommand('insertHtml', value);
    }
    function createEditor() {
        enableBtn();
        UE.getEditor('editor');
    }
    function getAllHtml() {
        alert(UE.getEditor('editor').getAllHtml());
    }
    function getContent() {
        var arr = [];
        arr.push("使用editor.getContent()方法可以获得编辑器的内容");
        arr.push("内容为：");
        arr.push(UE.getEditor('editor').getContent());
        alert(arr.join("\n"));
    }
    function getPlainTxt() {
        var arr = [];
        arr.push("使用editor.getPlainTxt()方法可以获得编辑器的带格式的纯文本内容");
        arr.push("内容为：");
        arr.push(UE.getEditor('editor').getPlainTxt());
        alert(arr.join('\n'));
    }
    function setContent(isAppendTo) {
        var arr = [];
        arr.push("使用editor.setContent('欢迎使用ueditor')方法可以设置编辑器的内容");
        UE.getEditor('editor').setContent('欢迎使用ueditor', isAppendTo);
        alert(arr.join("\n"));
    }
    function setDisabled() {
        UE.getEditor('editor').setDisabled('fullscreen');
        disableBtn("enable");
    }

    function setEnabled() {
        UE.getEditor('editor').setEnabled();
        enableBtn();
    }

    function getText() {
        //当你点击按钮时编辑区域已经失去了焦点，如果直接用getText将不会得到内容，所以要在选回来，然后取得内容
        var range = UE.getEditor('editor').selection.getRange();
        range.select();
        var txt = UE.getEditor('editor').selection.getText();
        alert(txt);
    }

    function getContentTxt() {
        var arr = [];
        arr.push("使用editor.getContentTxt()方法可以获得编辑器的纯文本内容");
        arr.push("编辑器的纯文本内容为：");
        arr.push(UE.getEditor('editor').getContentTxt());
        alert(arr.join("\n"));
    }
    function hasContent() {
        var arr = [];
        arr.push("使用editor.hasContents()方法判断编辑器里是否有内容");
        arr.push("判断结果为：");
        arr.push(UE.getEditor('editor').hasContents());
        alert(arr.join("\n"));
    }
    function setFocus() {
        UE.getEditor('editor').focus();
    }
    function deleteEditor() {
        disableBtn();
        UE.getEditor('editor').destroy();
    }
    function disableBtn(str) {
        var div = document.getElementById('btns');
        var btns = UE.dom.domUtils.getElementsByTagName(div, "button");
        for (var i = 0, btn; btn = btns[i++];) {
            if (btn.id === str) {
                UE.dom.domUtils.removeAttributes(btn, ["disabled"]);
            } else {
                btn.setAttribute("disabled", "true");
            }
        }
    }
    function enableBtn() {
        var div = document.getElementById('btns');
        var btns = UE.dom.domUtils.getElementsByTagName(div, "button");
        for (var i = 0, btn; btn = btns[i++];) {
            UE.dom.domUtils.removeAttributes(btn, ["disabled"]);
        }
    }

    function getLocalData() {
        alert(UE.getEditor('editor').execCommand("getlocaldata"));
    }

    function clearLocalData() {
        UE.getEditor('editor').execCommand("clearlocaldata");
        alert("已清空草稿箱");
    }
}


