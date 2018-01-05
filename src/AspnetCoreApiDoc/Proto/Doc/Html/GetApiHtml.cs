using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AspnetCoreApiDoc.Proto.Doc.Html
{
    internal class GetApiHtml
    {
        internal static string Get(string host, string wshost, string buildSvg, string coverageSvg)
        {
            var assembly = typeof(GetApiHtml).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("AspnetCoreApiDoc.Proto.Doc.Html.api.html");

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Dispose();
            var html = Encoding.UTF8.GetString(bytes)
                .Replace("{%host%}", host)
                .Replace("{%ws%}", wshost)
                .Replace("{%svg%}", string.IsNullOrWhiteSpace(buildSvg + coverageSvg)
                    ? ""
                    : $@"
<div class='version'>
    {(string.IsNullOrWhiteSpace(buildSvg) ? "" : $"<a href='javascript:void(0)'><img alt='编译状态' src='{buildSvg}' /></a>")}
    {(string.IsNullOrWhiteSpace(coverageSvg) ? "" : $"<a href='javascript:void(0)'><img alt='代码覆盖率' src='{coverageSvg}' /></a>")}
</div>
");
            return Regex.Replace(html, "\\n+\\s+", string.Empty);
        }
    }
}

/*
 //未压缩的JS 2017年9月6日 10:07:22
 //压缩工具地址：http://tool.css-js.com/
 
 ;var isSSL = 'https:' == document.location.protocol ? true : false;
        NProgress.configure({ ease: 'ease', speed: 500 });
        $(window).load(function () {
            var h = $('.wy-side-nav-search').height();
            $('.wy-menu-vertical').css("top", h + 28);

            NProgress.done();
        });
        $(document).ready(function () {
            NProgress.start();

            $('#rigthMenu').click(function () {
                $('html, body').animate({ scrollTop: 0 }, 'slow');
            });
            $('#searchtext').hideseek({ highlight: true, nodata: '没有找到相关的API' });
        });

        var rootHref = "api.do";

        function openPage(reginName, uuid) {
            window.location.href = rootHref + "#!" + reginName + "/" + uuid;
        }

        function CompileTemple(tid, uuid, hid) {
            var responseTp = document.getElementById(tid).innerHTML;
            laytpl(responseTp).render(getDomByUUID(uuid), function (b) {
                document.getElementById(hid).innerHTML = b;
            });
        }

        function getDomByUUID(key) {
            var jsonStr = sessionStorage.getItem(key);
            if (jsonStr != null) {
                return JSON.parse(jsonStr);
            }
        }

        function showTable(key) {
            var obj = getDomByUUID(key);
            Q.reg(obj.Param, function (aid) {
                if (!aid)
                    return alert('传入参数不正确，请确认您访问的地址。');
                NProgress.start();

                $("#requestDiv").html('');
                $("#responseDiv").html('');
                CompileTemple('ItemsTp', aid, 'main-div');

                NProgress.done();
            });
            openPage(obj.Param, obj.UUID);
        }

        function downProto(tag) {
            var fileName = tag + ".proto";
            var content = $("#down-" + tag).text();
            var file = new Blob([content], { type: "text/plain;charset=utf-8" });
            saveAs(file, fileName);
        }

        function recursion(pa) {
            //console.log(pa);
            if (pa == null) { return; }
            if (pa.ParamItems == null) {
                sessionStorage.setItem(pa.UUID, JSON.stringify(pa));
            } else {
                for (var i = 0; i < pa.ParamItems.length; i++) {
                    var innerOb = pa.ParamItems[i];
                    sessionStorage.setItem(innerOb.UUID, JSON.stringify(innerOb));

                    if (innerOb.ClassObj == null) {
                        continue;
                    } else {
                        recursion(innerOb.ClassObj);
                    }
                }
            }
        }

        function init(d) {
            $("#apiName").text(" " + d.ApiName);
            $("#Version").text(d.Version);
            $("#copyright").text(d.Copyright);
            $("title").text(d.ApiName + " - " + d.Version);

            console.log("%c" + d.ApiName, " text-shadow: 0 1px 0 #ccc,0 2px 0 #c9c9c9,0 3px 0 #bbb,0 4px 0 #b9b9b9,0 5px 0 #aaa,0 6px 1px rgba(0,0,0,.1),0 0 5px rgba(0,0,0,.1),0 1px 3px rgba(0,0,0,.3),0 3px 5px rgba(0,0,0,.2),0 5px 10px rgba(0,0,0,.25),0 10px 10px rgba(0,0,0,.2),0 20px 20px rgba(0,0,0,.15);font-size:4em")
            console.log("%c" + d.Copyright, " text-shadow: 0 1px 0 #ccc,0 2px 0 #c9c9c9,0 3px 0 #bbb,0 4px 0 #b9b9b9,0 5px 0 #aaa,0 6px 1px rgba(0,0,0,.1),0 0 5px rgba(0,0,0,.1),0 1px 3px rgba(0,0,0,.3),0 3px 5px rgba(0,0,0,.2),0 5px 10px rgba(0,0,0,.25),0 10px 10px rgba(0,0,0,.2),0 20px 20px rgba(0,0,0,.15);font-size:2em")
            console.log("likeli@jsj.com.cn")

            //右侧菜单
            for (var i = 0; i < d.DApiInfo.length; i++) {
                var singelMeth = d.DApiInfo[i];
                $("#rigthMenu").append(" <li class=\"toctree-l1\" ><a class=\"reference internal\" style=\"color: #cacaca;\" onclick=\"method('" + singelMeth.UUID + "')\" href=\"javascript:void(0);\" title=\"" + singelMeth.ApiName + "\">" + singelMeth.ApiName + "<br><span style=\"color: #7d7c7d\">" + singelMeth.ApiDesc + "</span></a></li> ");
            }

            //初始化网络文档
            for (var i = 0; i < d.NetworkDocs.length; i++) {
                var singelMeth = d.NetworkDocs[i];
                $("#networkDoc_rigthMenu").append(" <li class=\"toctree-l1\" ><a class=\"reference internal\" style=\"color: #cacaca;\" target=\"view_window\" href=\"" + singelMeth.Url + "\" href=\"javascript:void(0);\" title=\"" + singelMeth.Title + "\">" + singelMeth.Title + "</a></li> ");
            }

            //初始化localStorage
            sessionStorage.clear();
            for (var i = 0; i < d.DApiInfo.length; i++) {
                var firstDb = d.DApiInfo[i];
                sessionStorage.setItem(firstDb.UUID, JSON.stringify(firstDb));
                for (var j = 0; j < firstDb.Request.ParamItems.length; j++) {
                    var pa = firstDb.Request.ParamItems[j];
                    sessionStorage.setItem(pa.UUID, JSON.stringify(pa));
                    recursion(pa.ClassObj);
                }

                for (var j = 0; j < firstDb.Response.ParamItems.length; j++) {
                    var pa = firstDb.Response.ParamItems[j];
                    sessionStorage.setItem(pa.UUID, JSON.stringify(pa));
                    recursion(pa.ClassObj);
                }
            }
        }

        function changeSize() {
            var width = parseInt($("#Width").val());
            var height = parseInt($("#Height").val());
            $("#Demo").width(width).height(height);
            $('#Demo').perfectScrollbar('update');
            Ps.update(document.getElementById('Demo'));
        }

        function method(uid) {
            var apiInfo = getDomByUUID(uid);
            $("#methodName").text(apiInfo.ApiName);
            $("#methodUrl").text((isSSL ? 'https://' : 'http://') + window.location.host + '/' + apiInfo.Url);
            $("#methodNameDec").text(apiInfo.ApiDesc);
            Q.reg(apiInfo.Request.ClassName, function (aid) {
                if (!aid)
                    return alert('传入参数不正确，请确认您访问的地址。');

                NProgress.start();

                $("#main-div").html('');
                CompileTemple('requestTp', aid, 'requestDiv');
                CompileTemple('responseTp', aid, 'responseDiv');

                NProgress.done();
            });
            openPage(apiInfo.Request.ClassName, apiInfo.UUID);
        }

        function createItems(apiInfo) {
            //console.log(apiInfo);
            var requestTp = document.getElementById('requestTp').innerHTML;
            laytpl(requestTp).render(apiInfo, function (render) {
                document.getElementById('requestDiv').innerHTML = render;
            });
        }

        function clear() {
            $(".full-div").fadeOut("slow");
            setTimeout(function () {
                $(".full-div").remove();
            }, 700);
        }

        function notify(title, content) {
            try {
                if (!title && !content) {
                    title = "API服务中心消息";
                    content = "API文档更新提醒";
                }
                var iconUrl = "http://pic.hdf.kim/7dfd23f4a3dfe42eee1d6afb07cdfa5c";

                if (window.webkitNotifications) {
                    //chrome老版本
                    if (window.webkitNotifications.checkPermission() == 0) {
                        var notif = window.webkitNotifications.createNotification(iconUrl, title, content);
                        notif.display = function () { };
                        notif.onerror = function () { };
                        notif.onclose = function () { };
                        notif.onclick = function () { this.cancel(); };
                        notif.replaceId = 'Meteoric';
                        notif.show();
                    } else {
                        window.webkitNotifications.requestPermission($jy.notify);
                    }
                }
                else if ("Notification" in window) {
                    // 判断是否有权限
                    if (Notification.permission === "granted") {
                        var notification = new Notification(title, {
                            "icon": iconUrl,
                            "body": content
                        });
                    }
                    //如果没权限，则请求权限
                    else if (Notification.permission !== 'denied') {
                        Notification.requestPermission(function (permission) {
                            // Whatever the user answers, we make sure we store the
                            // information
                            if (!('permission' in Notification)) {
                                Notification.permission = permission;
                            }
                            //如果接受请求
                            if (permission === "granted") {
                                var notification = new Notification(title, {
                                    "icon": iconUrl,
                                    "body": content
                                });
                            }
                        });
                    }
                }
            } catch (e) { }
        }

        function check() {
            if ("Notification" in window) {
                if (Notification.permission !== 'denied') {
                    Notification.requestPermission(function (permission) {
                        if (!('permission' in Notification)) {
                            Notification.permission = permission;
                        }
                    });
                }
            }
        }

        var socket = new WebSocket((isSSL ? 'wss://' : 'ws://') + window.location.host + "{%ws%}");
        function doConnect() {
            var internvalId = window.setInterval(function () {
                try { socket.send('h+'); } catch (e) { };
            }, 5000);
            socket.onopen = function (e) { };
            socket.onclose = function (e) {
                clearInterval(internvalId);
                try {
                    var indexAlert = layer.open({
                        type: 1
                        , title: false
                        , closeBtn: false
                        , area: '300px;'
                        , shade: 0.8
                        , id: 'LAY_layuipro'
                        , btn: ['立刻刷新', '容老夫手动刷新']
                        , moveType: 1
                        , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">你知道吗？亲！<br>服务端的API文档更新了，因为本地使用了HTML5缓存，需要及时刷新才可以生效！</div>'
                        , yes: function (index, layero) {
                            layer.close(indexAlert)
                            setTimeout(function () {
                                window.location.reload();
                            }, 500);
                        }
                    });
                    notify("API服务消息", "服务端的API文档已更新,速度刷新游览最新文档.");
                } catch (e) {
                    alert("服务端API文档已经更新，需要刷新重新获取数据");
                    window.location.reload();
                };
            }
            socket.onerror = function (e) { alert("Error: " + e.data); };
        }

        //读取ProtoBuffer版本配置
        function protoversion(){
            if(typeof(localStorage.ProtoVersion) != 'undefined'){
                $("#proto_version").val(Number(localStorage.ProtoVersion));
            }else{
                localStorage.ProtoVersion = 3;
                $("#proto_version").val(3);
            }
        }

        $(function () {
            protoversion();
            $('#proto_version').val();
            check();
            var loadings = ["audioWave", "snake", "spinningDisc", "glisteningWindow", "circularSquare", "audioWave", "snake", "spinningDisc", "glisteningWindow", "circularSquare", "glisteningWindow", "circularSquare"];
            var loadingHtml = "<div class='loader loader--" + loadings[Math.floor(Math.random() * 10)] + "'></div>";
            $(".loading-center").html(loadingHtml);
            $("#proto_version").change(function(v){
                localStorage.ProtoVersion = $(this).val();
                window.location.reload();
            });

            $.post("{%host%}",{"":$("#proto_version").val()}, function (data) {
                init(data);
                method(data.DApiInfo[0].UUID);
                clear();
                doConnect();
            });
        });
 */